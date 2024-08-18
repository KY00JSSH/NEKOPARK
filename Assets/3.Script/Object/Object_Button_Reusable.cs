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

                Debug.Log("버튼을 눌렀습니다.");
            }
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (collision.bounds.Intersects(PushedButtonBox.bounds))
    //        {
    //            Debug.Log("플레이어가 눌린 버튼 영역 안에 있습니다.");
    //        }
    //        else
    //        {
    //            Debug.Log("플레이어가 눌린 버튼 영역 밖에 있습니다.");
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

                Debug.Log("버튼에서 벗어났습니다.");            
        }            
    }

    //private void ResetButton()
    //{
    //    if(!PushedButtonBox.bounds.Contains(Player.transform.position))
    //    {
    //        PushedButton.localScale = Vector3.zero;
    //        Button.localScale = Vector3.one;
    //
    //        Debug.Log("버튼에서 벗어났습니다.");
    //    }
    //}

}
