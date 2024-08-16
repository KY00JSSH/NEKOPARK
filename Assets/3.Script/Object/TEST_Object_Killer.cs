using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Object_Killer : MonoBehaviour
{
    private PlayerMove player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.Die();
        }
    }
}
