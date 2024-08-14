using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour {
    private int maxPlayerCount;
    private bool isSelectMenu;

    [SerializeField] private Text maxCountText;
    private LobbyMenuController lobbyMenuController;

    private void Awake() {
        maxPlayerCount = PlayerPrefs.GetInt("MaxPlayer");
        lobbyMenuController = FindObjectOfType<LobbyMenuController>();
    }

    private void Start() {
        maxCountText.text = ""+maxPlayerCount;
        lobbyMenuController.gameObject.SetActive(false);
    }

    private void Update() {
        if (
            (!isSelectMenu && Input.GetButtonDown("menu"))
                || (!isSelectMenu && Input.GetMouseButtonDown(0))
            ) {
            ShowMenu();
            return;
        }
    }

    private void ShowMenu() {
        lobbyMenuController.gameObject.SetActive(true);
        isSelectMenu = true;
    }

    public void CloseMenu() {
        lobbyMenuController.gameObject.SetActive(false);
        isSelectMenu = false;
    }
}
