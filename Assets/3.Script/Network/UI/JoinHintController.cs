using UnityEngine;

public class JoinHintController : MonoBehaviour
{
    private LobbyMenuManager lobbyMenuManager;

    private void Awake() {
        lobbyMenuManager = FindObjectOfType<LobbyMenuManager>();
    }

    private void Update() {
        if(Input.GetButtonDown("Select") || Input.GetButtonDown("menu")) {
            JoinOkClick();
        }
    }

    public void JoinOkClick() {
        lobbyMenuManager.OpenRoom();
    }
}
