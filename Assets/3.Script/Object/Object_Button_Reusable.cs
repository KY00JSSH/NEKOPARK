using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Button_Reusable : MonoBehaviour
{
    private PlayerMove Player;
       
    private BoxCollider2D ButtonBox; //눌리기 전 버튼 빨간부분
    private Transform Button; // 눌리기 전 버튼

    private BoxCollider2D PushedButtonBox; // 눌린 후의 버튼 빨간 부분    
    private Transform PushedButton; //눌린 후의 버튼         


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

                Debug.Log("버튼을 눌렀습니다.");
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
