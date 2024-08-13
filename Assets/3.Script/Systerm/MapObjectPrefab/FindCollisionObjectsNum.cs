using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindCollisionObjectsNum : MonoBehaviour {
    // 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������ return
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������

    private int collObjectsNum; // ����� ������Ʈ ����
    private int layerMask = (1 << 8);
    private float raycastCheckDistance = 500f; // raycast�� Ȯ���� �Ÿ�
    public bool isStartCollision { get; private set; }

    private List<GameObject> collObjectsList = new List<GameObject>(); // ����� ������Ʈ List

    public int GetCollObjectsNum() { return collObjectsNum; }
    public List<GameObject> GetCollObjectsList() { return collObjectsList; }



    private Vector2 saveDirectionVector; // �浹 ��ü ���� ����

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            isStartCollision = true;
            saveDirectionVector = (collision.transform.position - transform.position); // collision�� �浹�� ���� ����
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

    // ������Ʈ ���� �ʱ�ȭ
    public void InitCollisionObjectsNum() {
        collObjectsNum = 0;
        collObjectsList.Clear();
    }

    // Raycast�� ������Ʈ ��ü �迭�� ����
    private void GetRaycastArray(Vector2 collDirection, Collision2D collision) {
        Vector3 rayOrigin = transform.position;

        Vector3 rayDirection = (collision.transform.position - rayOrigin).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, rayDirection, raycastCheckDistance, layerMask);
        //Debug.LogWarning("hits.Length 1 ???? " + hits.Length);
        if (hits[0].collider.gameObject == transform.gameObject) {
            hits = hits.Where(hit => hit.collider.gameObject != transform.gameObject).ToArray();  // �ڱ� �ڽ��� ��� ���� (TextBOX)
        }

        if (hits.Length >= 2) {
            CoundtColliderObjectsNum(hits, collDirection);
        }
        else {
            collObjectsList.Add(collision.gameObject);
            collObjectsNum = 1;
        }
        Debug.LogWarning("collObjectsNum ���ⰹ�� Ȯ�� : " + collObjectsNum);
    }

    /*     
    raycast �迭�� ������ִ� ��� ��ü ����
    �����Ǿ��ִ� ������Ʈ�� �浹�� ���� ���� => �����̷��� ������Ʈ�� ����⿡ ��ü�� �´���ִ��� Ȯ��

    �˻� ���� ����
    1. �����̷��� ������Ʈ�� �������� �����Ǿ��ִ� ������Ʈ�� �浹�� ����
    2. �����̷��� ������Ʈ�� �������� �浹 �ݴ� ����  

    - 1���� ����� ������ ������Ʈ ����
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
                if (collDirection.y > 0) { // ���� ž�ױ�
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Bottom)) {
                        collObjectsNum++;
                        collObjectsList.Add(hits[i].collider.gameObject);
                        if (!checkCollision.GetObjectHasDirection(HasCollDirection.Top)) { break; }
                    }
                    else {
                        break;
                    }
                }
                else { // �Ʒ��� ž�ױ� �̷� ���� �ֳ�?
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
                if (collDirection.x < 0) { // ����
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
                else { // ������
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
 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
 2. ����
    2-1. ó�� ����
        1) int �� return : �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
            => ���� ���ó : ������, �ڽ�, �����̴� ��
    2-2. ó�� ���
        1) OnCollisionEnter2D -> �浹�� ������ ��� ���� ����
        2) OnCollisionStay2D -> �浹 �� �� ��� Raycast �迭 ���� =>  GetRaycastArray(Vector2 collDirection, Collision2D collision)
        3) CoundtColliderObjectsNum(hits, collDirection) 
            -> Raycast �迭�� ����Ǿ��ִ� ������Ʈ���� getcomponent�� CheckCollision�� ���� �浹�� �Ǿ��ִ� ���� Ȯ��
            -> 
 */