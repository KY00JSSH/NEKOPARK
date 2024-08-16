using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_KeyController : MonoBehaviour
{
    /*
    ���� ������Ʈ(Key)�� ���̴� ��ũ��Ʈ.
    �÷��̾� ����ٴϱ�, �� �� �� ������� ��� �⺻���� ��� ž��.
    */

    private Transform PlayerTransform;
    //private Collider2D KeyCollider; //�ʿ���� Ȯ���� �� 240814 10:53

    private bool IsFollowingPlayer = false;
    public float followingSpeed = 2.0f;            //���谡 �÷��̾ ���󰡴� �ӵ�

    private bool IsSFXPlayed;

    private void Awake()
    {
        PlayerTransform = GetComponent<Transform>();
        //KeyCollider = GetComponent<Collider2D>();
        
    }

    private void Update()
    {
        KeyFollowing();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !IsSFXPlayed)
        {
            PlayerTransform = collision.transform;
            IsFollowingPlayer = true;

            collision.GetComponent<PlayerMove>().SetHasKey(true);

            IsSFXPlayed = true;
        }
    }

    public void KeyFollowing()
    {
        if (IsFollowingPlayer && PlayerTransform != null)
        {
            float xOffset = PlayerTransform.localScale.x > 0 ? 0.9f : -0.9f;

            Vector2 targetPosition = (Vector2)PlayerTransform.position + new Vector2(xOffset, 0.5f);

            transform.position =
                Vector2.Lerp(transform.position, targetPosition, followingSpeed * Time.deltaTime);
        }
    }

    public void KeyUsed()      //���� �����ٸ� ���� ������Ʈ ��Ȱ��ȭ
    {
        gameObject.SetActive(false);
    }

}
