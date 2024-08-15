using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindCollisionObjects : MonoBehaviour {
    // 1. 목적 : objectPrefab들 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지 return
    // 검출하는 레이어 : 8번 레이어 붙어있는지 검출함

    private int layerMask = (1 << 8);
    private int collObjectsNumLeft;                                             // 왼쪽 검출된 오브젝트 개수
    private int collObjectsNumRight;                                            // 오른족 검출된 오브젝트 개수
    private float raycastCheckDistance = 500f;                                  // raycast를 확인할 거리
    private float objectWidth;                                                  // 오브젝트의 가로 반쪽 길이(raycast를 2개로 나눔)        

    private List<GameObject> collObjectsList = new List<GameObject>();          // 검출된 오브젝트 List

    public List<GameObject> GetCollObjectsList() { return collObjectsList; }    // 검출된 오브젝트 List 전달


    Vector2 transformPosition;                                                  // 스프링 위치 저장
    Vector2 leftOrigin;                                                         // 스프링 왼쪽 위치
    Vector2 rightOrigin;                                                        // 스프링 오른쪽 위치

    RaycastHit2D[] hitsLeft;
    RaycastHit2D[] hitsRight;

    private void Awake() {
        transformPosition = transform.position;

        objectWidth = GetComponent<Collider2D>().bounds.extents.x;
        // 왼쪽과 오른쪽에서 오프셋된 위치 계산
        leftOrigin = transformPosition - new Vector2(objectWidth, 0);
        rightOrigin = transformPosition + new Vector2(objectWidth, 0);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            InitCollisionObjectsNum();
            GetRaycastArray(hitsLeft, leftOrigin);
            GetRaycastArray(hitsRight, rightOrigin);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        InitCollisionObjectsNum();
    }

    // 오브젝트 갯수 초기화
    public void InitCollisionObjectsNum() {
        collObjectsNumLeft = 0;
        collObjectsNumRight = 0;
        collObjectsList.Clear();
    }

    // Raycast로 오브젝트 전체 배열에 담음
    private void GetRaycastArray(RaycastHit2D[] hit, Vector2 vector) {
        Vector3 rayDirection = Vector3.up; // 스프링 위로 raycast 쏴야함

        hit = Physics2D.RaycastAll(vector, rayDirection, raycastCheckDistance, layerMask);

        if (hit[0].collider.gameObject == transform.gameObject) {
            hit = hit.Where(hit => hit.collider.gameObject != transform.gameObject).ToArray();  // 자기 자신일 경우 제외 (TextBOX)
        }
        //Debug.LogWarning("hits.Length 1 ???? " + hits.Length);
    }

    // 배열 2개의 길이가 전부 1일 경우 -> 담긴 오브젝트를 비교함
    private bool CompareRaycastArray() {
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
        return (hitsLeft.Length == 1 && hitsRight.Length == 1 )? true : false; 
    }

    //배열의 담긴 오브젝트가 같은 오브젝트인지 확인
    private bool CompareRaycastObject() {
        return (hitsLeft[0] == hitsRight[0]) ? true : false;
    }

}

/*  
 1. 목적 : objectPrefab들 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지
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