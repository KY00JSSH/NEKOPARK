using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerMove : NetworkBehaviour {
    private float moveSpeed = 5f;
    public bool IsMoving { get; private set; }
    public bool IsMovingRight { get; private set; }     // 2024 08 14 ����� �׽�Ʈ�� �߰�

    public bool Haskey { get; private set; }

    private float jumpForce = 400f;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;

    private Animator playerAnimator;


    private Text textNickname;


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
        Jump();        
    }

    private void FixedUpdate() {
        Move();
        Jump_Limit();
    }

    private void Move() {
        if (!isOwned || !NetworkManager.singleton.DebuggingOverride) return;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            IsMoving = true;
            IsMovingRight = true;
            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            playerAnimator.SetBool("isMoving", true);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            IsMoving = true;
            IsMovingRight = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            playerAnimator.SetBool("isMoving", true);
        }
        else {
            IsMoving = false;
            playerAnimator.SetBool("isMoving", false);
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
        if (!isOwned || !NetworkManager.singleton.DebuggingOverride) return;

        if (Input.GetKey(KeyCode.Space) && !playerAnimator.GetBool("isJumping"))
        {            
            playerRigidbody.AddForce(new Vector2(0, jumpForce));            
            playerAnimator.SetBool("isJumping", true);

            AudioManager.instance.PlaySFX(AudioManager.Sfx.jump);
        }
    }

    private void Jump_Limit() {
        if (!isOwned || !NetworkManager.singleton.DebuggingOverride) return;

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
    }

    private void Die()      //�÷��̾� ���
    {

    }
    
    private void OpeningDoor()      //�� ����
    {

    }
}
