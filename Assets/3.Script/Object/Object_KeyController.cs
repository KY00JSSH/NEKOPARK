using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_KeyController : MonoBehaviour
{
    /*
    열쇠 오브젝트(Key)에 붙이는 스크립트.
    플레이어 따라다니기, 문 열 때 사라지기 등등 기본적인 기능 탑재.
    */

    private Transform PlayerTransform;
    private Collider2D KeyCollider; //필요없나 확인할 것 240814 10:53

    private bool IsFollowingPlayer = false;
    public float followingSpeed = 2.0f;            //열쇠가 플레이어를 따라가는 속도

    private void Awake()
    {
        PlayerTransform = GetComponent<Transform>();
        KeyCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        KeyFollowing();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerTransform = collision.transform;
            IsFollowingPlayer = true;

            collision.GetComponent<PlayerMove>().SetHasKey(true);
            AudioManager.instance.PlaySFX(AudioManager.Sfx.getKeyDoorOpen);
        }
    }

    private void KeyFollowing()
    {
        if (IsFollowingPlayer && PlayerTransform != null)
        {
            transform.position =
                Vector2.Lerp(transform.position, PlayerTransform.position, followingSpeed * Time.deltaTime);
        }
    }

    public void KeyUsed()      //문을 열었다면 열쇠 오브젝트 비활성화
    {
        gameObject.SetActive(false);
    }

}
