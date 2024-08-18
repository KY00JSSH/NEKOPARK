using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Reflection;
using static UnityEditor.Progress;

public class HostLoadListController : MonoBehaviour {
    private SaveDataButtonController[] buttons;

    // 24 08 18 ±è¼öÁÖ Multi List Load Ãß°¡ 
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
            // µð¹ö±ë
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
