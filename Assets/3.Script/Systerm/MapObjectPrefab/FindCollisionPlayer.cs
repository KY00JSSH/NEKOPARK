using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FindCollisionPlayerFourDirection {
    public Vector2 raycastDirection;
    public List<GameObject> collPlayersList = new List<GameObject>();
}

public class FindCollisionPlayer : MonoBehaviour {
    [SerializeField] HasCollDirection SelectPlayerDirection_1;
    [SerializeField] HasCollDirection SelectPlayerDirection_2;

    private int layerMask = (1 << 8);
    private float raycastOffsetDistance;
    private float raycastCheckDistance = 500f; // raycast를 확인할 거리

    private Collider2D transformCollider;

    private List<FindCollisionPlayerFourDirection> findPlayerFourDirection = new List<FindCollisionPlayerFourDirection>();

    private List<GameObject> foundPlayerList = new List<GameObject>();
    public List<GameObject> GetFoundPlayerCount() { return foundPlayerList; }

    private void Awake() {
        transformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            InitCollisionPlayerList();

            CheckDoubleRaycast(Vector2.up);
            CheckDoubleRaycast(Vector2.down);
            CheckDoubleRaycast(Vector2.left);
            CheckDoubleRaycast(Vector2.right);

            CountPlayerInSelectDirection(SelectPlayerDirection_1);
            CountPlayerInSelectDirection(SelectPlayerDirection_2);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            InitCollisionPlayerList();
        }
    }


    // 플레이어 리스트 초기화
    private void InitCollisionPlayerList() {
        foundPlayerList.Clear();
        findPlayerFourDirection.Clear();
    }

    private void CheckDoubleRaycast(Vector2 direction) {
        Vector3 rayOrigin = transform.position;

        // raycast 기존 중점
        Vector2 perpendicular = Vector2.Perpendicular(direction).normalized;
        raycastOffsetDistance = Vector3.Dot(transformCollider.bounds.extents, perpendicular);

        // raycast 위아래 지정
        Vector3 origin1 = rayOrigin + (Vector3)(perpendicular * raycastOffsetDistance);
        Vector3 origin2 = rayOrigin - (Vector3)(perpendicular * raycastOffsetDistance);

        // Perform the raycasts
        RaycastHit2D[] hits1 = Physics2D.RaycastAll(origin1, direction, raycastCheckDistance, layerMask);
        RaycastHit2D[] hits2 = Physics2D.RaycastAll(origin2, direction, raycastCheckDistance, layerMask);


        // RaycastHit2D 범위 안쪽으로 CheckCollision 연결되어있는지 확인
        ProcessRaycastHits(direction, hits1);
        ProcessRaycastHits(direction, hits2);
    }

    // 자기자신 제외하고 재배열 
    private void ProcessRaycastHits(Vector2 direction, RaycastHit2D[] hits) {
        if (hits.Length > 0 && hits[0].collider.gameObject == gameObject) {
            hits = hits.Where(hit => hit.collider.gameObject != gameObject).ToArray();
        }

        CoundtColliderObjectsNum(direction, hits);
    }
    // 재배열된 raycast로 CheckCollision 연결되어있는지 확인 후 list 담기
    private void CoundtColliderObjectsNum(Vector2 direction, RaycastHit2D[] hits) {
        FindCollisionPlayerFourDirection findCollisionPlayer = new FindCollisionPlayerFourDirection();
        findCollisionPlayer.raycastDirection = direction;
        for (int i = 0; i < hits.Length; i++) {
            if (i == 0) {
                if (hits[i].collider.CompareTag("Player")) {
                    findCollisionPlayer.collPlayersList.Add(hits[i].collider.gameObject);
                }
                continue;
            }

            CheckCollision checkCollision = hits[i].collider.GetComponent<CheckCollision>();

            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
                if (direction.y > 0) { // 위로 탑쌓기
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.down)) {
                        if (hits[i].collider.CompareTag("Player")) {
                            findCollisionPlayer.collPlayersList.Add(hits[i].collider.gameObject);
                        }
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.up)) { break; }
                    }
                    else {
                        break;
                    }
                }
                else { // 아래로 탑쌓기 이럴 일이 있나?
                    //Debug.Log("bottom side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.up)) {
                        if (hits[i].collider.CompareTag("Player")) {
                            findCollisionPlayer.collPlayersList.Add(hits[i].collider.gameObject);
                        }
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.down)) { break; }
                    }
                    else {
                        break;
                    }
                }
            }
            else {
                if (direction.x < 0) { // 왼쪽
                    //Debug.Log("Left side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.left)) {
                        if (hits[i].collider.CompareTag("Player")) {
                            findCollisionPlayer.collPlayersList.Add(hits[i].collider.gameObject);
                        }
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.right)) { break; }
                    }
                    else {
                        break;
                    }
                }
                else { // 오른쪽
                    //Debug.Log("Right side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.right)) {
                        if (hits[i].collider.CompareTag("Player")) {
                            findCollisionPlayer.collPlayersList.Add(hits[i].collider.gameObject);
                        }
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.left)) { break; }
                    }
                    else {
                        break;
                    }
                }
            }

        }

        findPlayerFourDirection.Add(findCollisionPlayer);
    }

    // 설정된 selectPlayerDirection으로 플레이어 방향 찾기, default = 전방향
    private void CountPlayerInSelectDirection(HasCollDirection selectDirection) {
        Vector2 selectD = new Vector2();

        if (selectDirection == HasCollDirection.up) selectD = Vector2.up;
        else if (selectDirection == HasCollDirection.down) selectD = Vector2.down;
        else if (selectDirection == HasCollDirection.left) selectD = Vector2.left;
        else if (selectDirection == HasCollDirection.right) selectD = Vector2.right;
        else {
            selectD = Vector2.zero;
        }


        if (selectD == Vector2.zero) {
            foreach (FindCollisionPlayerFourDirection each in findPlayerFourDirection) {
                if (foundPlayerList.Count <= each.collPlayersList.Count) {
                    foundPlayerList = each.collPlayersList;
                }
            }
        }
        else {
            foreach (FindCollisionPlayerFourDirection each in findPlayerFourDirection) {
                if (each.raycastDirection == selectD) {
                    if (foundPlayerList.Count <= each.collPlayersList.Count) {
                        foundPlayerList = each.collPlayersList;
                    }
                }
            }
        }
    }

}

/*  
 1. 목적 : Textbox에서 4방향의 레이어를 쏴서 player check
 2. 내용
    2-1. 처리 내용
            => 사용처 : TextBox
    2-2. 처리 방법
        1) OnCollisionStay2D -> 충돌 중 일 경우 
            Raycast 4방향 확인?
            Raycast 배열 저장
            player면 
        3) CoundtColliderObjectsNum(hits, collDirection) 
            -> Raycast 배열에 저장되어있는 오브젝트들의 getcomponent로 CheckCollision의 현재 충돌이 되어있는 면을 확인
            -> 
 */