using UnityEngine;
using UnityEngine.UI;

public class RegistController : MonoBehaviour {
    private InputField[] inputs;
    private Button[] buttons;
    private SignManager signManager;

    private int focusIndex = 0;

    private void Awake() {
        inputs = GetComponentsInChildren<InputField>();
        buttons = GetComponentsInChildren<Button>();
        signManager = FindObjectOfType<SignManager>();
    }

    private void Update() {
        if (Input.GetButtonDown("Tab")) {
            changeFocus();
            return;
        }

        if (Input.GetButtonDown("menu")) {
            RegistButtonClick();
        }
    }

    private void changeFocus() {
        if (focusIndex == 3) {
            inputs[0].Select();
            inputs[0].ActivateInputField();
            focusIndex = 0;
        }
        else {
            inputs[focusIndex + 1].Select();
            inputs[focusIndex + 1].ActivateInputField();
            focusIndex++;
        }
    }

    public void RegistButtonClick() {
        if (inputs[0].text.Equals(string.Empty) || inputs[1].text.Equals(string.Empty) ||
                inputs[2].text.Equals(string.Empty) || inputs[3].text.Equals(string.Empty)) {
            signManager.OpenFailDialog("There are values you did not enter\nPlease check again.");
            return;
        }

        if (!inputs[2].text.Equals(inputs[3].text)) {
            signManager.OpenFailDialog("Password doesn't match\nPlease check again.");
            return;
        }

        DBState registState = SQLManager.instance.SignUp(inputs[0].text, inputs[2].text, inputs[1].text);
        switch (registState) {
            case DBState.REG_SUCCESS:
                signManager.OpenLogin();
                break;
            case DBState.REG_ERROR:
                signManager.OpenFailDialog(SQLManager.instance.MessageByState(registState));
                break;
            case DBState.DIS_CONNECT:
                signManager.OpenFailDialog(SQLManager.instance.MessageByState(registState));
                break;
        }
    }
}
