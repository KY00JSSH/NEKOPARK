using UnityEngine;

public class InfoController : MonoBehaviour
{
    private InfoUpdateController infoUpdateController;
    private InfoDetailController infoDetailController;

    private void Awake() {
        infoUpdateController = FindObjectOfType<InfoUpdateController>();
        infoDetailController = FindObjectOfType<InfoDetailController>();
    }

    private void Start() {
        infoDetailController.gameObject.SetActive(false);
        infoUpdateController.gameObject.SetActive(false);
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
