using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
    private Canvas[] canvases;

    private void Awake() {
        canvases = GetComponentsInChildren<Canvas>();
    }

    private void Start() {
        OpenMenuCanvas();
    }

    public void OpenMenuCanvas() {
        canvases[0].gameObject.SetActive(true);
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(false);
    }

    public void OpenOnlineCanvas() {
        canvases[0].gameObject.SetActive(false);
        canvases[1].gameObject.SetActive(true);
        canvases[2].gameObject.SetActive(false);
    }

    public void OpenJoinCanvas() {
        canvases[0].gameObject.SetActive(false);
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(true);
    }
}
