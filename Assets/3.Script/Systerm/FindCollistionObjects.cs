using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCollistionObjects : MonoBehaviour {
    // 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������ return

    private HashSet<GameObject> collidingObjects = new HashSet<GameObject>(); //HashSet : �ߺ� ����
    public int GetCollObjectsNum() { return collidingObjects.Count; }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Add the colliding object to the HashSet
        collidingObjects.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // Remove the object when the collision ends
        collidingObjects.Remove(collision.gameObject);
    }
}

/*  
 1. ���� : objectPrefab�� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
 2. ����
    2-1. ó�� ����
        1) int �� return : �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������
            => ���� ���ó : �ڽ�, �����̴� ��
 */