using UnityEngine;
using UnityEngine.UI;

public class HostLoadListController : MonoBehaviour {
    private Button[] listButton;

    private void Awake() {
        listButton = GetComponentsInChildren<Button>();
    }

    private void Start() {
        for (int i = 0; i < listButton.Length; i++) {
            listButton[i].gameObject.SetActive(false);
        }
    }

    public void GetLoadList() {
        gameObject.SetActive(true);
        //TODO: 세이브 파일 목록 가져오기
    }
}
