using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMainListController : MonoBehaviour {
    private Button[] buttons;

    private void Awake() {
        buttons = GetComponentsInChildren<Button>();
    }

    public void CheckHoverIndex(string name) {
        if (name.Contains("01")) {
            buttons[0].GetComponent<GameButtonController>().EnableOutline();
            buttons[1].GetComponent<GameButtonController>().DisEnableOutline();
            buttons[2].GetComponent<GameButtonController>().DisEnableOutline();
        }
        else if(name.Contains("02")){
            buttons[0].GetComponent<GameButtonController>().DisEnableOutline();
            buttons[1].GetComponent<GameButtonController>().EnableOutline();
            buttons[2].GetComponent<GameButtonController>().DisEnableOutline();
        }
        else {
            buttons[0].GetComponent<GameButtonController>().DisEnableOutline();
            buttons[1].GetComponent<GameButtonController>().DisEnableOutline();
            buttons[2].GetComponent<GameButtonController>().EnableOutline();
        }
    }
}
