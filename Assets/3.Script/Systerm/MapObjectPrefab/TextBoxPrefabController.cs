using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxPrefabController : MonoBehaviour {
    // 1. ����
    //      ����� �浹 ������Ʈ ������ �޾ƿ� �ؽ�Ʈ ����
    //      ������ ������ ������

    [SerializeField] private int TextBoxCountNum;   // �ʱ� ī��Ʈ ����
    private int findPlayerInCollisionList =0;

    private bool canMove;   // box ������ ����

    private Text textBoxCountText; // �߾� ī��Ʈ �ؽ�Ʈ

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

        ChangeBoxCountNumText();    // �ؽ�Ʈ���ں���Ǿ����
        if (findPlayerInCollisionList >= 1) {

            if (TextBoxCountNum == 1) canMove = true;
            else {
                // �ڽ� ������ ����
                if (CheckCollisionCountNum()) {
                    if (CheckAllCollisionPushSameDirection()) canMove = true;// �ڽ� ������ ����                
                    else canMove = false;
                }
                else canMove = false;
            }

        }
        else canMove = false;

        FreezeTransformRigidbody(canMove);
    }

    // �������� �����ϸ� textbox�� collision�� �÷��̾��� ��ü ����
    private void OnCollisionStay2D(Collision2D collision) {
        if (canMove) {
            if(collision.gameObject.CompareTag("Player") && collision.transform.position.y <= transform.position.y) {
                // textbox�� x��ǥ�� �̵��Ǿ����
                followingObject = collision.gameObject;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        followingObject = null;
    }
    // �������� �����ϸ� textbox�� �ٴڸ鿡 �÷��̾ �ִٸ� x��ǥ ���󰡾���
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

    // setCollisionObjectList tag�� �÷��̾����� Ȯ���ؼ�
    private void FindCollisionObjectTag() {
        findPlayerInCollisionList = 0;
        setCollisionObjectList = findCollisionObjectsNum.GetCollObjectsList();
        for (int i = 0; i < setCollisionObjectList.Count; i++) {
            if (setCollisionObjectList[i].CompareTag("Player")) {
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

    //��ü �ο��� ���� �������� �а� �ִ��� Ȯ��
    private bool CheckAllCollisionPushSameDirection() {
        for (int i = 0; 0 < setCollisionObjectList.Count-1; i++) {            
            PlayerMove _playerMove = setCollisionObjectList[i].GetComponent<PlayerMove>();
            PlayerMove playerMove = setCollisionObjectList[i+1].GetComponent<PlayerMove>();

            if (_playerMove.isMovingRight != playerMove.isMovingRight) return false;
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
                1-2) �ش� ���ڰ� 0�̻� �� ��� �ؽ�Ʈ ����
            2) �ʿ��� ī��Ʈ�� �ش� ���ڰ� �´��� Ȯ��
            3) ��ü �ο��� ���� �������� �а� �ִ��� Ȯ��
            4) ������ ���� �޼ҵ�
            5) //TODO: Textbox�� �������� ��� ���� �̻�����
            6) //TODO: Textbox�� �÷��̾� ���� ���� ��� -> x��ǥ �̵��ؾ���
 */
