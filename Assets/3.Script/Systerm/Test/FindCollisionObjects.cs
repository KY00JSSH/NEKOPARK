using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindCollisionObjects : MonoBehaviour {
    // 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������ return
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������

    private int layerMask = (1 << 8);
    private int collObjectsNumLeft;                                             // ���� ����� ������Ʈ ����
    private int collObjectsNumRight;                                            // ������ ����� ������Ʈ ����
    private float raycastCheckDistance = 500f;                                  // raycast�� Ȯ���� �Ÿ�
    private float objectWidth;                                                  // ������Ʈ�� ���� ���� ����(raycast�� 2���� ����)        

    private List<GameObject> collObjectsList = new List<GameObject>();          // ����� ������Ʈ List

    public List<GameObject> GetCollObjectsList() { return collObjectsList; }    // ����� ������Ʈ List ����


    Vector2 transformPosition;                                                  // ������ ��ġ ����
    Vector2 leftOrigin;                                                         // ������ ���� ��ġ
    Vector2 rightOrigin;                                                        // ������ ������ ��ġ

    RaycastHit2D[] hitsLeft;
    RaycastHit2D[] hitsRight;

    private void Awake() {
        transformPosition = transform.position;

        objectWidth = GetComponent<Collider2D>().bounds.extents.x;
        // ���ʰ� �����ʿ��� �����µ� ��ġ ���
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

    // ������Ʈ ���� �ʱ�ȭ
    public void InitCollisionObjectsNum() {
        collObjectsNumLeft = 0;
        collObjectsNumRight = 0;
        collObjectsList.Clear();
    }

    // Raycast�� ������Ʈ ��ü �迭�� ����
    private void GetRaycastArray(RaycastHit2D[] hit, Vector2 vector) {
        Vector3 rayDirection = Vector3.up; // ������ ���� raycast ������

        hit = Physics2D.RaycastAll(vector, rayDirection, raycastCheckDistance, layerMask);

        if (hit[0].collider.gameObject == transform.gameObject) {
            hit = hit.Where(hit => hit.collider.gameObject != transform.gameObject).ToArray();  // �ڱ� �ڽ��� ��� ���� (TextBOX)
        }
        //Debug.LogWarning("hits.Length 1 ???? " + hits.Length);
    }

    // �迭 2���� ���̰� ���� 1�� ��� -> ��� ������Ʈ�� ����
    private bool CompareRaycastArray() {
        if (CheckRaycastNum()) {
            if (CompareRaycastObject() /*�迭�� ��� ������Ʈ�� ���� ������Ʈ���� Ȯ��*/) {
                return true;
            }
            else return false;
        }
        else return false;

    }

    // bool �迭�� ���� Ȯ�� (�迭 2���� ���̰� ���� 1���� ��� true) => 
    private bool CheckRaycastNum() { 
        return (hitsLeft.Length == 1 && hitsRight.Length == 1 )? true : false; 
    }

    //�迭�� ��� ������Ʈ�� ���� ������Ʈ���� Ȯ��
    private bool CompareRaycastObject() {
        return (hitsLeft[0] == hitsRight[0]) ? true : false;
    }

}

/*  
 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
 2. ����
    2-1. ó�� ����
        1) int �� return : �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
            => ���� ���ó : ������
    2-2. ó�� ���
        1) OnCollisionStay2D -> �浹 �� �� ��� Raycast 2���� �迭 ����
        2) Raycast ���� Ȯ��
        3) Raycast�� �迭 ������ ���� 1�̾����
            1-1) ������ �Ѵ� 1�� ��?
            1-2) ������ �Ѵ� 1���� ��� => 0��°�� �Ѵ� ����?
            1-3) ������ �Ѵ� 1���� �ƴϰų� �Ѵ� 2���̻� : ���
 */