using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Button_Once : MonoBehaviour
{
    private BoxCollider2D ButtonBox;
    public PlayerMove Player;
    public LocalPlayerMove LocalPlayer;

    private Transform PushedButton;
    private Transform Button;

    //24 08 17 김수주 button push된 상태전달 하기 위한 값 추가
    private bool isButtonPushed;
    public bool GetIsButtonPushed() { return isButtonPushed; }

    private Transform DontPushTransform;

    private void Awake()
    {

    }

    private void Start()
    {
        ButtonBox = GetComponent<BoxCollider2D>();
        if (GameListManager.instance.IsLocalGame)
        {
            LocalPlayer = FindObjectOfType<LocalPlayerMove>();
        }
        else
        {
            Player = FindObjectOfType<PlayerMove>();
        }

        PushedButton = transform.parent.GetChild(1);
        Button = transform;

        if (transform.parent.childCount > 2)
        {
            DontPushTransform = transform.parent.GetChild(2);
        }
        else
        {
            DontPushTransform = null; // 자식이 없을 경우 null로 설정
            Debug.Log("자식이 없습니다.");
        }
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
                isButtonPushed = true;

                if (DontPushTransform != null)
                {
                    if (GameListManager.instance.IsLocalGame)
                    {
                        LocalPlayer.Die();
                    }
                    else
                    {
                        Player.Die();
                    }
                }
                else
                {
                    Debug.LogWarning("DontPushTransform이 null 상태입니다. Player.Die()가 호출되지 않았습니다.");
                }
            }
        }
    }
}
