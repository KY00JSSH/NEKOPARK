using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_FallBoxController : MonoBehaviour {
    public int FallBoxRemainTime;

    private RectTransform rectTransform;
    private Rigidbody2D fallBoxRigidyBody;
    private Collider2D fallBoxCollider;

    private Vector2 lastPosition = Vector2.zero;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        fallBoxRigidyBody = GetComponent<Rigidbody2D>();
        fallBoxCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent(out RectTransform collisionRect)) {
            float collisionBottom = collision.transform.position.y - collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            // 충돌체가 내 위에 있는 경우.
            if (collisionBottom >= transform.position.y + rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f) {
                // 일정시간 뒤 상자 떨어짐
                StartCoroutine(FallBoxFallingAfterRemainTime());
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        StopCoroutine(FallBoxFallingAfterRemainTime());
    }


    public IEnumerator FallBoxFallingAfterRemainTime() {
        float timer = 0.0f;
        while (timer < FallBoxRemainTime) {
            timer += Time.deltaTime;
            yield return null;
        }

        fallBoxRigidyBody.bodyType = RigidbodyType2D.Dynamic;
        fallBoxRigidyBody.gravityScale = 5;

        fallBoxCollider.isTrigger = true;
    }

}
