using UnityEngine;
using System.Collections.Generic;

public class HostLoadListController : MonoBehaviour {
    private SaveDataButtonController[] buttons;

    // 24 08 18 ����� Multi List Load �߰� 
    private Dictionary<string, StageSaveData> multiSaveDictionary;

    private void Awake() {
        buttons = FindObjectsOfType<SaveDataButtonController>();
    }

    private void Start() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void GetLoadList() {
        gameObject.SetActive(true);
        multiSaveDictionary = Save.instance.LoadMultiFiles();

        if (multiSaveDictionary != null) {
            int index = 0;
            // �����
            foreach (var item in multiSaveDictionary) {
                if (index >= multiSaveDictionary.Count) {
                    break;
                }
                buttons[index].gameObject.SetActive(true);
                buttons[index].SetLoadDataText(item.Key, item.Value);
                index++;
            }
        }
    }


}
