using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {
    private InputField[] inputs;
    private Button[] buttons;
    private SignManager signManager;
    private MainManager mainManager;

    private bool isFocusid = true;

    private void Awake() {
        inputs = GetComponentsInChildren<InputField>();
        buttons = GetComponentsInChildren<Button>();
        signManager = FindObjectOfType<SignManager>();
    }

    private void Start() {
        inputs[0].Select();
        inputs[0].ActivateInputField();
    }

    private void Update() {
        if (Input.GetButtonDown("Tab")) {
            changeFocus();
            return;
        }

        if (Input.GetButtonDown("menu")) {
            LoginButtonClick();
        }
    }

    private void changeFocus() {
        if (isFocusid) {
            inputs[1].Select();
            inputs[1].ActivateInputField();
        }
        else {
            inputs[0].Select();
            inputs[0].ActivateInputField();
        }
    }

    public void LoginButtonClick() {
        if (inputs[0].text.Equals(string.Empty) || inputs[1].text.Equals(string.Empty)) {
            signManager.OpenFailDialog("Please enter your ID and password");
            return;
        }

        DBState loginState = SQLManager.instance.Login(inputs[0].text, inputs[1].text);
        switch (loginState) {
            case DBState.LGN_SUCCESS:
                mainManager.OpenOnlineCanvas();
                return;
            case DBState.LGN_ERROR:
                signManager.OpenFailDialog(SQLManager.instance.MessageByState(loginState));
                return;
            case DBState.DIS_CONNECT:
                signManager.OpenFailDialog(SQLManager.instance.MessageByState(loginState));
                return;
        }
    }
}