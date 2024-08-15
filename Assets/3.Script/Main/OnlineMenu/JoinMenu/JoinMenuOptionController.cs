using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinMenuOptionController : MonoBehaviour
{
    private Button checkBox;
    private Image checkImage;
    private bool isChecked = true;
    private JoinGameManager joinGameManager;

    private void Awake() {
        joinGameManager = FindObjectOfType<JoinGameManager>();
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons) {
            if (button.name.Contains("Check")) {
                checkBox = button;
            }
        }
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        checkImage = images[2];
    }

    public void ClickCheckBox() {
        if (isChecked) {
            isChecked = false;
            checkImage.enabled = false;
            joinGameManager.CheckOptions(gameObject.name, isChecked);
        }
        else {
            isChecked = true;
            checkImage.enabled = true;
            joinGameManager.CheckOptions(gameObject.name, isChecked);
        }
    }
}
