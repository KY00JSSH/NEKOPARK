using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //�÷��̾� �̵��� ����, �ִϸ��̼� ���� ����

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
    }
}
