using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostLoadConfirmController : MonoBehaviour {
    private HostLoadConfirmButton[] buttons;
    private OnlineMenuManager onlineMenuManager;

    private void Awake() {
        buttons = FindObjectsOfType<HostLoadConfirmButton>();
        onlineMenuManager = FindObjectOfType<OnlineMenuManager>();
    }

    public void GetHoverComponent(string name) {
        foreach (var component in buttons) {
            if (component.gameObject.name.Equals(name)) {
                component.EnableImage();
            }
            else {
                component.DisEnableImage();
            }
        }
    }

    public void NewGameButtonClick() {
        onlineMenuManager.OpenHostMenu();
    }

    public void LoadGameButtonClick() {

    }
}
