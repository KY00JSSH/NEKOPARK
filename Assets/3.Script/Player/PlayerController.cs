using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //플레이어 이동과 점프, 애니메이션 등을 제어

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
    }
}
