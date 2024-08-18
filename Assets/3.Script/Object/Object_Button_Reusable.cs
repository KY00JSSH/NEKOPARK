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

        PushedButtonBox = transform.parent.GetChild(1).GetComponent<BoxCollider2D>();
        PushedButton = transform.parent.GetChild(1);

        PushedButton.localScale = Vector3.zero;
        Button.localScale = Vector3.one;        
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

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (collision.bounds.Intersects(PushedButtonBox.bounds))
    //        {
    //            Debug.Log("�÷��̾ ���� ��ư ���� �ȿ� �ֽ��ϴ�.");
    //        }
    //        else
    //        {
    //            Debug.Log("�÷��̾ ���� ��ư ���� �ۿ� �ֽ��ϴ�.");
    //        }
    //    }
    //}
    //

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PushedButtonBox.bounds.Contains(Player.transform.position))
        {           
                PushedButton.localScale = Vector3.zero;
                Button.localScale = Vector3.one;

                Debug.Log("��ư���� ������ϴ�.");            
        }            
    }

    //private void ResetButton()
    //{
    //    if(!PushedButtonBox.bounds.Contains(Player.transform.position))
    //    {
    //        PushedButton.localScale = Vector3.zero;
    //        Button.localScale = Vector3.one;
    //
    //        Debug.Log("��ư���� ������ϴ�.");
    //    }
    //}

}
