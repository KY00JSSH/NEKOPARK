using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum OnlineMenuType {
    PUBLIC = 4,
    FREIND,
    HOST,
    OPTION
}
public class OnlineMenuManager : MonoBehaviour {
    private Button[] buttons;
    private OnlineMenuType onMenuType;
    private int buttonHover = 0;
    private bool isMenuSelect = false;

    private MainManager mainManager;

    private void Awake() {
        buttons = GetComponentsInChildren<Button>();
        mainManager = FindObjectOfType<MainManager>();

        setMenuType(OnlineMenuType.PUBLIC);
    }

    public void setMenuType(OnlineMenuType type) {
        onMenuType = type;
        checkButtonOutline();
    }

    private void Update() {

    }

    private void checkButtonOutline() {
        buttons[0].image.enabled = false;
        buttons[1].image.enabled = false;
        buttons[2].image.enabled = false;
        buttons[3].image.enabled = false;

        switch (onMenuType) {
            case OnlineMenuType.PUBLIC:                
                buttons[0].image.enabled = true;
                break;
            case OnlineMenuType.FREIND:
                buttons[1].image.enabled = true;
                break;
            case OnlineMenuType.HOST:
                buttons[2].image.enabled = true;
                break;
            case OnlineMenuType.OPTION:
                buttons[3].image.enabled = true;
                break;
        }
    }

    public void GoMenu() {
        switch (onMenuType) {
            case OnlineMenuType.PUBLIC:
                mainManager.OpenOnlineCanvas();
                break;
            case OnlineMenuType.FREIND:
                mainManager.OpenOnlineCanvas();
                break;
            case OnlineMenuType.HOST:
                openHostMenu();
                break;
            case OnlineMenuType.OPTION:
                //TODO: (수진) OPTION 추가하기
                break;
        }
    }

    private void openHostMenu() {

    }
}
