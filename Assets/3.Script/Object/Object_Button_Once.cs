using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Button_Once : MonoBehaviour
{
    private BoxCollider2D ButtonBox;
    private PlayerMove Player;

    private Transform PushedButton;
    private Transform Button;

    private void Awake()
    {
        ButtonBox = GetComponent<BoxCollider2D>();
        Player = FindObjectOfType<PlayerMove>();

        PushedButton = transform.parent.GetChild(1);
        Button = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(collision.bounds.Intersects(ButtonBox.bounds))
            {
                Button.localScale = Vector3.zero;
                PushedButton.localScale = Vector3.one;

                Debug.Log("버튼을 눌렀습니다.");
            }
            
        }
    }
}
