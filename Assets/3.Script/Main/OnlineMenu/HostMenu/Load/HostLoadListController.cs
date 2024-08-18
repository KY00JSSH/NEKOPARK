using UnityEngine;
using UnityEngine.UI;

public class HostLoadListController : MonoBehaviour {
    private Button[] listButton;

    // 24 08 18 ����� Multi List Load �߰�
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
        //TODO: ���̺� ���� ��� ��������
        multiSaveList = Save.instance.LoadMultiFiles();

        if(multiSaveList != null) {
            multiSaveList.MultiList.Reverse();          // ���� ����(�ֽŰ� ����)
            foreach (StageSaveData item in multiSaveList.MultiList) {
                Debug.Log(item);            //Todo: [�����] ����� �����
            }
        }

    }
}
