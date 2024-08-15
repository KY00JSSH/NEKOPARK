using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxPrefabController : MonoBehaviour {
    // 1. 목적
    //      검출된 충돌 오브젝트 개수를 받아와 텍스트 변경
    //      개수가 맞으면 움직임

    private Text textBoxCountText;                                                  // 중앙 카운트 텍스트

    private int laymask = (1 << 8);                                                 // 찾는 레이어
    [SerializeField] private int TextBoxCountNum;                                   // 초기 카운트 지정
    private float boxPlayerDeltaX = 0.5f;                                           // 박스 플레이어 x축 차이
    private float boxPlayerDeltaY = 0.8f;                                           // 박스 플레이어 y축 차이



    private int isPlayerPushingBox;                                                 // 플레이어가 박스를 미는 숫자

    private bool canMove;                                                           // 플레이어가 박스를 밀려고 할 때 움직일 수 있는지
    private bool isStartFollowObject;                                               // box가 플레이어를 따라가는 중
    private bool isGetCollObject_Player;                                            // 플레이어와 부딪힘 
    private bool isFisrtCollBoxWithPlayer;                                          // 플레이어와 부딪히는 첫번째 박스

    private float playerDeltaXposition = 0f;                                        // 플레이어와 상자 아래에서 부딪혔을 경우 상자와 플레이어의 x값 차이
    private float followingObjectDeltaTime = 0f;                                    // 플레이어와 상자 아래에서 부딪혔을 경우 x값 보정 시간 

    private Vector2 followingObjectPre = Vector2.zero;                              // 플레이어와 상자 아래에서 부딪혔을 경우 위치 비교값
    private GameObject followingObject;                                             // 플레이어와 상자 아래에서 부딪혔을 경우 플레이어 혹은 상자 오브젝트 저장
    [SerializeField] private GameObject findPlayerBoxObject;                        // 플레이어와 양 옆으로 부딪혔다면 부딪힌 상자 오브젝트 
    
    [SerializeField]private Collider2D[] hitCollisionAll;                           // 자기자신과 부딪힌 모든 박스 저장

    public bool GetCanMove() { return canMove; }                                    // 플레이어가 양 옆으로 밀때 box 움직일 수 있을 경우 true값 전달
    public int GetChangeTextCount() { return isPlayerPushingBox; }                  // 플레이어가 양 옆으로 밀때 박스를 미는 플레이어 숫자 전달
    public bool GetCollObject_Player() { return isGetCollObject_Player; }           // 플레이어와 양 옆으로 부딪혔다면 true값 전달
    public bool GetStartFollowObject() { return isStartFollowObject; }              // 플레이어와 상자 아래에서 부딪혔을 경우 따라감 여부 전달 
    public GameObject GetFindPlayerBoxObject() { return findPlayerBoxObject; }      // 플레이어와 양 옆으로 부딪혔다면 부딪힌 상자 오브젝트 전달

    private int findPlayerInCollisionList = 0;                                      // 플레이어와 양 옆으로 부딪혔다면 부딪힌 오브젝트 중 플레이어 전체 숫자
    private List<GameObject> setCollisionObjectList = new List<GameObject>();

    private Rigidbody2D transformRigidbody;
    private FindCollisionObjectsNum findCollisionObjectsNum;

    private void Awake() {
        transformRigidbody = GetComponent<Rigidbody2D>();

        textBoxCountText = transform.GetComponentInChildren<Text>();

        findCollisionObjectsNum = GetComponent<FindCollisionObjectsNum>();

        InitBoxCountNumText();
    }

    // Raycast로 부딪힌 모든 객체를 배열에 담음 =>부딪힌 배열에 플레이어가 있다면 객체 담음
    private void RaycastOverlapBoxAll() {
        hitCollisionAll = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.2f, 0.9f), 0, laymask);

        if (hitCollisionAll.Length > 0) {
            if (hitCollisionAll[0].gameObject == transform.gameObject) {
                hitCollisionAll = hitCollisionAll.Where(hitCollisionAll => hitCollisionAll.gameObject != transform.gameObject).ToArray();  // 자기 자신일 경우 제외 (TextBOX)
            }
        }

        if (IsRayCastHavePlayer()) {
            findPlayerBoxObject = gameObject;
            isGetCollObject_Player = true;
            isFisrtCollBoxWithPlayer = true;
        }
        else {
            for (int i = 0; i < hitCollisionAll.Length; i++) {
                TextBoxPrefabController textBox = hitCollisionAll[i].GetComponent<TextBoxPrefabController>();
                if (textBox.GetCollObject_Player()) {
                    Debug.LogWarning(gameObject.name + " | " + hitCollisionAll[i].name + " | " + textBox.GetCollObject_Player() + " | " + textBox.GetCanMove());

                    canMove = textBox.GetCanMove();
                    findPlayerBoxObject = textBox.GetFindPlayerBoxObject();
                    isGetCollObject_Player = textBox.GetCollObject_Player();
                }
            }
        }

    }
    // 자기자신과 부딪힌 배열에 플레이어가 있는지 확인
    private bool IsRayCastHavePlayer() {
        if (hitCollisionAll != null) {
            for (int i = 0; i < hitCollisionAll.Length; i++) {
                if (hitCollisionAll[i].gameObject.CompareTag("Player")) {
                    return true;
                }
            }
        }
        return false;

    }

    // 자기 자신과 부딪힌 배열에 플레이어가 없으면 isGetCollObject_Player 객체 삭제와 초기화
    private void DeleteFindPlayerBoxObjext() {
        if (!IsRayCastHavePlayer()) {
            if (findPlayerBoxObject != null) {
                TextBoxPrefabController textBox = findPlayerBoxObject.GetComponent<TextBoxPrefabController>();
                if (textBox.GetFindPlayerBoxObject() == null) {

                    findPlayerBoxObject = null;
                    isGetCollObject_Player = false;
                    InitBoxCountNumText();
                }
            }
        }
    }

    // 텍스트 숫자 변경
    private void ChangeTextChange() {
        if(findPlayerBoxObject != null) {
            TextBoxPrefabController textBox = findPlayerBoxObject.GetComponent<TextBoxPrefabController>();
            isPlayerPushingBox = textBox.GetChangeTextCount();
            ChangeBoxCountNumText();    // 텍스트숫자변경되어야함

        }
        else {

            findPlayerBoxObject = null;

            isFisrtCollBoxWithPlayer = false;            
            isGetCollObject_Player = false;

            canMove = false;

            InitBoxCountNumText();
        }
    }


    private void Update() {

        RaycastOverlapBoxAll();
        TextBoxMainControll();

        DeleteFindPlayerBoxObjext();


        FreezeTransformRigidbody(canMove);
        FollowObjectIfBoxCanMove();
    }

    private void TextBoxMainControll() {
        if (isFisrtCollBoxWithPlayer) {
            FindCollisionObjectTag();

            if (findPlayerInCollisionList >= 1) {
                CountAllCollisionPushSameDirection();
                ChangeBoxCountNumText();    // 텍스트숫자변경되어야함
                if (CheckCollisionCountNum()) {
                    canMove = true;
                }
                else {
                    canMove = false;
                }
            }
            else {
                InitBoxCountNumText();
                canMove = false;
            }
        }
        else {
            ChangeTextChange();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) {

        boxPlayerDeltaX = 0.5f;
        boxPlayerDeltaY = 0.8f;

        if ((transformRigidbody.position.y - collision.transform.position.y)  >= boxPlayerDeltaY){
            if ( Mathf.Abs( collision.transform.position.x - transformRigidbody.position.x )<= boxPlayerDeltaX) {
                if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
                    followingObjectPre.x = collision.gameObject.transform.position.x;
                }
            }
        }
    }

    // 해당 박스의 아래면에 collision가 플레이어라면 객체 저장 
    private void OnCollisionStay2D(Collision2D collision) {
        if ((transformRigidbody.position.y - collision.transform.position.y) >= boxPlayerDeltaY) {
            if (Mathf.Abs(collision.transform.position.x - transformRigidbody.position.x) <= boxPlayerDeltaX) {
                if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
                    followingObject = collision.gameObject;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
            canMove = false;
            followingObject = null;
            isGetCollObject_Player = false;
            findPlayerBoxObject = null; 
        }
    }


    // textbox의 바닥면에 플레이어가 있다면 x좌표 따라가야함
    private void FollowObjectIfBoxCanMove() {
        if (followingObject == null) return;

        if (followingObject.gameObject.CompareTag("Player")) {// 플레이어 일경우

            PlayerMove playerMove = followingObject.GetComponent<PlayerMove>();
            if (playerMove.IsMoving) {
                FollowObjectAmount(followingObject);
            }
            else {
                isStartFollowObject = false;
            }
        }
        else if (followingObject.gameObject.CompareTag("Box")) {
            // textbox의 바닥면에 상자가 있다면 플레이어 찾아야함
            TextBoxPrefabController boxMove = followingObject.GetComponent<TextBoxPrefabController>();
            if (boxMove.GetStartFollowObject()) {
                FollowObjectAmount(followingObject);
            }
        }
        else {
            isStartFollowObject = false;
        }
    }

    private void FollowObjectAmount(GameObject _followingObject) {

        Rigidbody2D followingObjectRigid = _followingObject.GetComponent<Rigidbody2D>();
        float startDeltaXposition = Mathf.Abs(followingObjectRigid.position.x - transformRigidbody.position.x);

        if (startDeltaXposition >= 3f) return;

        followingObjectDeltaTime += Time.deltaTime;

        if (followingObjectDeltaTime >= .005f) {
            followingObjectDeltaTime = 0f;
            playerDeltaXposition = followingObjectRigid.position.x - followingObjectPre.x;
            //Debug.LogWarning(playerDeltaXposition);
            if (playerDeltaXposition >= -3 || playerDeltaXposition <= 3) {
                transformRigidbody.position = new Vector2(transformRigidbody.position.x + playerDeltaXposition, transformRigidbody.position.y);
                isStartFollowObject = true;
            }
            else {
                isStartFollowObject = false;
            }
            followingObjectPre.x = followingObjectRigid.position.x;
        }
    }


    // setCollisionObjectList tag로 플레이어인지 확인해서
    private void FindCollisionObjectTag() {
        findPlayerInCollisionList = 0;
        setCollisionObjectList.Clear();
        //Debug.Log("FindCollisionObjectTag | "+findCollisionObjectsNum.GetCollObjectsList().Count + " | " + gameObject.name);

        for (int i = 0; i < findCollisionObjectsNum.GetCollObjectsList().Count; i++) {
            if (findCollisionObjectsNum.GetCollObjectsList()[i].CompareTag("Player")) {
                setCollisionObjectList.Add(findCollisionObjectsNum.GetCollObjectsList()[i]);
                findPlayerInCollisionList++;
                //Debug.LogWarning(findPlayerInCollisionList);
            }
        }
    }

    // 텍스트 초기화
    private void InitBoxCountNumText() {
        findPlayerInCollisionList = 0;
        textBoxCountText.text = TextBoxCountNum.ToString();
    }

    // 현재 박스를 밀고 있는 플레이어 카운트를 제외하여 텍스트를 변경
    private void ChangeBoxCountNumText() {
        int changeTextCount = TextBoxCountNum - isPlayerPushingBox;
        if (changeTextCount <= 0) {
            changeTextCount = 0;
        }
        textBoxCountText.text = changeTextCount.ToString();
    }

    //필요한 카운트와 현재 박스를 밀고 있는 플레이어 카운트가 맞는지 확인
    private bool CheckCollisionCountNum() {
        if (isPlayerPushingBox >= TextBoxCountNum) return true;
        else return false;
    }


    //전체 인원 중 박스와 같은 선상에서 밀고 있는 인원수
    private void CountAllCollisionPushSameDirection() {
        isPlayerPushingBox = 0;
        for (int i = 0; i < setCollisionObjectList.Count; i++) {
            PlayerMove _playerMove = setCollisionObjectList[i].GetComponent<PlayerMove>();
            Rigidbody2D followingObjectRigid = _playerMove.GetComponent<Rigidbody2D>();

            float deltaYPosition = Mathf.Abs(transformRigidbody.position.y - followingObjectRigid.position.y);
            if (_playerMove.IsMoving && deltaYPosition <= 1) {
                isPlayerPushingBox++;
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
            5) //TODO: [김수주] Textbox가 겹쳐있을 경우 판정 이상해짐
            6) //TODO: [김수주] Textbox가 플레이어 위에 있을 경우 -> x좌표 이동해야함 
 */
