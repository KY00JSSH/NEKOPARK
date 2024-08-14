using UnityEngine;
using UnityEngine.UI;

public enum MenuType {
    PLAY_LOCAL = 0,
    PLAY_ONLINE,
    OPTION,
    EXIT
}

public class MainMenuController : MonoBehaviour{
    //메인 메뉴 선택 관련 Controller

    private Text menuText;
    private MenuType menuType = MenuType.PLAY_LOCAL;

    private void Start() {
        menuText = gameObject.GetComponentInChildren<Text>();
    }

    private void Update() {
        menuTextChage(menuType);
    }

    private void menuTextChage(MenuType type) {
        switch (type) {
            case MenuType.PLAY_LOCAL:
                menuText.text = "LOCAL PLAY MODE";
                break;
            case MenuType.PLAY_ONLINE:
                menuText.text = "ONLINE PLAY MODE";
                break;
            case MenuType.OPTION:
                menuText.text = "OPTION";
                break;
            case MenuType.EXIT:
                menuText.text = "EXIT GAME";
                break;
        }
    }

    public MenuType GetSelectMenu() {
        return menuType;
    }

    public void NextButtonClick() {
        if ((int)menuType == 3) {
            menuType = MenuType.PLAY_LOCAL;
        }
        else {
            int newMenu = (int)menuType + 1;
            menuType = (MenuType)newMenu;
        }
    }

    public void PrevButtonClick() {
        if ((int)menuType == 0) {
            menuType = MenuType.EXIT;
        }
        else {
            int newMenu = (int)menuType - 1;
            menuType = (MenuType)newMenu;
        }
    }
}
