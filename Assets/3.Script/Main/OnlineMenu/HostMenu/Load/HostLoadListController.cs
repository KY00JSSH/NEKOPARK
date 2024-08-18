using UnityEngine;
using UnityEngine.UI;

public class HostLoadListController : MonoBehaviour {
    private Button[] listButton;

    // 24 08 18 김수주 Multi List Load 추가
    private MultiSaveList multiSaveList;

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
        multiSaveList = Save.instance.LoadMultiFiles();

        if(multiSaveList != null) {
            multiSaveList.MultiList.Reverse();          // 역순 정렬(최신게 위로)
            foreach (StageSaveData item in multiSaveList.MultiList) {
                Debug.Log(item);            //Todo: [김수주] 디버깅 지우기
            }
        }

    }
}
