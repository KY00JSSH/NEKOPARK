using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCollistionObjects : MonoBehaviour {
    // 1. 목적 : objectPrefab들 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지 return

    private HashSet<GameObject> collidingObjects = new HashSet<GameObject>(); //HashSet : 중복 방지
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
 1. 목적 : objectPrefab들 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지
 2. 내용
    2-1. 처리 내용
        1) int 값 return : 해당 오브젝트와 충돌된 오브젝트가 몇 개인지
            => 예상 사용처 : 박스, 움직이는 벽
 */