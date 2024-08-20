                            using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_DoorController : MonoBehaviour
{
    /*
    겉문 오브젝트(Door)에 붙이는 스크립트.
    열쇠를 가진 플레이어가 W 또는 위 방향키를 누를 경우, 문이 열리고 출입가능하게 된다.
    */

    [SerializeField] private Transform Door_outer; //갈색 문 콜라이더
    [SerializeField] private Object_NextStageController Door_inner; //문을 연 후의 검은색 공간 콜라이더

    private Object_KeyController key;
    
    public static bool IsDoorOpen { get; private set; }            // 24 08 17 김수주 : 플레이어 전체 들어왔는지 확인하는 값 추가
    private void Awake()
    {        
        Door_outer = GetComponent<Transform>();
        Door_inner = FindObjectOfType<Object_NextStageController>();

        key = FindObjectOfType<Object_KeyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D 호출됨: " + collision.name);

        if (collision.CompareTag("Key"))
        {
            if(Door_outer.transform.localScale == Vector3.one && Door_inner.transform.localScale == Vector3.zero)
            {               
                Debug.Log("문을 열려고 시도합니다.");

                Door_outer.transform.localScale = Vector3.zero;
                Door_inner.transform.localScale = Vector3.one;
                Debug.Log("문이 열렸습니다.");


                key.KeyUsed();
                Debug.Log("열쇠 사용 완료");
                AudioManager.instance.PlaySFX(AudioManager.Sfx.getKeyDoorOpen);
                Debug.Log("사운드 재생 완료");

                //TODO: [김수주] 플레이어 전체가 다 들어갔는지 확인하는 값 필요함 => 개별 플레이어마다 문에 닿아서 들어갔는지 확인하는 bool값 필요
                IsDoorOpen = true;
            }
        }
        else
        {
            Debug.Log("충돌한 오브젝트는 Key 태그가 아닙니다: " + collision.tag);
        }
    }

    private void OnDisable() {
        IsDoorOpen = false;
    }
}
