using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_NextStageController : MonoBehaviour
{
    [SerializeField] private Transform Door_inner; //���� �� �ݶ��̴�
    [SerializeField] private Object_DoorController Door_outer; //���� �� ���� ������ ���� �ݶ��̴�

    private Object_KeyController key;


    private GameObject[] allPlyersInGame;                   // 24 08 17 ����� : ���ӿ� ���� ��ü �÷��̾� �� => gamemanager�� ���� ���� ���� : Ȯ���ƴ�
    public bool IsStageClear { get; private set; }

    private void Awake()
    {
        Door_inner = GetComponent<Transform>();
        Door_outer = FindObjectOfType<Object_DoorController>();

        key = FindObjectOfType<Object_KeyController>();

        allPlyersInGame = GameObject.FindGameObjectsWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Door_outer.transform.localScale == Vector3.zero && Door_inner.transform.localScale == Vector3.one)
            {
                //�������� �Ѿ�� ����
                
            }
        }
    }

    private void Update() {
        if (Object_DoorController.IsDoorOpen) {
            if (CheckAllPlayersInTheDoor()) {
                // �������� ����â���� �Ѿ���� + ����
                //TODO: [�����] �������� �Ѿ���� + ���� �ʿ���
                //SceneManager.LoadScene("Feat_NetworkGamePlay");
                //Save.instance.MakeSave();
                Debug.LogWarning(" �������� Ŭ���� ");
                IsStageClear = true;
            }
        }
    }

    private bool CheckAllPlayersInTheDoor() {
        foreach (GameObject each in allPlyersInGame) {
            PlayerMove eachPlayer = each.GetComponent<PlayerMove>();
            if (!eachPlayer.IsPlayerEnterTheDoor) {
                // 24 08 17 [�����] �� ���̶� �÷��̾ �������� ���Դٴ� bool���� false�� stage �¸����� x
                return false;
            }
        }
        return true;
    }
}
