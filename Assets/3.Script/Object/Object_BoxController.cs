using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Object_BoxController : MonoBehaviour {
    private RectTransform rectTransform;
    private Rigidbody2D boxRigid;
    private Text textCount;

    private RaycastHit2D[] hits = null;

    public bool isRightPushMe { get; private set; }
    public bool isLeftPushMe { get; private set; }

    // �̴µ� �ʿ��� �� ī��Ʈ�� ���� ī��Ʈ
    [SerializeField] private int requiredCount = 1;
    private int currentCount;

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
        currentCount = requiredCount;
        if (hits != null)
            currentCount = requiredCount - hits.Length;

        if (currentCount > requiredCount) currentCount = requiredCount;
        else if (currentCount < 0) currentCount = 0;
        textCount.text = currentCount.ToString();
    }

    private void SetMoveAvailable() {
        // �������̰� �����մϴ�.
        if (currentCount != 0) 
            boxRigid.bodyType = RigidbodyType2D.Kinematic;
        else 
            boxRigid.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        // �� �Ʒ� �浹�� �����մϴ�.
        if (collision.transform.position.y >= transform.position.y + rectTransform.sizeDelta.y / 2f ||
            collision.transform.position.y <= transform.position.y - rectTransform.sizeDelta.y / 2f) return;

        // �浹ü�� ���� �а� �ִ� ���������� �����ɴϴ�.
        if (collision.transform.TryGetComponent(out PlayerMove player)) {
            isRightPushMe = player.IsMoving && player.IsMovingRight;
            isLeftPushMe = player.IsMoving && !player.IsMovingRight;
        }
        else if (collision.transform.TryGetComponent(out Object_BoxController box)) {
            isRightPushMe = box.isRightPushMe;
            isLeftPushMe = box.isLeftPushMe;
        }

        // �浹ü �������� ���̸� ���ϴ�.
        Vector2 origin = transform.position;
        origin.y = collision.transform.position.y;

        Vector2 direction = Vector2.right;
        if (collision.transform.position.x > transform.position.x) direction = Vector2.right;
        else if (collision.transform.position.x < transform.position.x) direction = Vector2.left;

        // �׷��� �� ����.
        if ((direction == Vector2.right && isLeftPushMe) ||
            (direction == Vector2.left && isRightPushMe)) {
            hits = Physics2D.RaycastAll(origin, direction);
        }
        else
            hits = null;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        hits = null;
    }
}