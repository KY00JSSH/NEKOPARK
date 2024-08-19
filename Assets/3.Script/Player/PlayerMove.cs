using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMove : NetworkBehaviour {
    public float moveSpeed = 3f;
    public bool IsMoving { get; private set; }
    public bool IsMovingRight { get; private set; }     // 2024 08 14 ����� �׽�Ʈ�� �߰�

    public bool Haskey { get; private set; }
    private bool IsPushingObject = false;
    private bool IsDie = false;

    public float jumpForce = 400f;
    private float dieAnimForce = 450f;

    private Vector2 lastPosition = Vector2.zero;

    private SpriteRenderer playerSpriteRenderer;
    private RectTransform rectTransform;
    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;

    private Animator playerAnimator;

    private Text textNickname;

    public bool isRightPushMe { get; private set; }
    public bool isLeftPushMe { get; private set; }
    public int rightPushCount { get; private set; }
    public int leftPushCount { get; private set; }


    // 24 08 17 김수주 : 플레이어 문에 들어왔는지 확인하는 값 추가
    public bool IsPlayerEnterTheDoor { get; private set; }

    private void Awake() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerAnimator = GetComponent<Animator>();

        textNickname = GetComponentInChildren<Text>();

        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        textNickname.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
    }

    private void Update() {
        if (!IsDie) {
            Jump();
        }
    }

    private void FixedUpdate() {
        if (!IsDie) {
            Move();
            Jump_Limit();
        }
    }

    private void Move() {
        if (GameListManager.instance == null) {
            if (!isOwned && !NetworkManager.singleton.DebuggingOverride) return;
        }
        else {
            if (!GameListManager.instance.IsLocalGame) {
                if (!isOwned && !NetworkManager.singleton.DebuggingOverride) return;
            }
        }

        // 문에 들어갈 경우 이동하지 않도록 처리
        if (IsPlayerEnterTheDoor) return;


        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)                                  //입력 들어가면 움직이도록             
        {
            IsMoving = true;
            IsMovingRight = horizontalInput > 0;

            float moveDirection = horizontalInput > 0 ? 1f : -1f;
            transform.localScale = new Vector3(-moveDirection, 1f, 1f);
            transform.position += Vector3.right * moveSpeed * horizontalInput * Time.deltaTime;
        }
        else                                                        //입력 없으니까 움직임 false
        {
            IsMoving = false;
        }

        playerAnimator.SetBool("isMoving", IsMoving);              //여기도 기본 상태로
        playerAnimator.SetBool("isPushing", IsPushingObject);      //여기도 기본 상태

        Vector3 textScale = new Vector3(0.02f, 0.02f, 0.02f);
        if (transform.localScale.x < 0) {
            textScale.x *= -1;
            textNickname.transform.localScale = textScale;
        }
        else if (transform.localScale.x > 0)
            textNickname.transform.localScale = textScale;
    }

    private void Jump() {
        if (GameListManager.instance == null) {
            if (!isOwned && !NetworkManager.singleton.DebuggingOverride) return;
        }
        else {
            if (!GameListManager.instance.IsLocalGame) {
                if (!isOwned && !NetworkManager.singleton.DebuggingOverride) return;
            }
        }

        if (Input.GetKey(KeyCode.Space) && !playerAnimator.GetBool("isJumping")) {
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAnimator.SetBool("isJumping", true);

        }
            AudioManager.instance.PlaySFX(AudioManager.Sfx.jump);
    }

    private void Jump_Limit() {
        if (GameListManager.instance == null) {
            if (!isOwned && !NetworkManager.singleton.DebuggingOverride) return;
        }
        else {
            if (!GameListManager.instance.IsLocalGame) {
                if (!isOwned && !NetworkManager.singleton.DebuggingOverride) return;
            }
        }

        if (playerRigidbody.velocity.y < 0) {
            Vector2 feetPosition =
                new Vector2(playerRigidbody.position.x, playerRigidbody.position.y - playerCollider.bounds.extents.y - 0.3f);

            Debug.DrawRay(feetPosition, Vector3.down * 0.1f, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(feetPosition, Vector2.down, 1f);
            if (rayHit.collider != null) {
                if (rayHit.distance <= 0.1f) {
                    playerAnimator.SetBool("isJumping", false);
                }
            }
        }
    }

    public void SetHasKey(bool hasKey) {
        if (!IsDie) {
            Haskey = hasKey;
            AudioManager.instance.PlaySFX(AudioManager.Sfx.getKeyDoorOpen);
        }
    }

    public void Die() {
        if (!IsDie) {
            IsDie = true;
            playerAnimator.SetBool("isDie", true);
            playerRigidbody.velocity = Vector2.zero;                    //속도 0으로 만들기
            playerRigidbody.AddForce(new Vector2(0, dieAnimForce));     //위로 튕기기
            AudioManager.instance.PlaySFX(AudioManager.Sfx.playerDie);
            StartCoroutine(PlayerDie_co(1.5f));                         //충돌 무시
        }
    }

    private IEnumerator PlayerDie_co(float delay) {
        yield return new WaitForSeconds(delay);
        playerCollider.enabled = false;
        //playerRigidbody.isKinematic = true;
        if (!GameListManager.instance.IsLocalGame) {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        else {
            RestartWhenPlayerDie();
        }
    }

    public void RestartWhenPlayerDie() {
        // 플레이어가 죽었을 경우 해당 씬 확인해서 리로드되는 메서드
        var roomManager = NetworkManager.singleton as RoomManager;

        // 현재 씬 확인
        string currentSceneName = SceneManager.GetActiveScene().name;
        roomManager.ServerChangeScene(currentSceneName);
    }



    private void OnCollisionEnter2D(Collision2D collision) {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground") return;  //땅과 부딪히면 계속 push 애니메이션이 재생될테니 layer로 예외처리
        IsPushingObject = true;


        // 충돌 처리 예시
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Collided with another player!");
        }
        else if (collision.gameObject.CompareTag("Box")) {
            Debug.Log("Collided with a box!");
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        // 24 08 17 김수주 : 문이 열렸을 때, 플레이어가 문에 닿고 있을 경우 키를 누를면 들어감 여부 변경
        if (Object_DoorController.IsDoorOpen) {
            if (collision.CompareTag("Door")) {
                float verticalInput = Input.GetAxis("Vertical");

                if (verticalInput > 0) {
                    //TODO: 플레이어 이미지 없어져야함 + 움직임 막아야함
                    if (!IsPlayerEnterTheDoor) {  // 중복 호출 방지
                        IsPlayerEnterTheDoor = true;
                        Debug.Log("문 입장 플레이어 이름 " + transform.name);
                        SettingPlayerMoveWhenDoorOpen(true);
                    }
                }
                else if (verticalInput < 0) {
                    //TODO: 플레이어 이미지 나타나야함
                    // 문에서 나올 때
                    if (IsPlayerEnterTheDoor) {  // 중복 호출 방지
                        IsPlayerEnterTheDoor = false;
                        SettingPlayerMoveWhenDoorOpen(false);
                    }
                }
            }
        }
    }

    // 플레이어가 문에 들어가거나 나왔을 경우  => 1. 이미지 off, collider off, position fix
    private void SettingPlayerMoveWhenDoorOpen(bool doesplayerEnterDoor) {
        Canvas playerName = GetComponentInChildren<Canvas>();

        if (doesplayerEnterDoor) {
            playerName.gameObject.SetActive(false);
            playerSpriteRenderer.enabled = false;
            playerCollider.isTrigger = true; // 플레이어가 문에 닿고있어야 플레이어가 문에 들어갔다는 flag가 살아있음

            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;            
        }
        else {
            playerName.gameObject.SetActive(true);
            playerSpriteRenderer.enabled = true;
            playerCollider.isTrigger = false;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {

        // 위 아래 충돌은 무시합니다.
        if (collision.transform.TryGetComponent(out RectTransform collisionRect)) {

            if (collision.transform.TryGetComponent(out Object_MovingWallController movingWall))
            {
                return;
            }

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
        if (!collision.collider.isTrigger) {
            IsPushingObject = false;
        }

        isRightPushMe = isLeftPushMe = false;
        rightPushCount = 0;
        leftPushCount = 0;
    }
    private void OnDisable() {
        IsPlayerEnterTheDoor = false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // 충돌 디버그 메시지
    //    Debug.Log($"OnCollisionEnter2D: {collision.gameObject.name} with {gameObject.name}");
    //
    //    // 충돌 처리 예시
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Collided with another player!");
    //    }
    //    else if (collision.gameObject.CompareTag("Box"))
    //    {
    //        Debug.Log("Collided with a box!");
    //    }
    //}
    //
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // 충돌 종료 디버그 메시지
    //    Debug.Log($"OnCollisionExit2D: {collision.gameObject.name} with {gameObject.name}");
    //
    //    // 충돌 종료 처리 예시
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("No longer colliding with another player!");
    //    }
    //    else if (collision.gameObject.CompareTag("Box"))
    //    {
    //        Debug.Log("No longer colliding with a box!");
    //    }
    //}

}
