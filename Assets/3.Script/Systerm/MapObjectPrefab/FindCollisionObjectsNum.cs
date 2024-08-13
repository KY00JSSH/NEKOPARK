using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindCollisionObjectsNum : MonoBehaviour {
    // 1. 목적 : objectPrefab들 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지 return
    // 검출하는 레이어 : 8번 레이어 붙어있는지 검출함

    private int collObjectsNum; // 검출된 오브젝트 개수
    private int layerMask = (1 << 8);
    private float raycastCheckDistance = 500f; // raycast를 확인할 거리
    public bool isStartCollision { get; private set; }

    private List<GameObject> collObjectsList = new List<GameObject>(); // 검출된 오브젝트 List

    public int GetCollObjectsNum() { return collObjectsNum; }
    public List<GameObject> GetCollObjectsList() { return collObjectsList; }



    private Vector2 saveDirectionVector; // 충돌 물체 방향 저장

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            isStartCollision = true;
            saveDirectionVector = (collision.transform.position - transform.position); // collision과 충돌된 방향 저장
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            isStartCollision = true;
            InitCollisionObjectsNum();
            GetRaycastArray(saveDirectionVector, collision); ;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isStartCollision = false;
        InitCollisionObjectsNum();
    }

    // 오브젝트 갯수 초기화
    public void InitCollisionObjectsNum() {
        collObjectsNum = 0;
        collObjectsList.Clear();
    }

    // Raycast로 오브젝트 전체 배열에 담음
    private void GetRaycastArray(Vector2 collDirection, Collision2D collision) {
        Vector3 rayOrigin = transform.position;

        Vector3 rayDirection = (collision.transform.position - rayOrigin).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, rayDirection, raycastCheckDistance, layerMask);
        //Debug.LogWarning("hits.Length 1 ???? " + hits.Length);
        if (hits[0].collider.gameObject == transform.gameObject) {
            hits = hits.Where(hit => hit.collider.gameObject != transform.gameObject).ToArray();  // 자기 자신일 경우 제외 (TextBOX)
        }

        if (hits.Length >= 2) {
            CoundtColliderObjectsNum(hits, collDirection);
        }
        else {
            collObjectsList.Add(collision.gameObject);
            collObjectsNum = 1;
        }
        Debug.LogWarning("collObjectsNum 검출갯수 확인 : " + collObjectsNum);
    }

    /*     
    raycast 배열에 담아져있는 모든 객체 조사
    정지되어있는 오브젝트의 충돌된 면을 기준 => 움직이려는 오브젝트의 양방향에 물체가 맞닿아있는지 확인

    검사 방향 순서
    1. 움직이려는 오브젝트를 기준으로 정지되어있는 오브젝트와 충돌한 방향
    2. 움직이려는 오브젝트를 기준으로 충돌 반대 방향  

    - 1번과 연결된 마지막 오브젝트 보정
    */
    private void CoundtColliderObjectsNum(RaycastHit2D[] hits, Vector2 collDirection) {
        for (int i = 0; i < hits.Length; i++) {
            if (i == 0) {
                collObjectsNum++;
                collObjectsList.Add(hits[i].collider.gameObject);
                continue;
            }

            CheckCollision checkCollision = hits[i].collider.GetComponent<CheckCollision>();

            if (Mathf.Abs(collDirection.y) > Mathf.Abs(collDirection.x)) {
                if (collDirection.y > 0) { // 위로 탑쌓기
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Bottom)) {
                        collObjectsNum++;
                        collObjectsList.Add(hits[i].collider.gameObject);
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.Top)) { break; }
                    }
                    else {
                        break;
                    }
                }
                else { // 아래로 탑쌓기 이럴 일이 있나?
                    //Debug.Log("bottom side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Top)) {
                        collObjectsNum++;
                        collObjectsList.Add(hits[i].collider.gameObject);
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.Bottom)) { break; }
                    }
                    else {
                        break;
                    }
                }
            }
            else {
                if (collDirection.x < 0) { // 왼쪽
                    //Debug.Log("Left side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Left)) {
                        collObjectsNum++;
                        collObjectsList.Add(hits[i].collider.gameObject);
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.Right)) { break; }
                    }
                    else {
                        break;
                    }
                }
                else { // 오른쪽
                    //Debug.Log("Right side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Right)) {
                        collObjectsNum++;
                        collObjectsList.Add(hits[i].collider.gameObject);
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.Left)) { break; }
                    }
                    else {
                        break;
                    }
                }
            }

        }
    }


}

/*  
 1. 목적 : objectPrefab들 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지
 2. 내용
    2-1. 처리 내용
        1) int 값 return : 해당 오브젝트와 충돌된 오브젝트가 몇 개인지
            => 예상 사용처 : 스프링, 박스, 움직이는 벽
    2-2. 처리 방법
        1) OnCollisionEnter2D -> 충돌이 들어왔을 경우 방향 저장
        2) OnCollisionStay2D -> 충돌 중 일 경우 Raycast 배열 저장 =>  GetRaycastArray(Vector2 collDirection, Collision2D collision)
        3) CoundtColliderObjectsNum(hits, collDirection) 
            -> Raycast 배열에 저장되어있는 오브젝트들의 getcomponent로 CheckCollision의 현재 충돌이 되어있는 면을 확인
            -> 
 */