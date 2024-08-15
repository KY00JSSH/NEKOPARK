using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindCollisionObjects : MonoBehaviour {
    // 1. 목적 : 스프링 프리펩 오브젝트 기능 구현 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지  / 다른 오브젝트를 날려버리기 
    // 검출하는 레이어 : 8번 레이어 붙어있는지 검출함

    private int layerMask = (1 << 8);

    private float raycastCheckDistance = 500f;                                  // raycast를 확인할 거리
    [SerializeField] private float objectWidth;                                 // 오브젝트의 가로 반쪽 길이(raycast를 2개로 나눔)       

    private Vector2 transformPosition;                                           // 스프링 위치 저장
    private Vector2 leftOrigin;                                                  // 스프링 왼쪽 위치
    private Vector2 rightOrigin;                                                 // 스프링 오른쪽 위치

    private Vector2 addForceVector;                                              // 충돌 물체 방향 확인
    private Vector2 saveDirectionVector;                                         // 충돌 물체 방향 초기 저장

    private RaycastHit2D[] hitsRight;
    private RaycastHit2D[] hitsLeft;

    [SerializeField] private List<GameObject> collObjectsListLeft = new List<GameObject>();       // 검출된 오브젝트 Left List
    [SerializeField] private List<GameObject> collObjectsListRight = new List<GameObject>();       // 검출된 오브젝트 Right List

    private GameObject collObject;                                                // 오브젝트 1개일 때
    private GameObject collObjectContact;                                         // 붙어있는 오브젝트
    public GameObject GetCollObject() {return collObject; }                       // 오브젝트 1개일 때 전달

    public bool GetIsObjectOnlyOne() {
        if (hitsLeft == null && hitsRight == null) return false;
        return CompareIsObjectOnlyOne(); }          // 충돌 물체가 하나일 경우 bool값 전달

    private void Awake() {
        transformPosition = transform.position;

        objectWidth = GetComponent<Collider2D>().bounds.extents.x * 0.5f;

        // 왼쪽과 오른쪽에서 오프셋된 위치 계산
        leftOrigin = transformPosition - new Vector2(objectWidth, 0);
        rightOrigin = transformPosition + new Vector2(objectWidth, 0);

    }


    // enter에서 충돌 물체 방향 초기 저장
    private void OnCollisionEnter2D(Collision2D collision) {
        saveDirectionVector.x = collision.transform.position.x - transform.position.x;
    }

    // 충돌한 물체의 진입방향을 확인하여 addforce
    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {

            hitsLeft = GetRaycastArray(leftOrigin);
            hitsRight = GetRaycastArray(rightOrigin);
            collObjectsListLeft = CheckObjectsConnection(hitsLeft);
            collObjectsListRight = CheckObjectsConnection(hitsRight);

            //TODO:[김수주] 충돌체에 붙어있는지 떨어져있는지 확인해야함
            collObjectContact = collision.collider.gameObject;
            if (CompareIsObjectOnlyOne()) {
                collObject = collObjectsListRight[0];
            }
            else {
                collObject = null;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        collObjectContact = null;
        collObject = null;
        hitsLeft = null;
        hitsRight = null;   
    }

    // Raycast로 오브젝트 전체 배열에 담음
    private RaycastHit2D[] GetRaycastArray(Vector2 vector) {
        Vector3 rayDirection = Vector3.up; // 스프링 위로 raycast 쏴야함

        Debug.DrawRay(vector, rayDirection * raycastCheckDistance, Color.red, 2.0f); //TODO: [김수주] 디버깅용 선

        RaycastHit2D[] hit = Physics2D.RaycastAll(vector, rayDirection, raycastCheckDistance, layerMask);
        //Debug.Log("length check | " + hit + " | " + hit.Length);

        if (hit.Length > 0) {
            if (hit[0].collider.gameObject == transform.gameObject) {
                hit = hit.Where(hit => hit.collider.gameObject != transform.gameObject).ToArray();  // 자기 자신일 경우 제외 (TextBOX)
            }
        }

        return hit;
        //Debug.LogWarning("hits.Length 1 ???? " + hits.Length);
    }

    // 배열 2개의 길이가 전부 1일 경우 -> 담긴 오브젝트를 비교함
    private bool CompareIsObjectOnlyOne() {
        if (CheckRaycastNum()) {
            if (CompareRaycastObject() /*배열의 담긴 오브젝트가 같은 오브젝트인지 확인*/) {
                return true;
            }
            else return false;
        }
        else return false;
    }

    // bool 배열의 개수 확인 (배열 2개의 길이가 전부 1개일 경우 true) => 
    private bool CheckRaycastNum() {
        return (collObjectsListLeft.Count == 1 && collObjectsListRight.Count == 1) ? true : false;
    }

    //배열의 담긴 오브젝트가 같은 오브젝트인지 확인
    private bool CompareRaycastObject() {
        return (collObjectsListLeft[0] == collObjectsListRight[0] == collObjectContact) ? true : false;
    }


    //TODO: [김수주] 충돌체에 붙어있는지 떨어져있는지 확인해야함
    private List<GameObject> CheckObjectsConnection(RaycastHit2D[] hits) {
        List<GameObject> findObjectList = new List<GameObject>();
        for (int i = 0; i < hits.Length; i++) {
            if(i ==0) {
                findObjectList.Add(hits[i].collider.gameObject);
                continue;
            }

            CheckCollision checkCollision = hits[i].collider.GetComponent<CheckCollision>();
            if (checkCollision.GetObjectHasDirection(HasCollDirection.down)) {  // 스프링은 위 방향일 경우만 필요함
                findObjectList.Add(hits[i].collider.gameObject);
                if (!checkCollision.GetObjectHasDirection(HasCollDirection.up)) {
                    Debug.Log("오브젝트위에 다른 객체 없음???" + hits[i].collider.name);
                    break;
                }
            }
            else {
                break;
            }
        }
        return findObjectList;
    }


}

/*  
 *   1. 목적1 : objectPrefab들 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지
 2. 내용
    2-1. 처리 내용
        1) int 값 return : 해당 오브젝트와 충돌된 오브젝트가 몇 개인지
            => 예상 사용처 : 스프링
    2-2. 처리 방법
        1) OnCollisionStay2D -> 충돌 중 일 경우 Raycast 2개로 배열 저장
        2) Raycast 개수 확인
        3) Raycast의 배열 개수가 전부 1이어야함
            1-1) 개수가 둘다 1개 임?
            1-2) 개수가 둘다 1개일 경우 => 0번째가 둘다 같음?
            1-3) 개수가 둘다 1개가 아니거나 둘다 2개이상 : 대기
 */