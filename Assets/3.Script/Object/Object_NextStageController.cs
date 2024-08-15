using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_NextStageController : MonoBehaviour
{
    [SerializeField] private Transform Door_inner; //갈색 문 콜라이더
    [SerializeField] private Object_DoorController Door_outer; //문을 연 후의 검은색 공간 콜라이더

    private Object_KeyController key;

    private void Awake()
    {
        Door_inner = GetComponent<Transform>();
        Door_outer = FindObjectOfType<Object_DoorController>();

        key = FindObjectOfType<Object_KeyController>();
    }

    private void NextStage()
    {
        if (Door_outer.transform.localScale == Vector3.zero && Door_inner.transform.localScale == Vector3.one)
        {
            //스테이지 넘어가는 로직
        }
    }
}
