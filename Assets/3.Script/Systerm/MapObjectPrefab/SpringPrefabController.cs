using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpringPrefabController : MonoBehaviour {
    // 1. 목적 : 스프링 프리펩 오브젝트 기능 구현 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지  / 다른 오브젝트를 날려버리기 
    // 검출하는 레이어 : 8번 레이어 붙어있는지 검출함

    private float moveX;
    public float addForce;                                                 // Addforce 값 (레벨 별로 수정가능해야함)

    private Vector2 transformPosition;                                           // 스프링 위치 저장

    private Vector2 addForceVector;                                              // 충돌 물체 방향 확인
    private Vector2 saveDirectionVector;                                         // 충돌 물체 방향 초기 저장

    [SerializeField] private GameObject collisionObject;                                          // 날릴 오브젝트

    private FindCollisionObjects findCollisionObjects;                           // 충돌 물체 찾는 스크립트

    private Animator spriteAnimator;                                             // spring image animation

    private void SetCollObject() { collisionObject = findCollisionObjects.GetCollObject(); }  // 날릴 오브젝트 1개일 때 받아오기



    private void Awake() {
        transformPosition = transform.position;

        findCollisionObjects = GetComponent<FindCollisionObjects>();

        spriteAnimator = GetComponent<Animator>();
    }


    private void Update() {

        if (findCollisionObjects.GetIsObjectOnlyOne()) {
            SetCollObject();
            AddforceObject();
            collisionObject = null;
        }

    }

    // enter에서 충돌 물체 방향 초기 저장
    private void OnCollisionEnter2D(Collision2D collision) {
        float topY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
            Vector2 collisionPosition;

            // Handle different types of colliders
            if (collision.gameObject.CompareTag("Player")) {
                collisionPosition = collision.transform.position;
            }
            else {
                RectTransform rectTransform = collision.gameObject.GetComponent<RectTransform>();
                collisionPosition = rectTransform.position;
            }

            if (collisionPosition.y >= topY) {
                Vector2 relativeVelocity = collision.relativeVelocity;
                moveX = relativeVelocity.x >= 0f ? 1f : -1f;
                //Debug.Log("Collision direction (X): " + moveX);
            }
            else {
                return;
            }
        }

        /*
         
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
            Vector2 relativeVelocity = collision.relativeVelocity; // 충돌체가 들어오는 방향이 맞나?

            moveX = relativeVelocity.x >= 0f ? 1f : -1f;

            Debug.Log("Collision direction (X): " + moveX);
        }
         */
    }
    private void OnCollisionExit2D(Collision2D collision) {
        moveX = 0f;
    }


    // 배열의 오브젝트가 같다면 addforce
    //TODO: [김수주] 상자 날리는 방향 없어짐, 랜덤으로 한 번씩 세게 날라감
    private void AddforceObject() {
        if (collisionObject != null) {

            Rigidbody2D collRigidbody2D = collisionObject.GetComponent<Rigidbody2D>();

            if (collRigidbody2D != null) {
                Vector2 force;
                Vector2 position;

                if (collisionObject.gameObject.CompareTag("Player")) {

                    //Debug.Log("? x방향 뭔데요 왜 안되는건데 시발것 " + force.x + " | " + force.y);
                    position = collisionObject.transform.position;
                }
                else {
                    RectTransform rectTransform = collisionObject.GetComponent<RectTransform>();
                    position = rectTransform.anchoredPosition;
                }

                force = new Vector2(moveX * addForce * 0.3f, addForce);
                //Debug.Log("? x방향 뭔데요 왜 안되는건데 시발것 " + force.x + " | " + force.y);
                collRigidbody2D.AddForceAtPosition(force, position, ForceMode2D.Impulse); // 물체 날리기
                collisionObject = null;
            }
        }
    }


}

/*  
 1. 목적2 : 스프링 프리펩 오브젝트 기능 구현 -> 다른 오브젝트를 날려버리기 
 2. 내용
    2-1. 변수 
        1) Addforce를 하려는 값 설정 : public float로 외부에서 설정 => 레벨별 점프대의 성능이 달라지는 경우가 있음
        2) addForceVector : 충돌 물체 방향 확인 용도
        3) spriteAnimator : spring 이미지 변경 애니메이션
    2-2. 처리 내용 
        1) FindCollisionObjects에서확인한 bool값 사용
        2) 물체가 들어온 방향 확인
        3) 확인된 object의 진입방향을 확인하여 해당 방향으로 날림
        4) 스프라이트 변경 
 3. 해당 스크립트 위치 : 스프링 프리펩
 */
