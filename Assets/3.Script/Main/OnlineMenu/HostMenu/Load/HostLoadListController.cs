using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HostLoadListController : MonoBehaviour {
    private Button[] listButton;

    // 24 08 18 ����� Multi List Load �߰� 
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
        //TODO: ���̺� ���� ��� ��������
        multiSaveDictionary = Save.instance.LoadMultiFiles();

        if(multiSaveDictionary != null) {

            // �����
            foreach (var item in multiSaveDictionary) {
                Debug.Log(item.Key);
                Debug.Log(item.Value);
            }
        }

    }
}
