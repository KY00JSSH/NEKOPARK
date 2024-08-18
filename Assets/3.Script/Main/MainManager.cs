using UnityEngine;

public class MainManager : MonoBehaviour {
    private Canvas[] canvases;
    private JoinGameManager joinGameManager;
    private JoinMenuFailController joinMenuFailController;

    private void Awake() {
        canvases = GetComponentsInChildren<Canvas>();
        joinGameManager = FindObjectOfType<JoinGameManager>();
        joinMenuFailController = FindObjectOfType<JoinMenuFailController>();
    }

    private void Start() {
        OpenMenuCanvas();
    }

    public void OpenMenuCanvas() {
        for (int i = 0; i < canvases.Length; i++) {
            if (i == 0) {
                canvases[i].gameObject.SetActive(true);
            }
            else {
                canvases[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenOnlineCanvas() {
        if (SQLManager.instance.UserData != null) {
            for (int i = 0; i < canvases.Length; i++) {
                if (i == 1) {
                    canvases[i].gameObject.SetActive(true);
                }
                else {
                    canvases[i].gameObject.SetActive(false);
                }
            }
            joinMenuFailController.CloseDialog();
        }
        else {
            OpenSignCanvas();
        }
    }

    public void OpenJoinCanvas() {
        if (joinGameManager.JoinRoomSetting()) {
            for (int i = 0; i < canvases.Length; i++) {
                if (i == 2) {
                    canvases[i].gameObject.SetActive(true);
                }
                else {
                    canvases[i].gameObject.SetActive(false);
                }
            }
        }
        else {
            joinMenuFailController.OpenDialog();
        }
    }

    public void OpenSignCanvas() {
        for (int i = 0; i < canvases.Length; i++) {
            if (i == 3) {
                canvases[i].gameObject.SetActive(true);
            }
            else {
                canvases[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenDLCDialog() {
        canvases[4].gameObject.SetActive(true);
    }

    public void CloseDLCDialog() {
        canvases[4].gameObject.SetActive(false);
    }

}
