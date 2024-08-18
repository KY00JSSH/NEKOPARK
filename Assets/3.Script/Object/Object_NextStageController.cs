using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_NextStageController : MonoBehaviour
{
    [SerializeField] private Transform Door_inner; //갈색 문 콜라이더
    [SerializeField] private Object_DoorController Door_outer; //문을 연 후의 검은색 공간 콜라이더

    private Object_KeyController key;


    private GameObject[] allPlyersInGame;                   // 24 08 17 김수주 : 게임에 들어온 전체 플레이어 수 => gamemanager에 넣을 수도 있음 : 확정아님
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
                //스테이지 넘어가는 로직
                
            }
        }
    }

    private void Update() {
        if (Object_DoorController.IsDoorOpen) {
            if (CheckAllPlayersInTheDoor()) {
                // 스테이지 선택창으로 넘어가야함 + 저장
                //TODO: [김수주] 스테이지 넘어가야함 + 저장 필요함
                //SceneManager.LoadScene("Feat_NetworkGamePlay");
                //Save.instance.MakeSave();
                Debug.LogWarning(" 스테이지 클리어 ");
                IsStageClear = true;
            }
        }
    }

    private bool CheckAllPlayersInTheDoor() {
        foreach (GameObject each in allPlyersInGame) {
            PlayerMove eachPlayer = each.GetComponent<PlayerMove>();
            if (!eachPlayer.IsPlayerEnterTheDoor) {
                // 24 08 17 [김수주] 한 명이라도 플레이어가 문안으로 들어왔다는 bool값이 false면 stage 승리조건 x
                return false;
            }
        }
        return true;
    }
}
