using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPrefabController : MonoBehaviour {
     // 1. ���� : ������ ������ ������Ʈ ��� ���� -> �ٸ� ������Ʈ�� ���������� 

    public float addForce = 10f; // Addforce �� (���� ���� ���������ؾ���)
    private Vector2 addForceVector; // �浹 ��ü ���� Ȯ��

    private Animator spriteAnimator; // spring image animation

    private void Awake() {
        Debug.Log("SpringPrefabController Awake");
        spriteAnimator = GetComponent<Animator>();
    }


    // �浹�� ��ü�� ���Թ����� Ȯ���Ͽ� addforce
    //TODO: 1�� �̻� ���ƾ���
    private void OnCollisionEnter2D(Collision2D collision) {

        Rigidbody2D collRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collRigidbody2D != null) {

            Vector2 directionToCollider = collision.transform.position - transform.position;  // �浹 ���� Ȯ��
            Debug.Log(directionToCollider);
            addForceVector = new Vector2(directionToCollider.x, addForce);
            Debug.Log(addForceVector);
            addForceVector.x = directionToCollider.x;
            collRigidbody2D.AddForce(collRigidbody2D.transform.up * addForce, ForceMode2D.Impulse);

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
