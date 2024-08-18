using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListUIManager : MonoBehaviour {
    private Canvas[] canvas;

    private void Awake() {
        canvas = GetComponentsInChildren<Canvas>();
    }

    private void Start() {
        OpenMainList();
    }

    public void OpenMainList() {
        canvas[0].gameObject.SetActive(true);
        canvas[1].gameObject.SetActive(false);
    }

    public void OpenDetailList() {
        canvas[0].gameObject.SetActive(false);
        canvas[1].gameObject.SetActive(true);
    }
}
