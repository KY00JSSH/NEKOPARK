using UnityEngine;

public class LobbyMenuManager : MonoBehaviour {
    private Canvas[] canvases;

    private void Awake() {
        canvases = GetComponentsInChildren<Canvas>();
    }

    private void Start() {
        OpenJoinHint();
    }

    public void OpenJoinHint() {
        canvases[0].gameObject.SetActive(false);
        canvases[1].gameObject.SetActive(true);
    }

    public void OpenRoom() {
        canvases[0].gameObject.SetActive(true);
        canvases[1].gameObject.SetActive(false);
    }
}
