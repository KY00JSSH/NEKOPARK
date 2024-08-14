using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPrefabController_ : MonoBehaviour {
    // 1. ���� : ������ ������ ������Ʈ ��� ���� -> �ٸ� ������Ʈ�� ���������� 

    private int layerMask = (1 << 8);                           // ���� ���̾�
    public float addForce = 5f;                                 // Addforce �� (���� ���� ���������ؾ���)
    private Vector2 addForceVector; // �浹 ��ü ���� Ȯ��
    private Vector2 saveDirectionVector;                         // �浹 ��ü ���� �ʱ� ����

    private Animator spriteAnimator; // spring image animation

    private FindCollisionObjectsNum findCollisionObjectsNum;

    private void Awake() {
        spriteAnimator = GetComponent<Animator>();

        findCollisionObjectsNum = GetComponent<FindCollisionObjectsNum>();
    }

    // enter���� �浹 ��ü ���� �ʱ� ����
    private void OnCollisionEnter2D(Collision2D collision) {
        saveDirectionVector.x = collision.transform.position.x - transform.position.x;
    }

    // �浹�� ��ü�� ���Թ����� Ȯ���Ͽ� addforce
    private void OnCollisionStay2D(Collision2D collision) {
        Debug.Log("SpringPrefabController : " + findCollisionObjectsNum.GetCollObjectsNum());

        if (findCollisionObjectsNum.GetCollObjectsNum()==1) {
            Rigidbody2D collRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collRigidbody2D != null) {

                //addForceVector = new Vector2(saveDirectionVector.x, addForce);
                collRigidbody2D.AddForce(collRigidbody2D.transform.up * addForce, ForceMode2D.Impulse);
            }
        }
    }

}

/*  
 1. ���� : ������ ������ ������Ʈ ��� ���� -> �ٸ� ������Ʈ�� ���������� 
 2. ����
    2-1. ���� 
        1) Addforce�� �Ϸ��� �� ���� : public float�� �ܺο��� ���� => ������ �������� ������ �޶����� ��찡 ����
        2) addForceVector : �浹 ��ü ���� Ȯ�� �뵵
        3) spriteAnimator : spring �̹��� ���� �ִϸ��̼�
    2-2. ó�� ���� 
        1) Collider Ȯ�� (������ �������� ��θ� Ȯ��)
        2) if( Collider's num >= ....) return? => 1���� ������ �� ���� //TODO: FindCollistionObjects ���� Ȯ�� �����Ѱ�?
        3) Ȯ�ε� object�� ���Թ����� Ȯ���Ͽ� �ش� �������� ����
        4) ��������Ʈ ���� //TODO: spring sprite ���� �Ǹ� animator ��������
 3. �ش� ��ũ��Ʈ ��ġ : ������ ������
 */
