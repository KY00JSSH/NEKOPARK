using UnityEngine;
using UnityEngine.UI;

public class InfoUpdateController : MonoBehaviour {
    private InputField[] inputs;
    private SignManager signManager;
    private InfoController infoController;

    private int focusIndex = 0;

    private void Awake() {
        inputs = GetComponentsInChildren<InputField>();
        signManager = FindObjectOfType<SignManager>();
        infoController = FindObjectOfType<InfoController>();
    }

    private void Update() {
        if (Input.GetButtonDown("Tab")) {
            changeFocus();
            return;
        }

        if (Input.GetButtonDown("menu")) {
            UpdateButtonClick();
        }
    }

    private void changeFocus() {
        if (focusIndex == 2) {
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

    public void UpdateButtonClick() {
        if (inputs[0].text.Equals(string.Empty) || inputs[1].text.Equals(string.Empty) ||
                inputs[2].text.Equals(string.Empty) || inputs[3].text.Equals(string.Empty)) {
            signManager.OpenFailDialog("There are values you did not enter\nPlease check again.");
            return;
        }

        if (!inputs[2].text.Equals(inputs[3].text)) {
            signManager.OpenFailDialog("Password doesn't match\nPlease check again.");
            return;
        }

        DBState updateState = SQLManager.instance.UpdateUsrInfo(inputs[1].text, inputs[0].text);

        switch (updateState) {
            case DBState.UPD_SUCCESS:
                infoController.OpenInfoDetail();
                break;
            case DBState.UPD_ERROR:
                signManager.OpenFailDialog(SQLManager.instance.MessageByState(updateState));
                break;
            case DBState.DIS_CONNECT:
                signManager.OpenFailDialog(SQLManager.instance.MessageByState(updateState));
                break;
        }
    }
}
