using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuManager : MonoBehaviour {
    [SerializeField] private Image[] playerIcons;
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
