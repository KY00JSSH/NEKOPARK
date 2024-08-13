using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour 
{
    private float MoveSpeed = 5f;
    private bool isMovingRight;

    private float JumpForce = 400f;
   
    private Rigidbody2D PlayerRigidbody;
    private Collider2D PlayerCollider;

    private Animator PlayerAnimator;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerCollider = GetComponent<Collider2D>();
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
        if (!isOwned) return;

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
                
        //float inputX = Input.GetAxisRaw("Horizontal");
        //Vector3 moveDirection = Vector3.ClampMagnitude(new Vector3(inputX, 0, 0), 1f);
        //
        //if (moveDirection.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
        //else if (moveDirection.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
        //
        //transform.position += moveDirection * MoveSpeed * Time.deltaTime;
    }

    private void Jump()
    {        
        if (Input.GetKey(KeyCode.Space) && !PlayerAnimator.GetBool("isJumping"))
        {            
            PlayerRigidbody.AddForce(new Vector2(0, JumpForce));            
            PlayerAnimator.SetBool("isJumping", true);
        }       
    }

    private void Jump_Limit() {
        if (!isOwned) return;

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
