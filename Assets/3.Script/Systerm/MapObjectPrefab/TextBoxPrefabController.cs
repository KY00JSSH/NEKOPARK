using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxPrefabController : MonoBehaviour {
    // 1. 목적
    //      검출된 충돌 오브젝트 개수를 받아와 텍스트 변경
    //      개수가 맞으면 움직임

    [SerializeField] private int TextBoxCountNum;   // 초기 카운트 지정
    private int findPlayerInCollisionList =0;

    private bool canMove;   // box 움직임 여부

    private Text textBoxCountText; // 중앙 카운트 텍스트

    private float followingObjectDeltaTime = 0f;
    private Vector2 followingObjectPre = new Vector2();
    private GameObject followingObject;
    private List<GameObject> setCollisionObjectList = new List<GameObject>();

    private Rigidbody2D transformRigidbody;
    private FindCollisionObjectsNum findCollisionObjectsNum;

    private void Awake() {
        transformRigidbody = GetComponent<Rigidbody2D>();

        textBoxCountText = transform.GetComponentInChildren<Text>();

        findCollisionObjectsNum = GetComponent<FindCollisionObjectsNum>();

        InitBoxCountNumText();
    }

    private void Update() {
        FindCollisionObjectTag();

        ChangeBoxCountNumText();    // 텍스트숫자변경되어야함
        if (findPlayerInCollisionList >= 1) {

            if (TextBoxCountNum == 1) canMove = true;
            else {
                // 박스 움직임 수정
                if (CheckCollisionCountNum()) {
                    if (CheckAllCollisionPushSameDirection()) canMove = true;// 박스 움직임 가능                
                    else canMove = false;
                }
                else canMove = false;
            }

        }
        else canMove = false;

        FreezeTransformRigidbody(canMove);
    }

    // 음직임이 가능하며 textbox의 collision가 플레이어라면 객체 저장
    private void OnCollisionStay2D(Collision2D collision) {
        if (canMove) {
            if(collision.gameObject.CompareTag("Player") && collision.transform.position.y <= transform.position.y) {
                // textbox의 x좌표는 이동되어야함
                followingObject = collision.gameObject;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        followingObject = null;
    }
    // 음직임이 가능하며 textbox의 바닥면에 플레이어가 있다면 x좌표 따라가야함
    private void FollowObjectIfBoxCanMove(bool _canMove) {

        if (_canMove) {
            followingObjectDeltaTime += Time.deltaTime;
            if (followingObject != null) {
                if (followingObjectDeltaTime >= 2f) {
                    followingObjectDeltaTime = 0f;
                    float playerDeltaXposition = followingObject.transform.position.x - followingObjectPre.x;
                    transform.position = new Vector2(transform.position.x + playerDeltaXposition, transform.position.y);
                    followingObjectPre.x = followingObject.transform.position.x;
                    
                }
            }
        }
    }

    // setCollisionObjectList tag로 플레이어인지 확인해서
    private void FindCollisionObjectTag() {
        findPlayerInCollisionList = 0;
        setCollisionObjectList = findCollisionObjectsNum.GetCollObjectsList();
        for (int i = 0; i < setCollisionObjectList.Count; i++) {
            if (setCollisionObjectList[i].CompareTag("Player")) {
                findPlayerInCollisionList++;
            }
        }
    }

    // 텍스트 초기화
    private void InitBoxCountNumText() {
        findPlayerInCollisionList = 0;
        textBoxCountText.text = TextBoxCountNum.ToString();
    }

    //FindCollisionObjectsNum 스크립트를 받아와서 텍스트를 변경
    private void ChangeBoxCountNumText() {
        int changeTextCount = TextBoxCountNum - findPlayerInCollisionList;
        textBoxCountText.text = changeTextCount.ToString();
    }

    //필요한 카운트와 현재 부딪힌 플레이어 카운트가 맞는지 확인
    private bool CheckCollisionCountNum() {
        if (findPlayerInCollisionList >= TextBoxCountNum) return true;
        else return false;
    }

    //전체 인원이 같은 방향으로 밀고 있는지 확인
    private bool CheckAllCollisionPushSameDirection() {
        for (int i = 0; 0 < setCollisionObjectList.Count-1; i++) {            
            PlayerMove _playerMove = setCollisionObjectList[i].GetComponent<PlayerMove>();
            PlayerMove playerMove = setCollisionObjectList[i+1].GetComponent<PlayerMove>();

            if (_playerMove.isMovingRight != playerMove.isMovingRight) return false;
        }
        return true;
    }

    //움직임 고정 메소드
    private void FreezeTransformRigidbody(bool _canMove) {
        if (_canMove) {
            transformRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else {
            transformRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}

/*
    1. 목적
          검출된 충돌 오브젝트 개수를 받아와 텍스트 변경
          개수가 맞으면 움직임
    2.  내용
        2-1. 변수   
            1) 중앙 카운트 텍스트
            2) 초기 카운트
            3) 움직임 가능 여부
        2-2. 처리 내용
            // -> update : CheckCountNum() 지속 확인하고 움직임 고정 여부 변경
            1) FindCollisionObjectsNum 스크립트를 받아와서 텍스트를 변경 
                1-1) list에서 플레이어 tag만 수량 확인
                1-2) 해당 숫자가 0이상 일 경우 텍스트 변경
            2) 필요한 카운트와 해당 숫자가 맞는지 확인
            3) 전체 인원이 같은 방향으로 밀고 있는지 확인
            4) 움직임 고정 메소드
            5) //TODO: Textbox가 겹쳐있을 경우 판정 이상해짐
            6) //TODO: Textbox가 플레이어 위에 있을 경우 -> x좌표 이동해야함
 */
