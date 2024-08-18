using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    private bool isSelectMenu = false;
    private bool isConfirm = false;

    private MainMenuController mainMenuController;
    private ConfirmManager confirmManager;
    private MainManager mainManager;

    private void Awake() {
        mainMenuController = FindObjectOfType<MainMenuController>();
        mainManager = FindObjectOfType<MainManager>();
        confirmManager = FindObjectOfType<ConfirmManager>();
    }

    private void Start() {
        mainMenuController.gameObject.SetActive(false);
    }

    private void Update() {
        if (!isConfirm && isSelectMenu && Input.GetButtonDown("Cancel")) {
            CloseMenu();
            return;
        }

        //���� �޴� OPEN
        if (
            (!isSelectMenu && Input.GetButtonDown("menu"))
                || (!isSelectMenu && Input.GetButtonDown("Select"))
                || (!isSelectMenu && Input.GetMouseButtonDown(0))
            ) {
            ShowMenu(); 
            return;
        }

        //���� �޴� �̵�
        if (!isConfirm && isSelectMenu && Input.GetButtonDown("Horizontal")) {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput > 0) {
                mainMenuController.NextButtonClick();
            }
            else {
                mainMenuController.PrevButtonClick();
            }
            return;
        }

        //���� â CLOSE
        if (isConfirm && Input.GetButtonDown("Cancel")) {
            CloseConfirm();
            return;
        }

        //���� â ���� �� �޴� �̵� / ����â �ݱ�
        if ((isConfirm && Input.GetButtonDown("Select")) || (isConfirm && Input.GetButtonDown("menu"))) {
            if (confirmManager.GetIsHoverYes()) {
                GoToMenu();
            }
            else {
                CloseConfirm(); 
                return;
            }
        }

        //���� â OPEN OR MENU �̵�
        if ((!isConfirm && isSelectMenu && Input.GetButtonDown("Select")) || (!isConfirm && isSelectMenu && Input.GetButtonDown("menu"))) {
            switch (mainMenuController.GetSelectMenu()) {
                case MenuType.PLAY_LOCAL:
                    PlayerPrefs.SetInt("localGame", 1);
                    SceneManager.LoadScene("Game_List");
                    break;
                case MenuType.PLAY_ONLINE:
                    ShowConfirm();
                    confirmManager.ConfirmMainTextChage(mainMenuController.GetSelectMenu());
                    break;
                case MenuType.OPTION:
                    mainManager.OpenDLCDialog();
                    break;
                case MenuType.EXIT:
                    ShowConfirm();
                    confirmManager.ConfirmMainTextChage(mainMenuController.GetSelectMenu());
                    break;
            }
            return;
        }
    }

    #region ���� �޴�
    private void ShowMenu() {
        mainMenuController.gameObject.SetActive(true);
        isSelectMenu = true;
    }

    public void CloseMenu() {
        mainMenuController.gameObject.SetActive(false);
        isSelectMenu = false;
    }
    #endregion

    #region ����â
    public void ShowConfirm() {
        confirmManager.gameObject.SetActive(true);
        isConfirm = true;
    }

    public void CloseConfirm() {
        confirmManager.gameObject.SetActive(false);
        isConfirm = false;
    }
    #endregion

    public void GoToMenu() {
        switch (mainMenuController.GetSelectMenu()) {
            case MenuType.PLAY_LOCAL:
                PlayerPrefs.SetInt("localGame", 1);
                SceneManager.LoadScene("Game_List");
                break;
            case MenuType.PLAY_ONLINE:
                mainManager.OpenOnlineCanvas();
                break;
            case MenuType.OPTION:
                mainManager.OpenDLCDialog();
                break;
            case MenuType.EXIT:
                break;
        }
    }
}
