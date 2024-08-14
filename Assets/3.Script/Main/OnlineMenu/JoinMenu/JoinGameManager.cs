using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameManager : MonoBehaviour {
    private MainManager mainManager;
    private JoinButtonHoverController[] joinButtonHovers;

    private void Awake() {
        mainManager = FindObjectOfType<MainManager>();
        joinButtonHovers = FindObjectsOfType<JoinButtonHoverController>();
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            mainManager.OpenOnlineCanvas();
        }
    }

    public void GetHoverComponent(string name) {
        foreach (var component in joinButtonHovers) {
            if (component.gameObject.name.Equals(name)) {
                component.EnableImage();
            }
            else {
                component.DisEnableImage();
            }
        }
    }
}
