using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_DoorController : MonoBehaviour
{
    /*
    문 오브젝트(Door)에 붙이는 스크립트.
    열쇠를 가진 플레이어가 W 또는 위 방향키를 누를 경우, 문이 열리고 출입가능하게 된다.
    */

    private Transform PlayerTransform;

    private Collider2D Door_outer; //갈색 문 콜라이더
    private Collider2D Door_inner; //문을 연 후의 검은색 공간 콜라이더

    private bool PlayerHasKey = false;

    private Object_KeyController key;

    private void Awake()
    {
        PlayerTransform = GetComponent<Transform>();

        Door_outer = transform.GetChild(0).GetComponent<Collider2D>();
        Door_inner = transform.GetChild(1).GetComponent<Collider2D>();

        key = FindObjectOfType<Object_KeyController>();
    }

    private void Update()
    {
        OpenDoor_EnterDoor();
    }

    private void OpenDoor_EnterDoor()
    {
        /*
        1. 문이 닫혀있고
        2. 플레이어가 키를 가지고 있고, 문의 범위 내에서
        3. w키 또는 위 방향키를 입력했을 경우
         -> 문이 열린다
        */
        if (Door_outer.transform.localScale == Vector3.one && Door_inner.transform.localScale == Vector3.zero)
        {
            if((!PlayerHasKey) && PlayerTransform != null)
            { 
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Door_outer.transform.localScale = Vector3.zero;
                    Door_inner.transform.localScale = Vector3.one;
                    
                    key.KeyUsed();
                    AudioManager.instance.PlaySFX(AudioManager.Sfx.getKeyDoorOpen);
                }
            }           
        }

        /*
        1. 문이 열려있고
        2. 플레이어가 열린 문의 범위 내에서
        3. w키 또는 위 방향키를 입력했을 경우
         -> 다음 스테이지로 이동
        */
        else if (Door_outer.transform.localScale == Vector3.zero && Door_inner.transform.localScale == Vector3.one)
        {
            if (PlayerTransform != null && Door_inner.bounds.Contains(PlayerTransform.position))
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    NextStage();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        { 
            PlayerTransform = collision.transform;

            PlayerHasKey = collision.GetComponent<PlayerMove>().Haskey;
        }
    }

    private void NextStage()
    {
        if(Door_outer.transform.localScale == Vector3.zero && Door_inner.transform.localScale == Vector3.one)
        {
            //스테이지 넘어가는 로직
        }
    }
}
