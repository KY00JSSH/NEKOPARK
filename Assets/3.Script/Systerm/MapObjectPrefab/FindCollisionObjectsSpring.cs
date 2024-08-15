using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindCollisionObjectsSpring : MonoBehaviour {
    // 1. ���� : ������ ������ ������Ʈ ��� ���� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������  / �ٸ� ������Ʈ�� ���������� 
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������

    private int layerMask = (1 << 8);

    private float raycastCheckDistance = 500f;                                  // raycast�� Ȯ���� �Ÿ�
    [SerializeField] private float objectWidth;                                 // ������Ʈ�� ���� ���� ����(raycast�� 2���� ����)       

    private Vector2 transformPosition;                                           // ������ ��ġ ����
    private Vector2 leftOrigin;                                                  // ������ ���� ��ġ
    private Vector2 rightOrigin;                                                 // ������ ������ ��ġ

    private Vector2 addForceVector;                                              // �浹 ��ü ���� Ȯ��
    private Vector2 saveDirectionVector;                                         // �浹 ��ü ���� �ʱ� ����

    private RaycastHit2D[] hitsRight;
    private RaycastHit2D[] hitsLeft;

    [SerializeField] private List<GameObject> collObjectsListLeft = new List<GameObject>();       // ����� ������Ʈ Left List
    [SerializeField] private List<GameObject> collObjectsListRight = new List<GameObject>();       // ����� ������Ʈ Right List

    private GameObject collObject;                                                // ������Ʈ 1���� ��
    private GameObject collObjectContact;                                         // �پ��ִ� ������Ʈ
    public GameObject GetCollObject() {return collObject; }                       // ������Ʈ 1���� �� ����

    public bool GetIsObjectOnlyOne() {
        if (hitsLeft == null && hitsRight == null) return false;
        return CompareIsObjectOnlyOne(); }          // �浹 ��ü�� �ϳ��� ��� bool�� ����

    private void Awake() {
        transformPosition = transform.position;

        objectWidth = GetComponent<Collider2D>().bounds.extents.x * 0.5f;

        // ���ʰ� �����ʿ��� �����µ� ��ġ ���
        leftOrigin = transformPosition - new Vector2(objectWidth, 0);
        rightOrigin = transformPosition + new Vector2(objectWidth, 0);

    }


    // enter���� �浹 ��ü ���� �ʱ� ����
    private void OnCollisionEnter2D(Collision2D collision) {
        saveDirectionVector.x = collision.transform.position.x - transform.position.x;
    }

    // �浹�� ��ü�� ���Թ����� Ȯ���Ͽ� addforce
    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {

            hitsLeft = GetRaycastArray(leftOrigin);
            hitsRight = GetRaycastArray(rightOrigin);
            collObjectsListLeft = CheckObjectsConnection(hitsLeft);
            collObjectsListRight = CheckObjectsConnection(hitsRight);

            //TODO:[�����] �浹ü�� �پ��ִ��� �������ִ��� Ȯ���ؾ���
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

    // Raycast�� ������Ʈ ��ü �迭�� ����
    private RaycastHit2D[] GetRaycastArray(Vector2 vector) {
        Vector3 rayDirection = Vector3.up; // ������ ���� raycast ������

        Debug.DrawRay(vector, rayDirection * raycastCheckDistance, Color.red, 2.0f); //TODO: [�����] ������ ��

        RaycastHit2D[] hit = Physics2D.RaycastAll(vector, rayDirection, raycastCheckDistance, layerMask);
        //Debug.Log("length check | " + hit + " | " + hit.Length);

        if (hit.Length > 0) {
            if (hit[0].collider.gameObject == transform.gameObject) {
                hit = hit.Where(hit => hit.collider.gameObject != transform.gameObject).ToArray();  // �ڱ� �ڽ��� ��� ���� (TextBOX)
            }
        }

        return hit;
        //Debug.LogWarning("hits.Length 1 ???? " + hits.Length);
    }

    // �迭 2���� ���̰� ���� 1�� ��� -> ��� ������Ʈ�� ����
    private bool CompareIsObjectOnlyOne() {
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
        return (collObjectsListLeft.Count == 1 && collObjectsListRight.Count == 1) ? true : false;
    }

    //�迭�� ��� ������Ʈ�� ���� ������Ʈ���� Ȯ��
    private bool CompareRaycastObject() {
        return ((collObjectsListLeft[0] == collObjectsListRight[0]) && (collObjectsListRight[0] == collObjectContact)) ? true : false;
    }


    //TODO: [�����] �浹ü�� �پ��ִ��� �������ִ��� Ȯ���ؾ���
    private List<GameObject> CheckObjectsConnection(RaycastHit2D[] hits) {
        List<GameObject> findObjectList = new List<GameObject>();
        for (int i = 0; i < hits.Length; i++) {
            if(i ==0) {
                findObjectList.Add(hits[i].collider.gameObject);
                continue;
            }

            CheckCollision checkCollision = hits[i].collider.GetComponent<CheckCollision>();
            if (checkCollision.GetObjectHasDirection(HasCollDirection.down)) {  // �������� �� ������ ��츸 �ʿ���
                //Debug.Log("?????????"+ hits[i].collider.name);
                findObjectList.Add(hits[i].collider.gameObject);
                if (!checkCollision.GetObjectHasDirection(HasCollDirection.up)) {
                    //Debug.Log("???false" + hits[i].collider.name);
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
 *   1. ����1 : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
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