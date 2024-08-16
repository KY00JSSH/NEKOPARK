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


    // �̴µ� �ʿ��� �� ī��Ʈ�� ���� ī��Ʈ
    [SerializeField] private int requiredCount = 1;
    private int currentCount;

    private Vector2 lastPosition = Vector2.zero;

    // ���������� �о���Ѵٸ� ��ġ���� Ǯ������ bool
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
        // ���� ǥ�õǴ� ī��Ʈ�� �� �ʿ� ī��Ʈ���� �̴� ����� ���� �� ���Դϴ�.
        currentCount = requiredCount - (Mathf.Max(rightPushCount, leftPushCount));

        if (currentCount > requiredCount) currentCount = requiredCount;
        else if (currentCount < 0) currentCount = 0;
        textCount.text = currentCount.ToString();
    }

    private void SetMoveAvailable() {
        // �������̰� �����մϴ�.
        if (isMoveable)
            boxRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        else {
            if (isThisBoxNeedAddforce) {    // �ڽ��� ���������Ҷ� ȸ���� ����
                boxRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else {
                boxRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (isThisBoxNeedAddforce) {    // �ڽ��� �������� ����� �ٸ� �浹�� �Ѵٸ� x��ǥ ����
            isThisBoxNeedAddforce = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        // �� �Ʒ� �浹�� �����մϴ�.
        if (collision.transform.TryGetComponent(out RectTransform collisionRect)) {
            float collisionTop = collision.transform.position.y + collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            float collisionBottom = collision.transform.position.y - collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;

            // �浹ü�� �� ���� �ִ� ���.
            if (collisionBottom >= transform.position.y + rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f) {
                if (lastPosition == Vector2.zero) lastPosition = (Vector2)transform.position;
                Vector2 deltaPosition = (Vector2)transform.position - lastPosition;

                //if (collision.gameObject.CompareTag("Player")) return;
                collision.transform.position += (Vector3)deltaPosition;
                lastPosition = transform.position;
                return;
            }
            // �浹ü�� �� �Ʒ��� �ִ� ���.
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

        // �浹ü�� ������ ���մϴ�.
        Vector2 direction = Vector2.right;
        if (collision.transform.position.x > transform.position.x) direction = Vector2.right;
        else if (collision.transform.position.x < transform.position.x) direction = Vector2.left;

        // �浹ü�� ���� �а� �ִ� ���������� �����ɴϴ�.
        if (collision.transform.TryGetComponent(out PlayerMove player)) {
            if (direction == Vector2.right) {
                isRightPushMe = player.IsMoving && !player.IsMovingRight;       // �÷��̾ �����ʿ��� �ڽ��� ���� ���·� �������� �̵��ϴ� ��Ȳ
                rightPushCount = player.rightPushCount + 1;
            }
            else if (direction == Vector2.left) {
                isLeftPushMe = player.IsMoving && player.IsMovingRight;
                leftPushCount = player.leftPushCount + 1;
            }
        }
        // �ڽ��� ��쿡�� �̴� ����� ���� ������ �ʽ��ϴ�.
        else if (collision.transform.TryGetComponent(out Object_BoxController box)) {
            if (direction == Vector2.right) {
                isRightPushMe = box.isRightPushMe && box.isMoveable;            // �ڽ��� �и��� �ְ�, ������ �� �ִ� ��Ȳ
                rightPushCount = box.rightPushCount;
            }
            else if (direction == Vector2.left) {
                isLeftPushMe = box.isLeftPushMe && box.isMoveable;
                leftPushCount = box.leftPushCount;
            }
        }
        //Debug.Log("WOW");

        // �ƹ��� ���� �а����� �ʴٸ� �̴� ����� ���� �ʱ�ȭ�մϴ�.
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

//TODO: �÷��̾�Ե� �Ȱ��� �����ؾ� ��.
