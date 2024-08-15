using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpringPrefabController_ : MonoBehaviour {
    // 1. ���� : ������ ������ ������Ʈ ��� ���� => �ش� ������Ʈ�� �浹�� ������Ʈ�� �� ������  / �ٸ� ������Ʈ�� ���������� 
    // �����ϴ� ���̾� : 8�� ���̾� �پ��ִ��� ������


    public float addForce = 2f;                                                 // Addforce �� (���� ���� ���������ؾ���)

    private Vector2 transformPosition;                                           // ������ ��ġ ����

    private Vector2 addForceVector;                                              // �浹 ��ü ���� Ȯ��
    private Vector2 saveDirectionVector;                                         // �浹 ��ü ���� �ʱ� ����

    [SerializeField]private GameObject collisionObject;                                          // ���� ������Ʈ

    private FindCollisionObjects findCollisionObjects;                           // �浹 ��ü ã�� ��ũ��Ʈ

    private Animator spriteAnimator;                                             // spring image animation

    private void SetCollObject() { collisionObject = findCollisionObjects.GetCollObject(); }  // ���� ������Ʈ 1���� �� �޾ƿ���



    private void Awake() {
        transformPosition = transform.position;

        findCollisionObjects = GetComponent<FindCollisionObjects>();

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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
            saveDirectionVector = collision.transform.position - transform.position;
        }
    }


    // �迭�� ������Ʈ�� ���ٸ� addforce
    //TODO: [�����] ������ ������ �����ؾ���
    private void AddforceObject() {
        if (collisionObject != null) {

            Rigidbody2D collRigidbody2D = collisionObject.GetComponent<Rigidbody2D>();
            if (collRigidbody2D != null) {
                //addForceVector = new Vector2(saveDirectionVector.x, addForce);
                collRigidbody2D.AddForce(saveDirectionVector * addForce, ForceMode2D.Impulse);
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
