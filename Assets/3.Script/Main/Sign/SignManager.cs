using UnityEngine;

public class SignManager : MonoBehaviour {
    private LoginController loginController;
    private RegistController registController;
    private InfoController infoController;
    private SignFailController failController;

    private void Awake() {
        loginController = FindObjectOfType<LoginController>();
        registController = FindObjectOfType<RegistController>();
        infoController = FindObjectOfType<InfoController>();
        failController = FindObjectOfType<SignFailController>();
    }

    private void Start() {
        loginController.gameObject.SetActive(false);
        registController.gameObject.SetActive(false);
        infoController.gameObject.SetActive(false);
        failController.gameObject.SetActive(false);
    }

    private void OnEnable() {
        if (SQLManager.instance != null) {
            if (SQLManager.instance.UserData == null) {
                OpenLogin();
            }
            else {
                OpenInfo();
            }
        }
    }

    public void OpenLogin() {
        loginController.gameObject.SetActive(true);
        registController.gameObject.SetActive(false);
        infoController.gameObject.SetActive(false);
        failController.gameObject.SetActive(false);
    }

    public void OpenFailDialog(string errorMsg) {
        failController.gameObject.SetActive(true);
        failController.SetErrorMessage(errorMsg);
    }

    public void OpenRegist() {
        loginController.LoginFieldReset();
        loginController.gameObject.SetActive(false);
        registController.gameObject.SetActive(true);
        infoController.gameObject.SetActive(false);
        failController.gameObject.SetActive(false);
    }

    public void OpenInfo() {
        loginController.gameObject.SetActive(false);
        registController.gameObject.SetActive(false);
        infoController.gameObject.SetActive(true);
       // failController.gameObject.SetActive(true);
    }

    public void CloseFailDialog() {
        failController.gameObject.SetActive(false);
    }
}
