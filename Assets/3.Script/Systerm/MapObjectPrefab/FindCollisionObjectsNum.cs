using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindCollisionObjectsNum : MonoBehaviour {
    // 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������ return
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������

    private int layerMask = (1 << 8);
    private int collObjectsNum;                                                 // ����� ������Ʈ ����
    private float raycastCheckDistance = 500f;                                  // raycast�� Ȯ���� �Ÿ�
    private float objectWidth;                                                  // ������Ʈ�� ���� ���� ����(raycast�� 2���� ����)        

    private List<GameObject> collObjectsList = new List<GameObject>();          // ����� ������Ʈ List

    public int GetCollObjectsNum() { return collObjectsNum; }                   // ����� ������Ʈ ���� ����
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
        collObjectsNum = 0;
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

    // ���� ��ü �迭 ���ؾ��� => ������ 1�� �ƴ� ��쿡
    private void CompareRaycastArray() {


    }

}

/*  
 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
 2. ����
    2-1. ó�� ����
        1) int �� return : �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
            => ���� ���ó : ������
    2-2. ó�� ���
        1) OnCollisionEnter2D -> �浹�� ������ ��� ���� ����
        2) OnCollisionStay2D -> �浹 �� �� ��� Raycast 2���� �迭 ���� =>  GetRaycastArray(Vector2 collDirection, Collision2D collision)
        3) CoundtColliderObjectsNum(hits, collDirection) 
            -> Raycast �迭�� ����Ǿ��ִ� ������Ʈ���� getcomponent�� CheckCollision�� ���� �浹�� �Ǿ��ִ� ���� Ȯ��
            -> 
 */