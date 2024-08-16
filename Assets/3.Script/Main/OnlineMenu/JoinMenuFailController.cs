using UnityEngine;

public class JoinMenuFailController : MonoBehaviour {
    private OnlineMenuManager onlineMenuManager;

    private void Awake() {
        onlineMenuManager = FindObjectOfType<OnlineMenuManager>();
    }

    public void CloseDialog() {
        gameObject.SetActive(false);
        onlineMenuManager.ServerFailFlagUpdate(false);
    }

    public void OpenDialog() {
        gameObject.SetActive(true);
        onlineMenuManager.ServerFailFlagUpdate(true);
    }
}
