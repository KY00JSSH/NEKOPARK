using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpringPrefabController : MonoBehaviour {
    // 1. ���� : ������ ������ ������Ʈ ��� ���� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������  / �ٸ� ������Ʈ�� ���������� 
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������

    private float moveX;
    public float addForce;                                                 // Addforce �� (���� ���� ���������ؾ���)

    private Vector2 transformPosition;                                           // ������ ��ġ ����

    private Vector2 addForceVector;                                              // �浹 ��ü ���� Ȯ��
    private Vector2 saveDirectionVector;                                         // �浹 ��ü ���� �ʱ� ����

    [SerializeField] private GameObject collisionObject;                                          // ���� ������Ʈ

    private FindCollisionObjectsSpring findCollisionObjects;                           // �浹 ��ü ã�� ��ũ��Ʈ

    private Animator spriteAnimator;                                             // spring image animation

    private void SetCollObject() { collisionObject = findCollisionObjects.GetCollObject(); }  // ���� ������Ʈ 1���� �� �޾ƿ���



    private void Awake() {
        transformPosition = transform.position;

        findCollisionObjects = GetComponent<FindCollisionObjectsSpring>();

        spriteAnimator = GetComponent<Animator>();
    }


    private void Update() {

        if (findCollisionObjects.GetIsObjectOnlyOne()) {
            SetCollObject();
            AddforceObject();
            collisionObject = null;
        }

    }

    // enter���� �浹 ��ü ���� �ʱ� ����
    private void OnCollisionEnter2D(Collision2D collision) {
        float topY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
            Vector2 collisionPosition;

            // Handle different types of colliders
            if (collision.gameObject.CompareTag("Player")) {
                collisionPosition = collision.transform.position;
            }
            else {
                RectTransform rectTransform = collision.gameObject.GetComponent<RectTransform>();
                collisionPosition = rectTransform.position;
            }

            if (collisionPosition.y >= topY) {
                Vector2 relativeVelocity = collision.relativeVelocity;
                moveX = relativeVelocity.x >= 0f ? 1f : -1f;
                //Debug.Log("Collision direction (X): " + moveX);
            }
            else {
                return;
            }
        }

        /*
         
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
            Vector2 relativeVelocity = collision.relativeVelocity; // �浹ü�� ������ ������ �³�?

            moveX = relativeVelocity.x >= 0f ? 1f : -1f;

            Debug.Log("Collision direction (X): " + moveX);
        }
         */
    }
    private void OnCollisionExit2D(Collision2D collision) {
        moveX = 0f;
    }


    // �迭�� ������Ʈ�� ���ٸ� addforce
    //TODO: [�����] ���� ������ ���� ������, �������� �� ���� ���� ����
    private void AddforceObject() {
        if (collisionObject != null) {

            Rigidbody2D collRigidbody2D = collisionObject.GetComponent<Rigidbody2D>();

            if (collRigidbody2D != null) {
                Vector2 force;
                Vector2 position;

                if (collisionObject.gameObject.CompareTag("Player")) {

                    //Debug.Log("? x���� ������ �� �ȵǴ°ǵ� �ù߰� " + force.x + " | " + force.y);
                    position = collisionObject.transform.position;
                }
                else {
                    //TODO: [김수주] 상자의 x 고정이 안풀리는 문제가 있음
                    collRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

                    RectTransform rectTransform = collisionObject.GetComponent<RectTransform>();
                    position = rectTransform.anchoredPosition;
                }

                force = new Vector2(moveX * addForce * 0.3f, addForce);
                //Debug.Log("? x���� ������ �� �ȵǴ°ǵ� �ù߰� " + force.x + " | " + force.y);
                collRigidbody2D.AddForceAtPosition(force, position, ForceMode2D.Impulse); // ��ü ������
            }
        }
    }


}

/*  
 1. ����2 : ������ ������ ������Ʈ ��� ���� -> �ٸ� ������Ʈ�� ���������� 
 2. ����
    2-1. ���� 
        1) Addforce�� �Ϸ��� �� ���� : public float�� �ܺο��� ���� => ������ �������� ������ �޶����� ��찡 ����
        2) addForceVector : �浹 ��ü ���� Ȯ�� �뵵
        3) spriteAnimator : spring �̹��� ���� �ִϸ��̼�
    2-2. ó�� ���� 
        1) FindCollisionObjects����Ȯ���� bool�� ���
        2) ��ü�� ���� ���� Ȯ��
        3) Ȯ�ε� object�� ���Թ����� Ȯ���Ͽ� �ش� �������� ����
        4) ��������Ʈ ���� 
 3. �ش� ��ũ��Ʈ ��ġ : ������ ������
 */
