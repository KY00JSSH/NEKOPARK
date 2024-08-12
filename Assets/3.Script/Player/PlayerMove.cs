using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour 
{
    private float MoveSpeed = 5f;
    private bool isMovingRight;

    private float JumpForce = 400f;
    private int JumpCount = 1;

    private Rigidbody2D PlayerRigidbody;

    private Animator PlayerAnimator;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        Move();
        Jump();        
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
        if (JumpCount == 0) return;

        if (Input.GetKey(KeyCode.Space) && JumpCount != 0)
        {
            JumpCount--;
            PlayerRigidbody.AddForce(new Vector2(0, JumpForce));            
            PlayerAnimator.SetBool("isJumping", true);
        }

        if(PlayerRigidbody.velocity.y < 0)
        { 
            Debug.DrawRay(PlayerRigidbody.position, Vector3.down, Color.red);
            RaycastHit2D rayHit = Physics2D.Raycast(PlayerRigidbody.position, Vector3.down, 1);
            if(rayHit.transform.gameObject != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    PlayerAnimator.SetBool("isJumping", false);
                    JumpCount = 1;
                }
            }
        }

    }
}
