using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxPrefabController : MonoBehaviour {
    // 1. ����
    //      ����� �浹 ������Ʈ ������ �޾ƿ� �ؽ�Ʈ ����
    //      ������ ������ ������

    private Text textBoxCountText;                                                  // �߾� ī��Ʈ �ؽ�Ʈ

    private int laymask = (1 << 8);                                                 // ã�� ���̾�
    [SerializeField] private int TextBoxCountNum;                                   // �ʱ� ī��Ʈ ����
    private float boxPlayerDeltaX = 0.5f;                                           // �ڽ� �÷��̾� x�� ����
    private float boxPlayerDeltaY = 0.8f;                                           // �ڽ� �÷��̾� y�� ����



    private int isPlayerPushingBox;                                                 // �÷��̾ �ڽ��� �̴� ����

    private bool canMove;                                                           // �÷��̾ �ڽ��� �з��� �� �� ������ �� �ִ���
    private bool isStartFollowObject;                                               // box�� �÷��̾ ���󰡴� ��
    private bool isGetCollObject_Player;                                            // �÷��̾�� �ε��� 
    private bool isFisrtCollBoxWithPlayer;                                          // �÷��̾�� �ε����� ù��° �ڽ�

    private float playerDeltaXposition = 0f;                                        // �÷��̾�� ���� �Ʒ����� �ε����� ��� ���ڿ� �÷��̾��� x�� ����
    private float followingObjectDeltaTime = 0f;                                    // �÷��̾�� ���� �Ʒ����� �ε����� ��� x�� ���� �ð� 

    private Vector2 followingObjectPre = Vector2.zero;                              // �÷��̾�� ���� �Ʒ����� �ε����� ��� ��ġ �񱳰�
    private GameObject followingObject;                                             // �÷��̾�� ���� �Ʒ����� �ε����� ��� �÷��̾� Ȥ�� ���� ������Ʈ ����
    [SerializeField] private GameObject findPlayerBoxObject;                        // �÷��̾�� �� ������ �ε����ٸ� �ε��� ���� ������Ʈ 
    
    [SerializeField]private Collider2D[] hitCollisionAll;                           // �ڱ��ڽŰ� �ε��� ��� �ڽ� ����

    public bool GetCanMove() { return canMove; }                                    // �÷��̾ �� ������ �ж� box ������ �� ���� ��� true�� ����
    public int GetChangeTextCount() { return isPlayerPushingBox; }                  // �÷��̾ �� ������ �ж� �ڽ��� �̴� �÷��̾� ���� ����
    public bool GetCollObject_Player() { return isGetCollObject_Player; }           // �÷��̾�� �� ������ �ε����ٸ� true�� ����
    public bool GetStartFollowObject() { return isStartFollowObject; }              // �÷��̾�� ���� �Ʒ����� �ε����� ��� ���� ���� ���� 
    public GameObject GetFindPlayerBoxObject() { return findPlayerBoxObject; }      // �÷��̾�� �� ������ �ε����ٸ� �ε��� ���� ������Ʈ ����

    private int findPlayerInCollisionList = 0;                                      // �÷��̾�� �� ������ �ε����ٸ� �ε��� ������Ʈ �� �÷��̾� ��ü ����
    private List<GameObject> setCollisionObjectList = new List<GameObject>();

    private Rigidbody2D transformRigidbody;
    private FindCollisionObjectsNum findCollisionObjectsNum;

    private void Awake() {
        transformRigidbody = GetComponent<Rigidbody2D>();

        textBoxCountText = transform.GetComponentInChildren<Text>();

        findCollisionObjectsNum = GetComponent<FindCollisionObjectsNum>();

        InitBoxCountNumText();
    }

    // Raycast�� �ε��� ��� ��ü�� �迭�� ���� =>�ε��� �迭�� �÷��̾ �ִٸ� ��ü ����
    private void RaycastOverlapBoxAll() {
        hitCollisionAll = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.2f, 0.9f), 0, laymask);

        if (hitCollisionAll.Length > 0) {
            if (hitCollisionAll[0].gameObject == transform.gameObject) {
                hitCollisionAll = hitCollisionAll.Where(hitCollisionAll => hitCollisionAll.gameObject != transform.gameObject).ToArray();  // �ڱ� �ڽ��� ��� ���� (TextBOX)
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
    // �ڱ��ڽŰ� �ε��� �迭�� �÷��̾ �ִ��� Ȯ��
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

    // �ڱ� �ڽŰ� �ε��� �迭�� �÷��̾ ������ isGetCollObject_Player ��ü ������ �ʱ�ȭ
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

    // �ؽ�Ʈ ���� ����
    private void ChangeTextChange() {
        if(findPlayerBoxObject != null) {
            TextBoxPrefabController textBox = findPlayerBoxObject.GetComponent<TextBoxPrefabController>();
            isPlayerPushingBox = textBox.GetChangeTextCount();
            ChangeBoxCountNumText();    // �ؽ�Ʈ���ں���Ǿ����

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
                ChangeBoxCountNumText();    // �ؽ�Ʈ���ں���Ǿ����
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

    // �ش� �ڽ��� �Ʒ��鿡 collision�� �÷��̾��� ��ü ���� 
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


    // textbox�� �ٴڸ鿡 �÷��̾ �ִٸ� x��ǥ ���󰡾���
    private void FollowObjectIfBoxCanMove() {
        if (followingObject == null) return;

        if (followingObject.gameObject.CompareTag("Player")) {// �÷��̾� �ϰ��

            PlayerMove playerMove = followingObject.GetComponent<PlayerMove>();
            if (playerMove.IsMoving) {
                FollowObjectAmount(followingObject);
            }
            else {
                isStartFollowObject = false;
            }
        }
        else if (followingObject.gameObject.CompareTag("Box")) {
            // textbox�� �ٴڸ鿡 ���ڰ� �ִٸ� �÷��̾� ã�ƾ���
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


    // setCollisionObjectList tag�� �÷��̾����� Ȯ���ؼ�
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

    // �ؽ�Ʈ �ʱ�ȭ
    private void InitBoxCountNumText() {
        findPlayerInCollisionList = 0;
        textBoxCountText.text = TextBoxCountNum.ToString();
    }

    // ���� �ڽ��� �а� �ִ� �÷��̾� ī��Ʈ�� �����Ͽ� �ؽ�Ʈ�� ����
    private void ChangeBoxCountNumText() {
        int changeTextCount = TextBoxCountNum - isPlayerPushingBox;
        if (changeTextCount <= 0) {
            changeTextCount = 0;
        }
        textBoxCountText.text = changeTextCount.ToString();
    }

    //�ʿ��� ī��Ʈ�� ���� �ڽ��� �а� �ִ� �÷��̾� ī��Ʈ�� �´��� Ȯ��
    private bool CheckCollisionCountNum() {
        if (isPlayerPushingBox >= TextBoxCountNum) return true;
        else return false;
    }


    //��ü �ο� �� �ڽ��� ���� ���󿡼� �а� �ִ� �ο���
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

    //������ ���� �޼ҵ�
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
    1. ����
          ����� �浹 ������Ʈ ������ �޾ƿ� �ؽ�Ʈ ����
          ������ ������ ������
    2.  ����
        2-1. ����   
            1) �߾� ī��Ʈ �ؽ�Ʈ
            2) �ʱ� ī��Ʈ
            3) ������ ���� ����
        2-2. ó�� ����
            // -> update : CheckCountNum() ���� Ȯ���ϰ� ������ ���� ���� ����
            1) FindCollisionObjectsNum ��ũ��Ʈ�� �޾ƿͼ� �ؽ�Ʈ�� ���� 
                1-1) list���� �÷��̾� tag�� ���� Ȯ��
                1-2) �ش� ���ڰ� 0�̻� �� ��� �ж��� ���� ���� �ؽ�Ʈ ����
            2) �ʿ��� ī��Ʈ�� �ش� ���ڰ� �´��� Ȯ��
            3) ��ü �ο��� ���� �������� �а� �ִ��� Ȯ��
            4) ������ ���� �޼ҵ�
            5) //TODO: [�����] Textbox�� �������� ��� ���� �̻�����
            6) //TODO: [�����] Textbox�� �÷��̾� ���� ���� ��� -> x��ǥ �̵��ؾ��� 
 */
