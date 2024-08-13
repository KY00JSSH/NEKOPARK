using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_KeyController : MonoBehaviour
{
    private Collider2D KeyCollider;
    private Transform PlayerTransform;
    private bool IsFollowingPlayer = false;
    public float followSpeed = 2.0f;

    private void Awake()
    {
        KeyCollider = GetComponent<Collider2D>();
        PlayerTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if(IsFollowingPlayer && PlayerTransform != null)
        {
            transform.position = 
                Vector2.Lerp(transform.position, PlayerTransform.position, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerTransform = collision.transform;
            IsFollowingPlayer = true;
        }
    }

    private void KeyUsed()      //문을 열었다면 열쇠 오브젝트 비활성화
    {
        gameObject.SetActive(false);
    }
}
