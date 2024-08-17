using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignFailController : MonoBehaviour {
    private Text errorText;

    private void Awake() {
        errorText = GetComponentInChildren<Text>();
    }

    public void SetErrorMessage(string errorMsg) {
        errorText.text = errorMsg;
    }
}
