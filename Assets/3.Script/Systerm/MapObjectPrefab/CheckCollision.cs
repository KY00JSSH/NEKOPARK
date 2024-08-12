using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HasCollDirection {
    Left, Right, Top, Bottom
}

public class CheckCollision : MonoBehaviour {
    private bool[] checkCollBool;
    public bool GetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection]; }
    public bool SetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection] = true; }
    public bool ResetObjectHasDirection(HasCollDirection collDirection) { return checkCollBool[(int)collDirection] = false; }

    private void Awake() {
        checkCollBool = new bool[Enum.GetValues(typeof(HasCollDirection)).Length];
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {
            Vector3 collDirection = (transform.position - collision.transform.position);

            if (Mathf.Abs(collDirection.y) > Mathf.Abs(collDirection.x)) {
                if (collDirection.y > 0) {
                    SetObjectHasDirection(HasCollDirection.Bottom);
                }
                else {
                    SetObjectHasDirection(HasCollDirection.Top);
                }
            }
            else {
                if (collDirection.x > 0) {
                    SetObjectHasDirection(HasCollDirection.Left);
                }
                else {
                    SetObjectHasDirection(HasCollDirection.Right);
                }
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Box")) {

            Vector3 collDirection = (transform.position - collision.transform.position);

            if (Mathf.Abs(collDirection.y) > Mathf.Abs(collDirection.x)) {
                if (collDirection.y > 0) {
                    ResetObjectHasDirection(HasCollDirection.Bottom);
                }
                else {
                    ResetObjectHasDirection(HasCollDirection.Top);
                }
            }
            else {
                if (collDirection.x > 0) {
                    ResetObjectHasDirection(HasCollDirection.Left);
                }
                else {
                    ResetObjectHasDirection(HasCollDirection.Right);
                }
            }
        }

    }
}
