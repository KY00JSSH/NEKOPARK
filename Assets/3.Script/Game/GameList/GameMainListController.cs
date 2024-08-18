using UnityEngine;
using UnityEngine.UI;

public class GameMainListController : MonoBehaviour {
    private GameMainButtonController[] buttons;
    private GameListUIManager gameListUIManager;

    private void Awake() {
        buttons = FindObjectsOfType<GameMainButtonController>();
        gameListUIManager = FindObjectOfType<GameListUIManager>();
    }

    public void CheckHoverIndex(string name) {
        if (name.Contains("01")) {
            buttons[0].EnableOutline();
            buttons[1].DisEnableOutline();
            buttons[2].DisEnableOutline();
        }
        else if(name.Contains("02")){
            buttons[0].DisEnableOutline();
            buttons[1].EnableOutline();
            buttons[2].DisEnableOutline();
        }
        else {
            buttons[0].DisEnableOutline();
            buttons[1].DisEnableOutline();
            buttons[2].EnableOutline();
        }
    }

    public void OpenStage(int index) {
        gameListUIManager.OpenDetailList();
    }
}
