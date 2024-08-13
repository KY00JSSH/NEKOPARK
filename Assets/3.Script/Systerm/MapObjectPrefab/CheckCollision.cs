using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HasCollDirection {
    left, right, up, down, defaultD
}

public class CheckCollision : MonoBehaviour {
    private bool[] checkCollBool;
    public bool GetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection]; }
    public bool SetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection] = true; }
    public bool ResetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection] = false; }


    private bool checkCollisionIsPlayer;
    public bool GetCollisionIsPlayer() { return checkCollisionIsPlayer; }

    private void Awake() {
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {

            if (collision.transform.CompareTag("Player")) checkCollisionIsPlayer = true;
            else checkCollisionIsPlayer = false;

            Vector3 collDirection = (transform.position - collision.transform.position);

            if (Mathf.Abs(collDirection.y) > Mathf.Abs(collDirection.x)) {
                if (collDirection.y > 0) {
                    SetObjectHasDirection(HasCollDirection.down);
                }
                else {
                    SetObjectHasDirection(HasCollDirection.up);
                }
            }
            else {
                if (collDirection.x > 0) {
                    SetObjectHasDirection(HasCollDirection.left);
                }
                else {
                    SetObjectHasDirection(HasCollDirection.right);
                }
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {

            Vector3 collDirection = (transform.position - collision.transform.position);

            if (Mathf.Abs(collDirection.y) > Mathf.Abs(collDirection.x)) {
                if (collDirection.y > 0) {
                    ResetObjectHasDirection(HasCollDirection.down);
                }
                else {
                    ResetObjectHasDirection(HasCollDirection.up);
                }
            }
            else {
                if (collDirection.x > 0) {
                    ResetObjectHasDirection(HasCollDirection.left);
                }
                else {
                    ResetObjectHasDirection(HasCollDirection.right);
                }
            }
        }

    }
}
