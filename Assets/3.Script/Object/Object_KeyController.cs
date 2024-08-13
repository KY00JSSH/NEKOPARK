using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_KeyController : MonoBehaviour
{
    private Collider2D KeyCollider;

    private void Awake()
    {
        KeyCollider = GetComponent<Collider2D>();
    }

}
