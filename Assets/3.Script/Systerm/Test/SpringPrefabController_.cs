using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpringPrefabController_ : MonoBehaviour {
    // 1. 목적 : 스프링 프리펩 오브젝트 기능 구현 => 해당 오브젝트와 충돌된 오브젝트가 몇 개인지  / 다른 오브젝트를 날려버리기 
    // 검출하는 레이어 : 8번 레이어 붙어있는지 검출함


    public float addForce = 2f;                                                 // Addforce 값 (레벨 별로 수정가능해야함)

    private Vector2 transformPosition;                                           // 스프링 위치 저장

    private Vector2 addForceVector;                                              // 충돌 물체 방향 확인
    private Vector2 saveDirectionVector;                                         // 충돌 물체 방향 초기 저장

    [SerializeField]private GameObject collisionObject;                                          // 날릴 오브젝트

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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) {
            saveDirectionVector = collision.transform.position - transform.position;
        }
    }


    // 배열의 오브젝트가 같다면 addforce
    //TODO: [김수주] 날리는 방향을 수정해야함
    private void AddforceObject() {
        if (collisionObject != null) {

            Rigidbody2D collRigidbody2D = collisionObject.GetComponent<Rigidbody2D>();
            if (collRigidbody2D != null) {
                //addForceVector = new Vector2(saveDirectionVector.x, addForce);
                collRigidbody2D.AddForce(saveDirectionVector * addForce, ForceMode2D.Impulse);
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
