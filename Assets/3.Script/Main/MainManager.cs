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
        canvases[0].gameObject.SetActive(true);
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(false);
        canvases[3].gameObject.SetActive(false);
    }

    public void OpenOnlineCanvas() {
        canvases[0].gameObject.SetActive(false);
        canvases[1].gameObject.SetActive(true);
        canvases[2].gameObject.SetActive(false);
        canvases[3].gameObject.SetActive(false);
        joinMenuFailController.CloseDialog();
    }

    public void OpenJoinCanvas() {
        if (joinGameManager.JoinRoomSetting()) {
            canvases[0].gameObject.SetActive(false);
            canvases[1].gameObject.SetActive(false);
            canvases[2].gameObject.SetActive(true);
            canvases[3].gameObject.SetActive(false);
        }
        else {
            joinMenuFailController.OpenDialog();
        }
    }

    public void OpenSignCanvas() {
        canvases[0].gameObject.SetActive(false);
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(false);
        canvases[3].gameObject.SetActive(true);
    }
}
