using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxPrefabController_ : MonoBehaviour {
    // 1. 목적
    //      검출된 충돌 오브젝트 개수를 받아와 텍스트 변경
    //      개수가 맞으면 움직임
    private Text textBoxCountText; // 중앙 카운트 텍스트

    [SerializeField] private int TextBoxCountNum;   // 초기 카운트 지정
    private int isPushingMemberCount;

    private bool canMove;   // 플레이어가 양 옆으로 밀때 box 움직임 여부
    private bool isGetFollowObject;   // box 움직임 여부

    private bool isStartFollowObject;   // box가 플레이어를 따라가는 중
    public bool GetStartFollowObject() { return isStartFollowObject; }


    private float playerDeltaXposition = 0f;                // 상자와 플레이어의 x값 차이
    private float followingObjectDeltaTime = 0f;             // x값 보정 시간 

    private Vector2 followingObjectPre = Vector2.zero;


    private GameObject followingObject;                     // 플레이어 저장

    private Rigidbody2D transformRigidbody;
    private FindCollisionPlayer findCollisionPlayer;

    private void Awake() {
        transformRigidbody = GetComponent<Rigidbody2D>();

        textBoxCountText = transform.GetComponentInChildren<Text>();

        findCollisionPlayer = GetComponent<FindCollisionPlayer>();

        InitBoxCountNumText();
    }

    private void Update() {
        if (findCollisionPlayer.GetFoundPlayerCount().Count >= 1) {
            CheckAllCollisionPushSameDirection();       // 밀고 있는 사람 수 확인
            ChangeBoxCountNumText();                     //  밀고 있는 사람만큼 텍스트숫자변경되어야함

            if (CheckCollisionCountNum()) {                 // 밀고 있는 사람과 초기 카운트를 비교
                canMove = true;
            }
            else {
                canMove = false;
            }
        }
        else {
            canMove = false;
            InitBoxCountNumText();
        }

        FreezeTransformRigidbody(canMove);
        FollowObjectIfBoxCanMove();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.position.y <= transform.position.y) {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
                followingObjectPre.x = collision.gameObject.transform.position.x;
            }
        }
    }
    // 음직임이 가능하며 textbox의 collision가 플레이어라면 객체 저장
    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.position.y <= transform.position.y) {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
                // textbox의 x좌표는 이동되어야함
                isGetFollowObject = true;
                followingObject = collision.gameObject;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        followingObject = null;
        isGetFollowObject = false;
    }


    // textbox의 바닥면에 플레이어가 있다면 x좌표 따라가야함
    private void FollowObjectIfBoxCanMove() {
        if (isGetFollowObject) {

            if (followingObject.CompareTag("Player")) { // 플레이어 일경우
                PlayerMove playerMove = followingObject.GetComponent<PlayerMove>();
                if (playerMove.IsMoving) {
                    FollowObjectAmount(followingObject);
                }
                else {
                    isStartFollowObject = false;
                }

            }
            else {
                // textbox의 바닥면에 상자가 있다면 플레이어 찾아야함
                TextBoxPrefabController_ boxMove = followingObject.GetComponent<TextBoxPrefabController_>();
                if (boxMove.GetStartFollowObject()) {
                    FollowObjectAmount(followingObject);
                }
            }
        }
        else {
            isStartFollowObject = false;
        }
    }

    private void FollowObjectAmount(GameObject _followingObject) {

        followingObjectDeltaTime += Time.deltaTime;

        if (followingObjectDeltaTime >= .005f) {
            followingObjectDeltaTime = 0f;
            playerDeltaXposition = _followingObject.transform.position.x - followingObjectPre.x;
            //Debug.LogWarning(playerDeltaXposition);
            if (playerDeltaXposition >= -3 || playerDeltaXposition <= 3) {
                transform.position = new Vector2(transform.position.x + playerDeltaXposition, transform.position.y);
                isStartFollowObject = true;
            }
            followingObjectPre.x = _followingObject.transform.position.x;
        }
    }

    // 텍스트 초기화
    private void InitBoxCountNumText() {
        textBoxCountText.text = TextBoxCountNum.ToString();
    }

    //FindCollisionObjectsNum 스크립트를 받아와서 텍스트를 변경
    private void ChangeBoxCountNumText() {
        int changeTextCount = TextBoxCountNum - isPushingMemberCount;
        textBoxCountText.text = changeTextCount.ToString();
    }

    //필요한 카운트와 현재 부딪혀서 밀고 있는 플레이어 카운트가 맞는지 확인
    private bool CheckCollisionCountNum() {
        if (isPushingMemberCount >= TextBoxCountNum) return true;
        else return false;
    }

    /*
    // 1명일 경우 같은 방향으로 밀고 있는지 확인
    private bool CheckCollisionPushSameDirection() {
        PlayerMove _playerMove = setCollisionObjectList[0].GetComponent<PlayerMove>();
        float deltaYPosition = Mathf.Abs(transform.position.y - setCollisionObjectList[0].transform.position.y);
        if (_playerMove.IsMoving && deltaYPosition <= 1) return true;
        return false;
    }
    */

    //전체 인원 중 첫번째 플레이어부터 밀고있는 사람의 수
    private void CheckAllCollisionPushSameDirection() {
        isPushingMemberCount = 0;
        for (int i = 0; i < findCollisionPlayer.GetFoundPlayerCount().Count; i++) {
            PlayerMove _playerMove = findCollisionPlayer.GetFoundPlayerCount()[i].GetComponent<PlayerMove>();

            if (_playerMove.IsMoving) {
                isPushingMemberCount++;
            }
            else {
                break;
            }
        }
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
                1-2) 해당 숫자가 0이상 일 경우 밀때만 숫자 감소 텍스트 변경
            2) 필요한 카운트와 해당 숫자가 맞는지 확인
            3) 전체 인원이 같은 방향으로 밀고 있는지 확인
            4) 움직임 고정 메소드
            5) //TODO: Textbox가 겹쳐있을 경우 판정 이상해짐
            6) //TODO: Textbox가 플레이어 위에 있을 경우 -> x좌표 이동해야함 
 */
