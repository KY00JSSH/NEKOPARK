using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_DoorController : MonoBehaviour
{
    /*
    �� ������Ʈ(Door)�� ���̴� ��ũ��Ʈ.
    ���踦 ���� �÷��̾ W �Ǵ� �� ����Ű�� ���� ���, ���� ������ ���԰����ϰ� �ȴ�.
    */

    private Transform PlayerTransform;

    private Collider2D Door_outer; //���� �� �ݶ��̴�
    private Collider2D Door_inner; //���� �� ���� ������ ���� �ݶ��̴�

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
        1. ���� �����ְ�
        2. �÷��̾ Ű�� ������ �ְ�, ���� ���� ������
        3. wŰ �Ǵ� �� ����Ű�� �Է����� ���
         -> ���� ������
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
        1. ���� �����ְ�
        2. �÷��̾ ���� ���� ���� ������
        3. wŰ �Ǵ� �� ����Ű�� �Է����� ���
         -> ���� ���������� �̵�
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
            //�������� �Ѿ�� ����
        }
    }
}
