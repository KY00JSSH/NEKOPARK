using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxPrefabController : MonoBehaviour {
    // 1. ����
    //      ����� �浹 ������Ʈ ������ �޾ƿ� �ؽ�Ʈ ����
    //      ������ ������ ������
    private Text textBoxCountText; // �߾� ī��Ʈ �ؽ�Ʈ

    [SerializeField] private int TextBoxCountNum;   // �ʱ� ī��Ʈ ����

    private bool canMove;   // �÷��̾ �� ������ �ж� box ������ ����
    private bool isGetFollowObject;   // box ������ ����

    private bool isStartFollowObject;   // box�� �÷��̾ ���󰡴� ��
    public bool GetStartFollowObject() { return isStartFollowObject; }


    private float playerDeltaXposition = 0f;                // ���ڿ� �÷��̾��� x�� ����
    private float followingObjectDeltaTime = 0f;             // x�� ���� �ð� 

    private Vector2 followingObjectPre = Vector2.zero;
    
    
    private GameObject followingObject;                     // �÷��̾� ����

    private int findPlayerInCollisionList = 0;
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

        if (findPlayerInCollisionList >= 1) {
            if (TextBoxCountNum == 1) {
                if (CheckCollisionPushSameDirection()) {
                    canMove = true;
                    ChangeBoxCountNumText();    // �ؽ�Ʈ���ں���Ǿ����
                }
                else {
                    canMove = false;
                    //InitBoxCountNumText();
                }
            }
            else {
                // �ڽ� ������ ����
                if (CheckCollisionCountNum()) {
                    if (CheckAllCollisionPushSameDirection()) {
                        canMove = true;// �ڽ� ������ ����
                        ChangeBoxCountNumText();    // �ؽ�Ʈ���ں���Ǿ����
                    }
                    else {
                        canMove = false;
                        //InitBoxCountNumText();
                    }
                }
                else {
                    canMove = false;
                    //InitBoxCountNumText();
                }
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
    // �������� �����ϸ� textbox�� collision�� �÷��̾��� ��ü ����
    private void OnCollisionStay2D(Collision2D collision) {
        if ( collision.transform.position.y <= transform.position.y) {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
                // textbox�� x��ǥ�� �̵��Ǿ����
                isGetFollowObject = true;
                followingObject = collision.gameObject;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        followingObject = null;
        isGetFollowObject = false;
    }


    // textbox�� �ٴڸ鿡 �÷��̾ �ִٸ� x��ǥ ���󰡾���
    private void FollowObjectIfBoxCanMove() {
        if (isGetFollowObject) {

            if (followingObject.CompareTag("Player")) { // �÷��̾� �ϰ��
                PlayerMove playerMove = followingObject.GetComponent<PlayerMove>();
                if (playerMove.isMoving) {
                    FollowObjectAmount(followingObject);
                }
                else {
                    isStartFollowObject = false;
                }

            }
            else {
                // textbox�� �ٴڸ鿡 ���ڰ� �ִٸ� �÷��̾� ã�ƾ���
                TextBoxPrefabController boxMove = followingObject.GetComponent<TextBoxPrefabController>();
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
            playerDeltaXposition =  _followingObject.transform.position.x - followingObjectPre.x;
            //Debug.LogWarning(playerDeltaXposition);
            if (playerDeltaXposition >= -3 || playerDeltaXposition <= 3 ) {
                transform.position = new Vector2(transform.position.x + playerDeltaXposition, transform.position.y);
                isStartFollowObject = true;
            }
            followingObjectPre.x = _followingObject.transform.position.x;
        }
    }




    // setCollisionObjectList tag�� �÷��̾����� Ȯ���ؼ�
    private void FindCollisionObjectTag() {
        findPlayerInCollisionList = 0;
        setCollisionObjectList.Clear();
        for (int i = 0; i < findCollisionObjectsNum.GetCollObjectsList().Count; i++) {
            if (findCollisionObjectsNum.GetCollObjectsList()[i].CompareTag("Player")) {
                setCollisionObjectList.Add(findCollisionObjectsNum.GetCollObjectsList()[i]);
                findPlayerInCollisionList++;
            }
        }
    }

    // �ؽ�Ʈ �ʱ�ȭ
    private void InitBoxCountNumText() {
        findPlayerInCollisionList = 0;
        textBoxCountText.text = TextBoxCountNum.ToString();
    }

    //FindCollisionObjectsNum ��ũ��Ʈ�� �޾ƿͼ� �ؽ�Ʈ�� ����
    private void ChangeBoxCountNumText() {
        int changeTextCount = TextBoxCountNum - findPlayerInCollisionList;
        textBoxCountText.text = changeTextCount.ToString();
    }

    //�ʿ��� ī��Ʈ�� ���� �ε��� �÷��̾� ī��Ʈ�� �´��� Ȯ��
    private bool CheckCollisionCountNum() {
        if (findPlayerInCollisionList >= TextBoxCountNum) return true;
        else return false;
    }

    // 1���� ��� ���� �������� �а� �ִ��� Ȯ��
    private bool CheckCollisionPushSameDirection() {
        PlayerMove _playerMove = setCollisionObjectList[0].GetComponent<PlayerMove>();
        float deltaYPosition = Mathf.Abs(transform.position.y - setCollisionObjectList[0].transform.position.y);
        if (_playerMove.isMoving && deltaYPosition <= 1) return true;
        return false;
    }
    //��ü �ο��� ���� �������� �а� �ִ��� Ȯ��
    private bool CheckAllCollisionPushSameDirection() {
        for (int i = 0; 0 < setCollisionObjectList.Count - 1; i++) {
            PlayerMove _playerMove = setCollisionObjectList[i].GetComponent<PlayerMove>();
            PlayerMove playerMove = setCollisionObjectList[i + 1].GetComponent<PlayerMove>();

            if (_playerMove.isMoving != playerMove.isMoving) return false;
        }
        return true;
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
            5) //TODO: Textbox�� �������� ��� ���� �̻�����
            6) //TODO: Textbox�� �÷��̾� ���� ���� ��� -> x��ǥ �̵��ؾ��� 
 */
