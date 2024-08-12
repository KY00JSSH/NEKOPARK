using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCollisionObjectsNum : MonoBehaviour {
    // 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������ return
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������

    private int layerMask = (1 << 8);
    private int collObjectsNum;
    [SerializeField] private float raycastCheckDistance = 500f;
    public int GetCollObjectsNum() { return collObjectsNum; }

    private Vector2 saveDirectionVector; // �浹 ��ü ���� ����

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            collObjectsNum = 1;

            saveDirectionVector = (collision.transform.position - transform.position); // collision�� �浹�� ���� ����
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        GetRaycastArray(saveDirectionVector, collision); ;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        InitCollisionObjectsNum();
    }

    // ������Ʈ ���� �ʱ�ȭ
    public void InitCollisionObjectsNum() {
        collObjectsNum = 0;
    }

    // Raycast�� ������Ʈ ��ü �迭�� ����
    private void GetRaycastArray(Vector2 collDirection, Collision2D collision) {
        Vector3 rayOrigin = transform.position;

        Vector3 rayDirection = (collision.transform.position - rayOrigin).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, rayDirection, raycastCheckDistance, layerMask);

        Debug.LogWarning("RaycastHit2D ���ⰹ�� Ȯ�� : " + hits.Length);

        if (hits.Length >= 2) {
            CoundtColliderObjectsNum(hits, collDirection);
        }
        else {
            collObjectsNum = 1;
        }
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
            if (i == 0) { collObjectsNum = 1; continue; }
            CheckCollision checkCollision = hits[i].collider.GetComponent<CheckCollision>();

            if (Mathf.Abs(collDirection.y) > Mathf.Abs(collDirection.x)) {
                if (collDirection.y > 0) { // ���� ž�ױ�
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Bottom)) {
                        if (checkCollision.GetObjectHasDirection(HasCollDirection.Top)) {
                            collObjectsNum++;
                        }
                        else {
                            collObjectsNum++;
                            break;
                        }
                    }
                    else {
                        break;
                    }
                }
                else { // �Ʒ��� ž�ױ� �̷� ���� �ֳ�?
                    //Debug.Log("bottom side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Top)) {
                        if (checkCollision.GetObjectHasDirection(HasCollDirection.Bottom)) {
                            collObjectsNum++;
                        }
                        else {
                            collObjectsNum++;
                            break;
                        }
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
                        if (checkCollision.GetObjectHasDirection(HasCollDirection.Right)) {
                            collObjectsNum++;
                        }
                        else {
                            collObjectsNum++;
                            break;
                        }
                    }
                    else {
                        break;
                    }
                }
                else { // ������
                    //Debug.Log("Right side collided");
                    if (checkCollision.GetObjectHasDirection(HasCollDirection.Right)) {
                        if (checkCollision.GetObjectHasDirection(HasCollDirection.Left)) {
                            collObjectsNum++;
                        }
                        else {
                            collObjectsNum++;
                            break;
                        }
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