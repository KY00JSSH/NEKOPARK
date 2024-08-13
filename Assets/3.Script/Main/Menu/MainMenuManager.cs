using System.Collections;
using System.Collections.Generic;
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

        if (
            (!isSelectMenu && Input.GetButtonDown("menu"))
                || (!isSelectMenu && Input.GetButtonDown("Select"))
                || (!isSelectMenu && Input.GetMouseButtonDown(0))
            ) {
            ShowMenu();
            return;
        }

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

        if(isConfirm && Input.GetButtonDown("Cancel")) {
            CloseConfirm();
            return;
        }

        if ((isConfirm && Input.GetButtonDown("Select")) || (isConfirm && Input.GetButtonDown("menu"))) {
            if (confirmManager.GetIsHoverYes()) {
                GoToMenu();
            }
            else {
                CloseConfirm(); 
                return;
            }
        }

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

    private void ShowMenu() {
        mainMenuController.gameObject.SetActive(true);
        isSelectMenu = true;
    }

    public void CloseMenu() {
        mainMenuController.gameObject.SetActive(false);
        isSelectMenu = false;
    }

    public void ShowConfirm() {
        confirmManager.gameObject.SetActive(true);
        isConfirm = true;
    }

    public void CloseConfirm() {
        confirmManager.gameObject.SetActive(false);
        isConfirm = false;
    }

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
