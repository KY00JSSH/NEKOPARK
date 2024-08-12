using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour {
    [SyncVar] private float MoveSpeed = 5f;

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        if (!isOwned) return;
        float inputX = Input.GetAxisRaw("Horizontal");
        Vector3 moveDirection = Vector3.ClampMagnitude(new Vector3(inputX, 0, 0), 1f);

        if (moveDirection.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (moveDirection.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);

        transform.position += moveDirection * MoveSpeed * Time.deltaTime;
    }
}
