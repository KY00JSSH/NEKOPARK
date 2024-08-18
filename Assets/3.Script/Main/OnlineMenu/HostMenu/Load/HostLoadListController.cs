using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HostLoadListController : MonoBehaviour {
    private Button[] listButton;

    // 24 08 18 김수주 Multi List Load 추가 
    private Dictionary<string, StageSaveData> multiSaveDictionary;

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
        multiSaveDictionary = Save.instance.LoadMultiFiles();

        if(multiSaveDictionary != null) {

            // 디버깅
            foreach (var item in multiSaveDictionary) {
                Debug.Log(item.Key);
                Debug.Log(item.Value);
            }
        }

    }
}
