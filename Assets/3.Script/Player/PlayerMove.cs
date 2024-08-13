using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerMove : NetworkBehaviour 
{
    private float MoveSpeed = 5f;
    public bool isMovingRight { get; private set; }

    private float JumpForce = 400f;
   
    private Rigidbody2D PlayerRigidbody;
    private Collider2D PlayerCollider;

    private Animator PlayerAnimator;

    private Text textNickname;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<Collider2D>();
        textNickname = GetComponentInChildren<Text>();
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
        //if (!isOwned) return;

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            isMovingRight = true;
            transform.localScale = new Vector3(-1f, 1f, 1f);
            transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
            PlayerAnimator.SetBool("isMoving", true);
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            isMovingRight = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
            PlayerAnimator.SetBool("isMoving", true);
        }
        else
        {
            PlayerAnimator.SetBool("isMoving", false);
        }

        Vector3 textScale = new Vector3(0.02f, 0.02f, 0.02f);
        if (transform.localScale.x < 0) {
            textScale.x *= -1;
            textNickname.transform.localScale = textScale;
        }
        else if (transform.localScale.x > 0)
            textNickname.transform.localScale = textScale;
    }

    private void Jump()
    {
        if (!isOwned) return;

        if (Input.GetKey(KeyCode.Space) && !PlayerAnimator.GetBool("isJumping"))
        {            
            PlayerRigidbody.AddForce(new Vector2(0, JumpForce));            
            PlayerAnimator.SetBool("isJumping", true);
        }       
    }

    private void Jump_Limit() {
        //if (!isOwned) return;

        if (PlayerRigidbody.velocity.y < 0)
        {
            Vector2 feetPosition = 
                new Vector2(PlayerRigidbody.position.x, PlayerRigidbody.position.y - PlayerCollider.bounds.extents.y - 0.3f);

            Debug.DrawRay(feetPosition, Vector3.down * 0.1f, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(feetPosition, Vector2.down, 1f);
            if (rayHit.collider != null)
            {
                if (rayHit.distance <= 0.1f)
                {
                    PlayerAnimator.SetBool("isJumping", false);
                }
            }
        }
    }
}
