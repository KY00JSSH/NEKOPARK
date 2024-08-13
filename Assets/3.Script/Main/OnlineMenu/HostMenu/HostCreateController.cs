using UnityEngine;

public class HostCreateController : MonoBehaviour
{
    private void Awake() {
        CloseCreateLoading();
    }

    public void OpenCreateLoading() {
        gameObject.SetActive(true);
    }

    public void CloseCreateLoading() {
        gameObject.SetActive(false);
    }

    private void compeleteCreate() {
        FindObjectOfType<MainManager>().OpenJoinHintCanvas();
    }
}
