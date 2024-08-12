using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    private bool isSelectMenu = false;
    private int localPlayerCount = -1;
    private bool isConfirm = false;

    [SerializeField] private GameObject menu;
    private MenuTextController menuTextController;

    private void Awake() {
        menuTextController = FindObjectOfType<MenuTextController>();
    }

    private void Update() {
        if (!isSelectMenu && Input.GetKeyDown(KeyCode.Return)) {
            isSelectMenu = true;
            showMenu();
        }

        if (!isSelectMenu && Input.GetKeyDown(KeyCode.Space)) {
            isSelectMenu = true;
            showMenu();
        }

        if (!isSelectMenu && Input.GetMouseButtonDown(0)) {
            isSelectMenu = true;
            showMenu();
        }

        if (isSelectMenu && !isConfirm) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
                switch (menuTextController.getSelectMenu()) {
                    case MenuType.PLAY_LOCAL:
                        if (localPlayerCount != -1) {
                            isConfirm = true;
                        }
                        else {
                            //플레이어 숫자 선택창 보이기
                        }
                            break;
                    case MenuType.PLAY_ONLINE:
                        isConfirm = true;
                        break;
                    case MenuType.OPTION:
                        break;
                    case MenuType.EXIT:
                        isConfirm = true;
                        break;
                }
            }
        }

        if (isSelectMenu || isConfirm) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                closeMenu();
            }
        }
    }

    private void showMenu() {
        menu.SetActive(true);
    }

    public void closeMenu() {
        menu.SetActive(false);
        isSelectMenu = false;
    }

    public void clickMenu() {

    }
}
