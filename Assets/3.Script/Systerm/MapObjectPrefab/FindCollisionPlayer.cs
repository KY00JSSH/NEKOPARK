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
    private float raycastCheckDistance = 500f; // raycast�� Ȯ���� �Ÿ�

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


    // �÷��̾� ����Ʈ �ʱ�ȭ
    private void InitCollisionPlayerList() {
        foundPlayerList.Clear();
        findPlayerFourDirection.Clear();
    }

    private void CheckDoubleRaycast(Vector2 direction) {
        Vector3 rayOrigin = transform.position;

        // raycast ���� ����
        Vector2 perpendicular = Vector2.Perpendicular(direction).normalized;
        raycastOffsetDistance = Vector3.Dot(transformCollider.bounds.extents, perpendicular);

        // raycast ���Ʒ� ����
        Vector3 origin1 = rayOrigin + (Vector3)(perpendicular * raycastOffsetDistance);
        Vector3 origin2 = rayOrigin - (Vector3)(perpendicular * raycastOffsetDistance);

        // Perform the raycasts
        RaycastHit2D[] hits1 = Physics2D.RaycastAll(origin1, direction, raycastCheckDistance, layerMask);
        RaycastHit2D[] hits2 = Physics2D.RaycastAll(origin2, direction, raycastCheckDistance, layerMask);


        // RaycastHit2D ���� �������� CheckCollision ����Ǿ��ִ��� Ȯ��
        ProcessRaycastHits(direction, hits1);
        ProcessRaycastHits(direction, hits2);
    }

    // �ڱ��ڽ� �����ϰ� ��迭 
    private void ProcessRaycastHits(Vector2 direction, RaycastHit2D[] hits) {
        if (hits.Length > 0 && hits[0].collider.gameObject == gameObject) {
            hits = hits.Where(hit => hit.collider.gameObject != gameObject).ToArray();
        }

        CoundtColliderObjectsNum(direction, hits);
    }
    // ��迭�� raycast�� CheckCollision ����Ǿ��ִ��� Ȯ�� �� list ���
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
                if (direction.y > 0) { // ���� ž�ױ�
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
                else { // �Ʒ��� ž�ױ� �̷� ���� �ֳ�?
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
                if (direction.x < 0) { // ����
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
                else { // ������
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

    // ������ selectPlayerDirection���� �÷��̾� ���� ã��, default = ������
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
 1. ���� : Textbox���� 4������ ���̾ ���� player check
 2. ����
    2-1. ó�� ����
            => ���ó : TextBox
    2-2. ó�� ���
        1) OnCollisionStay2D -> �浹 �� �� ��� 
            Raycast 4���� Ȯ��?
            Raycast �迭 ����
            player�� 
        3) CoundtColliderObjectsNum(hits, collDirection) 
            -> Raycast �迭�� ����Ǿ��ִ� ������Ʈ���� getcomponent�� CheckCollision�� ���� �浹�� �Ǿ��ִ� ���� Ȯ��
            -> 
 */