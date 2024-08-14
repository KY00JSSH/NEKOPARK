using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_NextStageController : MonoBehaviour
{
    [SerializeField] private Transform Door_inner; //���� �� �ݶ��̴�
    [SerializeField] private Object_DoorController Door_outer; //���� �� ���� ������ ���� �ݶ��̴�

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
            //�������� �Ѿ�� ����
        }
    }
}
