using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Button_Reusable : MonoBehaviour
{
    private PlayerMove Player;
       
    private BoxCollider2D ButtonBox; //������ �� ��ư �����κ�
    private Transform Button; // ������ �� ��ư

    private BoxCollider2D PushedButtonBox; // ���� ���� ��ư ���� �κ�    
    private Transform PushedButton; //���� ���� ��ư         


    private void Awake()
    {
        Player = FindObjectOfType<PlayerMove>();

        ButtonBox = GetComponent<BoxCollider2D>();
        Button = transform;

        PushedButton = transform.parent.GetChild(1);
        PushedButtonBox = transform.parent.GetChild(1).GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.bounds.Intersects(ButtonBox.bounds))
            {
                Button.localScale = Vector3.zero;
                PushedButton.localScale = Vector3.one;

                Debug.Log("��ư�� �������ϴ�.");
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Button.localScale = Vector3.one;
            PushedButton.localScale = Vector3.zero;
        }
    }
}
