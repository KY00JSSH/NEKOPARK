using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HasCollDirection {
    left, right, up, down, defaultD
}

public class CheckCollision : MonoBehaviour {
    private RectTransform rectTransform;
    private Vector2 lastPosition = Vector2.zero;

    private bool[] checkCollBool;
    public bool GetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection]; }
    public bool SetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection] = true; }
    public bool ResetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection] = false; }


    private bool checkCollisionIsPlayer;
    public bool GetCollisionIsPlayer() { return checkCollisionIsPlayer; }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        checkCollBool = new bool[Enum.GetValues(typeof(HasCollDirection)).Length];
    }
    /*
    private void Update() {
        SettingTransformIsPushing();
    }
    private void SettingTransformIsPushing() {
        if (transform.CompareTag("Player")) {
            PlayerMove playerMove = GetComponent<PlayerMove>();
            if (playerMove.IsMoving) checkTransformIsPushing= true;
            else checkTransformIsPushing= false;
        }
        else {
            checkTransformIsPushing = false;
        }
    }
    */

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {

            RectTransform collisionRect = collision.transform.GetComponent<RectTransform>();

            //Debug.Log("?????OnCollisionEnter2D?????" + gameObject.name);


            float collisionTop = collision.transform.position.y + collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            float collisionBottom = collision.transform.position.y - collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            float collisionRight = collision.transform.position.x + collisionRect.sizeDelta.x * collisionRect.localScale.x / 2f;
            float collisionLeft = collision.transform.position.x - collisionRect.sizeDelta.x * collisionRect.localScale.x / 2f;

            float objectTop = transform.position.y + rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f;
            float objectBottom = transform.position.y - rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f;
            float objectRight = transform.position.x + rectTransform.sizeDelta.x * rectTransform.localScale.x / 2f;
            float objectLeft = transform.position.x - rectTransform.sizeDelta.x * rectTransform.localScale.x / 2f;

            // 충돌체가 내 위에 있는 경우.
            if (collisionBottom >= objectTop) {
                SetObjectHasDirection(HasCollDirection.up);
            }
            // 충돌체가 내 아래에 있는 경우.
            else if (collisionTop <= objectBottom) {
                SetObjectHasDirection(HasCollDirection.down);
            }
            // 충돌체가 내 왼쪽에 있는 경우.
            else if (collisionRight <= objectLeft) {
                SetObjectHasDirection(HasCollDirection.left);
            }
            // 충돌체가 내 오른쪽에 있는 경우.
            else if (collisionLeft >= objectRight) {
                SetObjectHasDirection(HasCollDirection.right);
            }


            Debug.Log("?????OnCollisionEnter2D????? up" + gameObject.name + " | " + GetObjectHasDirection(HasCollDirection.up));
            Debug.Log("?????OnCollisionEnter2D????? down" + gameObject.name + " | " + GetObjectHasDirection(HasCollDirection.down));

        }

    }



    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {

            RectTransform collisionRect = collision.transform.GetComponent<RectTransform>();

            //Debug.Log("?????OnCollisionEnter2D?????" + gameObject.name);


            float collisionTop = collision.transform.position.y + collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            float collisionBottom = collision.transform.position.y - collisionRect.sizeDelta.y * collisionRect.localScale.y / 2f;
            float collisionRight = collision.transform.position.x + collisionRect.sizeDelta.x * collisionRect.localScale.x / 2f;
            float collisionLeft = collision.transform.position.x - collisionRect.sizeDelta.x * collisionRect.localScale.x / 2f;

            float objectTop = transform.position.y + rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f;
            float objectBottom = transform.position.y - rectTransform.sizeDelta.y * rectTransform.localScale.y / 2f;
            float objectRight = transform.position.x + rectTransform.sizeDelta.x * rectTransform.localScale.x / 2f;
            float objectLeft = transform.position.x - rectTransform.sizeDelta.x * rectTransform.localScale.x / 2f;

            // 충돌체가 내 위에 있는 경우.
            if (collisionBottom >= objectTop) {
                ResetObjectHasDirection(HasCollDirection.up);
            }
            // 충돌체가 내 아래에 있는 경우.
            else if (collisionTop <= objectBottom) {
                ResetObjectHasDirection(HasCollDirection.down);
            }
            // 충돌체가 내 왼쪽에 있는 경우.
            else if (collisionRight <= objectLeft) {
                ResetObjectHasDirection(HasCollDirection.left);
            }
            // 충돌체가 내 오른쪽에 있는 경우.
            else if (collisionLeft >= objectRight) {
                ResetObjectHasDirection(HasCollDirection.right);
            }


            //Debug.Log("?????OnCollisionEnter2D????? up" + gameObject.name + " | " + GetObjectHasDirection(HasCollDirection.up));
            //Debug.Log("?????OnCollisionEnter2D????? down" + gameObject.name + " | " + GetObjectHasDirection(HasCollDirection.down));

        }

    }
}
