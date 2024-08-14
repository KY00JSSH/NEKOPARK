using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_DoorController : MonoBehaviour
{
    /*
    �ѹ� ������Ʈ(Door)�� ���̴� ��ũ��Ʈ.
    ���踦 ���� �÷��̾ W �Ǵ� �� ����Ű�� ���� ���, ���� ������ ���԰����ϰ� �ȴ�.
    */

    [SerializeField] private Transform Door_outer; //���� �� �ݶ��̴�
    [SerializeField] private Object_NextStageController Door_inner; //���� �� ���� ������ ���� �ݶ��̴�

    private Object_KeyController key;
    
    private void Awake()
    {        
        Door_outer = GetComponent<Transform>();
        Door_inner = FindObjectOfType<Object_NextStageController>();

        key = FindObjectOfType<Object_KeyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D ȣ���: " + collision.name);

        if (collision.CompareTag("Key"))
        {
            if(Door_outer.transform.localScale == Vector3.one && Door_inner.transform.localScale == Vector3.zero)
            {               
                Debug.Log("���� ������ �õ��մϴ�.");

                Door_outer.transform.localScale = Vector3.zero;
                Door_inner.transform.localScale = Vector3.one;
                Debug.Log("���� ���Ƚ��ϴ�.");


                key.KeyUsed();
                Debug.Log("���� ��� �Ϸ�");
                AudioManager.instance.PlaySFX(AudioManager.Sfx.getKeyDoorOpen);
                Debug.Log("���� ��� �Ϸ�");                
            }
        }
        else
        {
            Debug.Log("�浹�� ������Ʈ�� Key �±װ� �ƴմϴ�: " + collision.tag);
        }
    }

}
