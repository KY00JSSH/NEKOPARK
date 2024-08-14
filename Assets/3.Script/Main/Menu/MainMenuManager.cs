using UnityEngine;

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
        confirmManager.gameObject.SetActive(false);
    }

    private void Update() {
        if (!isConfirm && isSelectMenu && Input.GetButtonDown("Cancel")) {
            CloseMenu();
            return;
        }

        //메인 메뉴 OPEN
        if (
            (!isSelectMenu && Input.GetButtonDown("menu"))
                || (!isSelectMenu && Input.GetButtonDown("Select"))
                || (!isSelectMenu && Input.GetMouseButtonDown(0))
            ) {
            ShowMenu(); 
            return;
        }

        //메인 메뉴 이동
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

        //컨펌 창 CLOSE
        if (isConfirm && Input.GetButtonDown("Cancel")) {
            CloseConfirm();
            return;
        }

        //컨펌 창 선택 → 메뉴 이동 / 컨펌창 닫기
        if ((isConfirm && Input.GetButtonDown("Select")) || (isConfirm && Input.GetButtonDown("menu"))) {
            if (confirmManager.GetIsHoverYes()) {
                GoToMenu();
            }
            else {
                CloseConfirm(); 
                return;
            }
        }

        //컨펌 창 OPEN OR MENU 이동
        if ((!isConfirm && isSelectMenu && Input.GetButtonDown("Select")) || (!isConfirm && isSelectMenu && Input.GetButtonDown("menu"))) {
            switch (mainMenuController.GetSelectMenu()) {
                case MenuType.PLAY_LOCAL:
                    //TODO: (수진) LOCAL MODE 추가하기
                    break;
                case MenuType.PLAY_ONLINE:
                    ShowConfirm();
                    confirmManager.ConfirmTextChage(mainMenuController.GetSelectMenu());
                    break;
                case MenuType.OPTION:
                    //TODO: (수진) OPTION 추가하기
                    break;
                case MenuType.EXIT:
                    ShowConfirm();
                    confirmManager.ConfirmTextChage(mainMenuController.GetSelectMenu());
                    break;
            }
            return;
        }
    }

    #region 메인 메뉴
    private void ShowMenu() {
        mainMenuController.gameObject.SetActive(true);
        isSelectMenu = true;
    }

    public void CloseMenu() {
        mainMenuController.gameObject.SetActive(false);
        isSelectMenu = false;
    }
    #endregion

    #region 컨펌창
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
                break;
            case MenuType.PLAY_ONLINE:
                mainManager.OpenOnlineCanvas();
                break;
            case MenuType.OPTION:
                break;
            case MenuType.EXIT:
                break;
        }
    }
}
