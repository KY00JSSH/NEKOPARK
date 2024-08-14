using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPrefabController_ : MonoBehaviour {
    // 1. 목적 : 스프링 프리펩 오브젝트 기능 구현 -> 다른 오브젝트를 날려버리기 

    private int layerMask = (1 << 8);                           // 검출 레이어
    public float addForce = 5f;                                 // Addforce 값 (레벨 별로 수정가능해야함)
    private Vector2 addForceVector; // 충돌 물체 방향 확인
    private Vector2 saveDirectionVector;                         // 충돌 물체 방향 초기 저장

    private Animator spriteAnimator; // spring image animation

    private FindCollisionObjectsNum findCollisionObjectsNum;

    private void Awake() {
        spriteAnimator = GetComponent<Animator>();

        findCollisionObjectsNum = GetComponent<FindCollisionObjectsNum>();
    }

    // enter에서 충돌 물체 방향 초기 저장
    private void OnCollisionEnter2D(Collision2D collision) {
        saveDirectionVector.x = collision.transform.position.x - transform.position.x;
    }

    // 충돌한 물체의 진입방향을 확인하여 addforce
    private void OnCollisionStay2D(Collision2D collision) {
        Debug.Log("SpringPrefabController : " + findCollisionObjectsNum.GetCollObjectsNum());

        if (findCollisionObjectsNum.GetCollObjectsNum()==1) {
            Rigidbody2D collRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collRigidbody2D != null) {

                //addForceVector = new Vector2(saveDirectionVector.x, addForce);
                collRigidbody2D.AddForce(collRigidbody2D.transform.up * addForce, ForceMode2D.Impulse);
            }
        }
    }

}

/*  
 1. 목적 : 스프링 프리펩 오브젝트 기능 구현 -> 다른 오브젝트를 날려버리기 
 2. 내용
    2-1. 변수 
        1) Addforce를 하려는 값 설정 : public float로 외부에서 설정 => 레벨별 점프대의 성능이 달라지는 경우가 있음
        2) addForceVector : 충돌 물체 방향 확인 용도
        3) spriteAnimator : spring 이미지 변경 애니메이션
    2-2. 처리 내용 
        1) Collider 확인 (스프링 프리펩의 상부만 확인)
        2) if( Collider's num >= ....) return? => 1개만 점프할 수 있음 //TODO: FindCollistionObjects 개수 확인 가능한가?
        3) 확인된 object의 진입방향을 확인하여 해당 방향으로 날림
        4) 스프라이트 변경 //TODO: spring sprite 생성 되면 animator 만들어야함
 3. 해당 스크립트 위치 : 스프링 프리펩
 */
