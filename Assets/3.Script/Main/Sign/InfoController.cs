using UnityEngine;

public class InfoController : MonoBehaviour
{
    private InfoDetailController infoDetailController;
    private InfoUpdateController infoUpdateController;

    private void Awake() {
        infoDetailController = FindObjectOfType<InfoDetailController>();
        infoUpdateController = FindObjectOfType<InfoUpdateController>();
    }

    private void OnEnable() {
        if (SQLManager.instance != null) {
            if (SQLManager.instance.UserData != null) {
                OpenInfoDetail();
            }
        }
    }

    public void OpenInfoDetail() {
        infoDetailController.gameObject.SetActive(true);
        infoUpdateController.gameObject.SetActive(false);
    }

    public void OpenInfoUpdate() {
        infoDetailController.gameObject.SetActive(false);
        infoUpdateController.gameObject.SetActive(true);
    }
}
