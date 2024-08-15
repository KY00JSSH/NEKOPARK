using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections;

public class PlayerMove : NetworkBehaviour {
    private float moveSpeed = 5f;
    public bool IsMoving { get; private set; }
    public bool IsMovingRight { get; private set; }     // 2024 08 14 ����� �׽�Ʈ�� �߰�

    public bool Haskey { get; private set; }
    private bool IsPushingObject = false;
    private bool IsDie = false;

    private float jumpForce = 400f;
    private float dieAnimForce = 300f;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;

    private Animator playerAnimator;

    private Text textNickname;

    public bool isRightPushMe { get; private set; }
    public bool isLeftPushMe { get; private set; }
    public int rightPushCount { get; private set; }
    public int leftPushCount { get; private set; }


    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider  = GetComponent<Collider2D>();
        playerAnimator  = GetComponent<Animator>();

        textNickname    = GetComponentInChildren<Text>();
    }

    private void Start() {
        textNickname.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
    }

    private void Update()
    {
        if (!IsDie)
        {
            Jump();      
        }
    }

    private void FixedUpdate() {
        if (!IsDie)
        {
            Move();
            Jump_Limit();
        }
    }

    private void Move() {
        //if (!isOwned || !NetworkManager.singleton.DebuggingOverride) return;

        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0) {
            IsMoving = true;
            IsMovingRight = horizontalInput > 0;

            float moveDirection = horizontalInput > 0 ? 1f : -1f;
            transform.localScale = new Vector3(-moveDirection, 1f, 1f);
            transform.position += Vector3.right * moveSpeed * horizontalInput * Time.deltaTime;
            playerAnimator.SetBool("isMoving", true);

            if(IsPushingObject)
            {
                playerAnimator.SetBool("isPushing", true);
            }
            else
            {
                playerAnimator.SetBool("isPushing", false);
            }
        }
        else {
            IsMoving = false;
            playerAnimator.SetBool("isMoving", false);
            playerAnimator.SetBool("isPushing", false);
            
        }

        Vector3 textScale = new Vector3(0.02f, 0.02f, 0.02f);
        if (transform.localScale.x < 0) {
            textScale.x *= -1;
            textNickname.transform.localScale = textScale;
        }
        else if (transform.localScale.x > 0)
            textNickname.transform.localScale = textScale;
    }

    private void Jump() {
        //if (!isOwned || !NetworkManager.singleton.DebuggingOverride) return;

        if (Input.GetKey(KeyCode.Space) && !playerAnimator.GetBool("isJumping"))
        {            
            playerRigidbody.AddForce(new Vector2(0, jumpForce));            
            playerAnimator.SetBool("isJumping", true);

            //AudioManager.instance.PlaySFX(AudioManager.Sfx.jump);
        }
    }

    private void Jump_Limit() {
        //if (!isOwned || !NetworkManager.singleton.DebuggingOverride) return;

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

    public void SetHasKey(bool hasKey)
    {
        Haskey = hasKey;
        AudioManager.instance.PlaySFX(AudioManager.Sfx.getKeyDoorOpen);
    }

    private void Die()      
    {
        if (!IsDie)
        {
            IsDie = true;
            playerAnimator.SetBool("isDie", true);
            playerRigidbody.velocity = Vector2.zero;                    //속도 0으로 만들기
            playerRigidbody.AddForce(new Vector2(0, dieAnimForce));     //위로 튕기기
            StartCoroutine(PlayerDie_co(0.5f));                         //충돌 무시
        }
    }

    private IEnumerator PlayerDie_co(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerCollider.enabled = false;
        playerRigidbody.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.isTrigger)
        {
            IsPushingObject = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
        {
            IsPushingObject = false;
        }
    }

    /* 충돌 테스트
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 디버그 메시지
        Debug.Log($"OnCollisionEnter2D: {collision.gameObject.name} with {gameObject.name}");

        // 충돌 처리 예시
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with another player!");
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            Debug.Log("Collided with a box!");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌 종료 디버그 메시지
        Debug.Log($"OnCollisionExit2D: {collision.gameObject.name} with {gameObject.name}");

        // 충돌 종료 처리 예시
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("No longer colliding with another player!");
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            Debug.Log("No longer colliding with a box!");
        }
    }
    */
}
