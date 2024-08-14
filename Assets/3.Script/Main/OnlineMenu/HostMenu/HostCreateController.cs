using UnityEngine;
using System.Collections;

public class HostCreateController : MonoBehaviour
{
    private void Start() {
        CloseCreateLoading();
    }

    public void OpenCreateLoading() {
        gameObject.SetActive(true);
        StartCoroutine(delayStart());
    }

    public void CloseCreateLoading() {
        gameObject.SetActive(false);
    }

    private void compeleteCreate() {
        FindObjectOfType<MainManager>().OpenJoinHintCanvas();
    }

    private IEnumerator delayStart() {
        yield return new WaitForSeconds(1f);
        compeleteCreate();
    }
}
