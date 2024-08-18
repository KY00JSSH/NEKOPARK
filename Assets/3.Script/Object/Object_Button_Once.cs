using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Button_Once : MonoBehaviour
{
    private BoxCollider2D ButtonBox;
    private PlayerMove Player;

    private Transform PushedButton;
    private Transform Button;

    //24 08 17 ����� button push�� �������� �ϱ� ���� �� �߰�
    private bool isButtonPushed;
    public bool GetIsButtonPushed() { return isButtonPushed; }

    private Transform DontPushTransform;

    private void Awake()
    {
        ButtonBox = GetComponent<BoxCollider2D>();
        Player = FindObjectOfType<PlayerMove>();

        PushedButton = transform.parent.GetChild(1);
        Button = transform;

        if (transform.parent.childCount > 2)
        {
            DontPushTransform = transform.parent.GetChild(2);
        }
        else
        {
            DontPushTransform = null; // �ڽ��� ���� ��� null�� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(collision.bounds.Intersects(ButtonBox.bounds))
            {
                Button.localScale = Vector3.zero;
                PushedButton.localScale = Vector3.one;

                Debug.Log("��ư�� �������ϴ�.");
                isButtonPushed = true;

                if(DontPushTransform != null)
                {
                    Player.Die();
                    
                }
            }  
        }
    }
}
