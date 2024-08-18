using UnityEngine;
using UnityEngine.UI;

public class GameDetailListController : MonoBehaviour {

    private GameDetailButtonController[] buttons;
    private Text title;

    private void Awake() {
        buttons = GetComponentsInChildren<GameDetailButtonController>();
        title = GetComponentInChildren<Text>();
    }

    private void Start() {
        title.text = "01. HELLO NEKO PARK";
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].CloseStage();
        }
    }

    public void CheckHoverIndex(string name) {
        if (name.Contains("01")) {
            buttons[0].EnableOutline();
            buttons[1].DisEnableOutline();
            buttons[2].DisEnableOutline();
            buttons[3].DisEnableOutline();
        }
        else if (name.Contains("02")) {
            buttons[0].DisEnableOutline();
            buttons[1].EnableOutline();
            buttons[2].DisEnableOutline();
            buttons[3].DisEnableOutline();
        }
        else if(name.Contains("03")) {
            buttons[0].DisEnableOutline();
            buttons[1].DisEnableOutline();
            buttons[2].EnableOutline();
            buttons[3].DisEnableOutline();
        }
        else {
            buttons[0].DisEnableOutline();
            buttons[1].DisEnableOutline();
            buttons[2].DisEnableOutline();
            buttons[3].EnableOutline();
        }
    }
}
