using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpringPrefabController : MonoBehaviour {
    // 1. ���� : ������ ������ ������Ʈ ��� ���� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������  / �ٸ� ������Ʈ�� ���������� 
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������

    private float moveX;
    public float addForceY;                                                 // Addforce �� (���� ���� ���������ؾ���)
    public float addForceX;                                                 // Addforce �� (���� ���� ���������ؾ���)

    public bool isBoxNeedAddforce { get; private set; }     // 스프링에서 box 밀어야할때 사용

    private RectTransform rectTransform;
    private Vector2 transformPosition;                                           // ������ ��ġ ����
    private Vector2 addForceVector;                                              // �浹 ��ü ���� Ȯ��
    private Vector2 saveDirectionVector;                                         // �浹 ��ü ���� �ʱ� ����

    [SerializeField] private GameObject collisionObject;                                          // ���� ������Ʈ

    private FindCollisionObjectsSpring findCollisionObjects;                           // �浹 ��ü ã�� ��ũ��Ʈ

    private Animator spriteAnimator;                                             // spring image animation

    private void SetCollObject() { collisionObject = findCollisionObjects.GetCollObject(); }  // ���� ������Ʈ 1���� �� �޾ƿ���



    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
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
        if (collision.transform.TryGetComponent(out RectTransform collisionRect)) {
            float collisionBottom = collision.transform.position.y - collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            // 충돌체가 내 위에 있는 경우.
            if (collision.collider.CompareTag("Box")) {
                if (collisionBottom >= transform.position.y + rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f) {
                    //Vector2 collisionPosition = collisionRect.position;

                    Vector2 relativeVelocity = collision.relativeVelocity;
                    moveX = relativeVelocity.x >= 0f ? 1f : -1f;
                    Debug.Log("Collision direction (X): " + moveX);
                }
            }
            else {
                moveX = 0;
            }
            //Debug.Log("collisionBottom : " + collisionBottom + " | " + transform.position.y + rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f);

        }

    }

    private void OnCollisionExit2D(Collision2D collision) {

        //isBoxNeedAddforce = false;
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
                    isBoxNeedAddforce = true;
                    RectTransform rectTransform = collisionObject.GetComponent<RectTransform>();
                    position = rectTransform.transform.position;
                }

                force = new Vector2(moveX * addForceX, addForceY);
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
