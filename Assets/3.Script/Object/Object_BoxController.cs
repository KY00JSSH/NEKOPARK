using UnityEngine;
using UnityEngine.UI;

public class Object_BoxController : MonoBehaviour {
    private RectTransform rectTransform;
    private Rigidbody2D boxRigid;
    private Text textCount;

    public bool isMoveable { get { return currentCount == 0; } }
    public bool isRightPushMe { get; private set; }
    public bool isLeftPushMe { get; private set; }
    public int rightPushCount { get; private set; }
    public int leftPushCount { get; private set; }


    // 미는데 필요한 총 카운트와 현재 카운트
    [SerializeField] private int requiredCount = 1;
    private int currentCount;

    private Vector2 lastPosition = Vector2.zero;

    // 스프링에서 밀어야한다면 위치고정 풀기위한 bool
    private bool isThisBoxNeedAddforce;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        boxRigid = GetComponent<Rigidbody2D>();

        textCount = GetComponentInChildren<Text>();
        textCount.text = requiredCount.ToString();
        currentCount = requiredCount;
    }

    private void Update() {
        SetCount();
        SetMoveAvailable();
    }

    private void SetCount() {
        // 현재 표시되는 카운트는 총 필요 카운트에서 미는 사람의 수를 뺀 값입니다.
        currentCount = requiredCount - (Mathf.Max(rightPushCount, leftPushCount));

        if (currentCount > requiredCount) currentCount = requiredCount;
        else if (currentCount < 0) currentCount = 0;
        textCount.text = currentCount.ToString();
    }

    private void SetMoveAvailable() {
        // 못움직이게 설정합니다.
        if (isMoveable)
            boxRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        else {
            if (isThisBoxNeedAddforce) {    // 박스가 움직여야할때 회전만 정지
                boxRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else {
                boxRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (isThisBoxNeedAddforce) {    // 박스가 스프링을 벗어나서 다른 충돌을 한다면 x좌표 고정
            isThisBoxNeedAddforce = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        // 위 아래 충돌은 무시합니다.
        if (collision.transform.TryGetComponent(out RectTransform collisionRect)) {
            float collisionTop = collision.transform.position.y + collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            float collisionBottom = collision.transform.position.y - collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;

            // 충돌체가 내 위에 있는 경우.
            if (collisionBottom >= transform.position.y + rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f) {
                if (lastPosition == Vector2.zero) lastPosition = (Vector2)transform.position;
                Vector2 deltaPosition = (Vector2)transform.position - lastPosition;

                //if (collision.gameObject.CompareTag("Player")) return;
                collision.transform.position += (Vector3)deltaPosition;
                lastPosition = transform.position;
                return;
            }
            // 충돌체가 내 아래에 있는 경우.
            else if (collisionTop <= transform.position.y - rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f) {
                //if (lastPosition == Vector2.zero) lastPosition = (Vector2)transform.position;
                //Vector2 deltaPosition = (Vector2)transform.position - lastPosition;

                //collision.transform.position += (Vector3)deltaPosition;
                //lastPosition = transform.position;

                if (collision.collider.CompareTag("Spring")) {
                    if (collision.transform.TryGetComponent(out SpringPrefabController springController)) {
                        if (springController.isBoxNeedAddforce ) {
                            isThisBoxNeedAddforce = true;
                        }
                    }
                }
                return;
            }


        }

        // 충돌체의 방향을 구합니다.
        Vector2 direction = Vector2.right;
        if (collision.transform.position.x > transform.position.x) direction = Vector2.right;
        else if (collision.transform.position.x < transform.position.x) direction = Vector2.left;

        // 충돌체가 나를 밀고 있는 상태인지를 가져옵니다.
        if (collision.transform.TryGetComponent(out PlayerMove player)) {
            if (direction == Vector2.right) {
                isRightPushMe = player.IsMoving && !player.IsMovingRight;       // 플레이어가 오른쪽에서 박스와 닿은 상태로 왼쪽으로 이동하는 상황
                rightPushCount = player.rightPushCount + 1;
            }
            else if (direction == Vector2.left) {
                isLeftPushMe = player.IsMoving && player.IsMovingRight;
                leftPushCount = player.leftPushCount + 1;
            }
        }
        // 박스의 경우에는 미는 사람의 수를 더하지 않습니다.
        else if (collision.transform.TryGetComponent(out Object_BoxController box)) {
            if (direction == Vector2.right) {
                isRightPushMe = box.isRightPushMe && box.isMoveable;            // 박스가 밀리고 있고, 움직일 수 있는 상황
                rightPushCount = box.rightPushCount;
            }
            else if (direction == Vector2.left) {
                isLeftPushMe = box.isLeftPushMe && box.isMoveable;
                leftPushCount = box.leftPushCount;
            }
        }
        //Debug.Log("WOW");

        // 아무도 나를 밀고있지 않다면 미는 사람의 수를 초기화합니다.
        if (!isRightPushMe && !isLeftPushMe) {
            rightPushCount = 0;
            leftPushCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isRightPushMe = isLeftPushMe = false;
        rightPushCount = 0;
        leftPushCount = 0;
    }






}

//TODO: 플레이어에게도 똑같이 적용해야 함.
