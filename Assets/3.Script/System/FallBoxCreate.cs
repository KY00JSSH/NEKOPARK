using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBoxCreate : MonoBehaviour
{
    public GameObject FallboxPrefab;


    [Header("FallBox Num")]
    public int FallBoxCreateNum;

    private void Awake() {
        for (int i = 0; i < FallBoxCreateNum; i++) {
            Vector2 fallboxsposition = new Vector2(transform.position.x + i, transform.position.y);
            GameObject fallboxs = Instantiate(FallboxPrefab, fallboxsposition, Quaternion.identity);
            fallboxs.transform.SetParent(transform);
            fallboxs.name = FallboxPrefab.name;
        }
    }
}
