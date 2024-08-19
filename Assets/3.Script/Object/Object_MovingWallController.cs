using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Object_MovingWallController : MonoBehaviour {
    // 1. 목적 : 움직일 거리와 움직임에 필요한 숫자를 확인해서 비율만큼 위아래로 움직이는 벽


    private int layerMask = (1 << 8);
    private int currentCollisionNum;
    private int preCollisionNum;
    private float smoothingFactor = 0.5f;                                        // y 좌표 이동 기본 값
    private bool isMoving = false;                                             // 코르틴 객체 움직이는 중 실행
    private bool isStartToChangeText;

    [Header("Base Size")]
    public float BaseWidth;
    public float BaseHeight;

    public GameObject PillarObject;                                             // 중간 기둥 오브젝트
    public GameObject BaseObject;                                               // 바닥 오브젝트

    [Space(5)]
    [Header("Move Info")]
    public int NeedNumToMove;
    public float MoveAmount;
    private float MoveRatio;                                                     // y 보정 값
    public bool isBaseMoveToUp;

    private Text NumText;

    private Vector2 originPosition;
    private Vector2 currentPosition = Vector2.zero;

    RaycastHit2D[] raycastHits;                                                      // base에 붙어 있는 오브젝트 중 위에 물체가 있는 객체에서 윗방향으로 raycast 배열 들고옴

    [SerializeField] private Collider2D[] hitCollisionAll;                           // base에 부딪힌 모든 박스 저장
    //[SerializeField] private Collider2D[] previousHitCollisionAll;


    private Collider2D Basecollider;
    private RectTransform baseRectTransform;
    private RectTransform rectTransform;

    private void Awake() {
        PillarObject = transform.GetChild(1).gameObject;
        BaseObject = transform.GetChild(2).gameObject;

        rectTransform = GetComponent<RectTransform>();

        Basecollider = BaseObject.GetComponent<Collider2D>();
        baseRectTransform = BaseObject.GetComponent<RectTransform>();
        baseRectTransform.localScale = new Vector2(BaseWidth, BaseHeight);

        MoveRatio = MoveAmount / (float)NeedNumToMove;

        NumText = GetComponentInChildren<Text>();

        originPosition = transform.position;
    }

    private void Update() {

        ChangeText();
        hitCollisionAll = GetOverlapBoxAll();
        CheckHitCollisionArrayAll();

        Debug.Log("currentCollisionNum | " + currentCollisionNum);
        Debug.Log("preCollisionNum | " + preCollisionNum);
        if (preCollisionNum != currentCollisionNum) {
            ChangeTransformPosition();
            preCollisionNum = currentCollisionNum;
            //Debug.Log("currentCollisionNum | " + currentCollisionNum);
        }
    }

    // 상자에 부딪힌 모든 객체를 배열에 담음 =>부딪힌 배열에 플레이어가 있다면 객체 담음
    private Collider2D[] GetOverlapBoxAll() {

        Vector2 actualSize = Basecollider.bounds.size;

        Collider2D[] _hitCollisionAll = Physics2D.OverlapBoxAll(BaseObject.transform.position, actualSize, 0, layerMask);

        // 불필요한 요소들을 제외하는 방법
        if (_hitCollisionAll.Length > 0) {

            foreach (Collider2D item in _hitCollisionAll) {
                if (item.gameObject == BaseObject) {
                    _hitCollisionAll = _hitCollisionAll.Where(_hitCollisionAll => _hitCollisionAll.gameObject != BaseObject).ToArray();
                }
            }

        }

        //foreach (var item in _hitCollisionAll) {
        //    Debug.LogWarning(item.name);
        //}

        return _hitCollisionAll;
    }

    // 필요 인원수 글자 변경 
    private void ChangeText() {

        int changeNum = (NeedNumToMove - currentCollisionNum);

        if (changeNum <= 0) changeNum = 0;

        NumText.text = changeNum.ToString();
    }

    // 필요 인원수 지금 확인한 인원수 비율대로 위아래 움직임
    private void ChangeTransformPosition() {
        if (isMoving) return;

        currentPosition = transform.position;
        int calculatorNum = (currentCollisionNum <= NeedNumToMove) ? currentCollisionNum : NeedNumToMove;

        // 목표 위치
        Vector2 PositionIWantToGo;
        if (isBaseMoveToUp) {
            PositionIWantToGo = originPosition + new Vector2(0, calculatorNum * MoveRatio);
        }
        else {
            PositionIWantToGo = originPosition - new Vector2(0, calculatorNum * MoveRatio);
        }


        if (PositionIWantToGo.y > currentPosition.y) {     // 현재위치가 낮으면 올라가야함
            StartCoroutine(MovingWallPositionChangeUp_Co(PositionIWantToGo));
        }
        else if(PositionIWantToGo.y < currentPosition.y) {
            StartCoroutine(MovingWallPositionChangeDown_Co(PositionIWantToGo));
        }

    }

    private IEnumerator MovingWallPositionChangeUp_Co(Vector2 PositionIWantToGo) {
        isMoving = true;
        while (currentPosition.y <= PositionIWantToGo.y) {
            currentPosition.y += Time.smoothDeltaTime; // Time.deltaTime * smoothingFactor;

            foreach (Collider2D each in hitCollisionAll) {
                Vector2 eachPositionChange = each.gameObject.transform.position;
                eachPositionChange.y += Time.smoothDeltaTime;
                each.gameObject.transform.position = eachPositionChange;
            }

            transform.position = currentPosition;
            //transform.position = Vector2.Lerp(transform.position, PositionIWantToGo, Time.deltaTime * smoothingFactor);
            yield return null;
        }

        isMoving = false;
        currentPosition.y = PositionIWantToGo.y;
    }

    private IEnumerator MovingWallPositionChangeDown_Co(Vector2 PositionIWantToGo) {
        isMoving = true;

        while (currentPosition.y >= PositionIWantToGo.y) {
            currentPosition.y -= Time.smoothDeltaTime; // Time.deltaTime * smoothingFactor;
            transform.position = currentPosition;

            foreach (Collider2D each in hitCollisionAll) {
                Vector2 eachPositionChange = each.gameObject.transform.position;
                eachPositionChange.y -= Time.smoothDeltaTime;
                each.gameObject.transform.position = eachPositionChange;
            }

            yield return null;
        }

        isMoving = false;
        currentPosition.y = PositionIWantToGo.y;
    }



    #region ??? 관측
    // 배열을 돌면서 위에 무언가 있는지 확인 : 개수 측정
    private void CheckHitCollisionArrayAll() {
        int allObjectCountNum = 0;
        for (int i = 0; i < hitCollisionAll.Length; i++) {
            if (CheckHitCollisionArray(hitCollisionAll[i].gameObject)) {
                raycastHits = null;
                raycastHits = FindCollisionObject(hitCollisionAll[i].gameObject);

                if (raycastHits.Length >= 1) {
                    allObjectCountNum += CountCollisionObject(raycastHits);
                }
            }

            allObjectCountNum++;

        }
        currentCollisionNum = allObjectCountNum;
    }

    // 찾는 객체의 위에 다른 물체가 있으면 true;
    private bool CheckHitCollisionArray(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CheckCollision checkCollision)) {
            if (checkCollision.GetObjectHasDirection(HasCollDirection.up)) {
                return true;
            }
        }
        return false;
    }

    // 찾는 객체 위에 물체가 있으면 해당 오브젝트에서 윗부분만 raycast를 쏠것
    private RaycastHit2D[] FindCollisionObject(GameObject _gameObject) {
        Vector3 rayDirection = _gameObject.transform.position;
        RaycastHit2D[] _raycastHits = Physics2D.RaycastAll(rayDirection, Vector2.up, layerMask);

        foreach (var item in _raycastHits) {
            //Debug.Log(item.collider.name);
        }

        // 불필요한 요소들을 제외하는 방법
        if (_raycastHits.Length > 0) {
            if (_raycastHits[0].collider.gameObject == _gameObject) {
                _raycastHits = _raycastHits.Where(_raycastHits => _raycastHits.collider.gameObject != _gameObject).ToArray();
            }
        }

        return _raycastHits;
    }

    // 찾은 배열의 돌면서 위아래 붙어있는 객체들 찾아서 숫자 return
    private int CountCollisionObject(RaycastHit2D[] _raycastHits) {
        int countCollisionsNum = 0;
        for (int i = 0; i < _raycastHits.Length; i++) {

            if (_raycastHits[i].collider.TryGetComponent(out CheckCollision checkCollision)) {

                if (checkCollision.GetObjectHasDirection(HasCollDirection.down)) {
                    countCollisionsNum++;
                    if (!checkCollision.GetObjectHasDirection(HasCollDirection.up)) {
                        break;
                    }
                }
                else {
                    break;
                }
            }
            
        }

        return countCollisionsNum;
    }

    #endregion

    // Gizmos를 사용하여 충돌 영역을 디버깅할 수 있는 방법
    private void OnDrawGizmos() {
        // Gizmos를 사용하여 모든 충돌 박스를 그립니다.
        Gizmos.color = Color.red;

        // Collider2D의 크기 사용
        Collider2D collider = BaseObject.GetComponent<Collider2D>();
        if (collider != null) {
            Vector3 boxCenter = collider.bounds.center;
            Vector3 boxSize = collider.bounds.size;

            // 충돌 박스를 그립니다.
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }

        // Hit Colliders를 그립니다.
        if (hitCollisionAll != null) {
            Gizmos.color = Color.green;
            foreach (Collider2D hitCollider in hitCollisionAll) {
                if (hitCollider != null) {
                    // 콜라이더의 중심을 표시
                    Gizmos.DrawWireCube(hitCollider.bounds.center, hitCollider.bounds.size);
                }
            }
        }
    }


}

/*
 1. 목적 : 움직일 거리와 움직임에 필요한 숫자를 확인해서 비율만큼 위아래로 움직이는 벽
 2. 내용
    1. 위 아래 콜라이더 확인
        원하는 tag list 중복 제외 저장
    2. 해당 물체 위 방향으로 raycast확인
        raycast 2개 이상인 경우만 콜라이더에 뭐가 있는지 확인
    3. 개수 추가

    4. 개수 비교
        1. 숫자 변경
        2. 비율대로 위아래 움직임



==========================================================================================
- 비율대로 위아래 움직여 봅시다
1. 위로 올라가야할 경우만 생각해보면

기준위치 y - 현재위치 y  = 전 인원 x 비율

차이 : 인원 x 비율 보다 작으면? 위로 올라가야함 



 
 */