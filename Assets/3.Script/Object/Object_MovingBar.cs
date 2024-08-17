using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_MovingBar : MonoBehaviour
{

    // 버튼을 누르면 scale 커지게 함
    
    private int layerMask = (1 << 31);                          // ground
    public float Speed = 1f;                                    // Speed of scaling

    [Header("Bar Size")]
    public float BarWidth;

    [Space(5)]
    [Header("Move Info")]
    public GameObject UseButton;                                // moving bar 에서 사용될 버튼 지정

    private bool isMoveStop;                                    // 코르틴을 한번 실행하기 위한 값\


    private Vector2 originPosition;
    private Vector2 currentPosition = Vector2.zero;

    private Transform barTransform;
    private Collider2D barCollider;

    //TODO: [김수주] Need button pushed flag

    private void Awake() {

        barTransform = transform.GetComponent<Transform>();
    }

    private void Update() {
        if (SetButtonPushed() /*button push*/) {
            if (!isMoveStop) {
                Move();
            }
        }
    }

    private bool SetButtonPushed() {
        if (UseButton.transform.GetChild(0).TryGetComponent(out Object_Button_Once button_Once)) {
            if (button_Once.GetIsButtonPushed()) return true;
        }
        return false;
    }



    // 버튼이 눌렸을 경우 bar가 왼쪽 오른쪽 bool값에 따라 이동 ( 양 끝의 tip이 부딪혔을 경우 정지)
    private void Move() {
        currentPosition = transform.position;
        StartCoroutine(MovingBarPositionChangeLeft_Co());
    }

    private IEnumerator MovingBarPositionChangeLeft_Co() {
        isMoveStop = true;

        // Get the starting scale
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(BarWidth, initialScale.y, initialScale.z);

        float elapsedTime = 0f;
        float duration = Mathf.Abs(BarWidth - initialScale.x) / Speed;

        while (elapsedTime < duration) {
            // Calculate the current scale based on elapsed time
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = targetScale;
    }

}

