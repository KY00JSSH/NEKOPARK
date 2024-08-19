using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Object_MovingWallController : MonoBehaviour {
    // 1. ���� : ������ �Ÿ��� �����ӿ� �ʿ��� ���ڸ� Ȯ���ؼ� ������ŭ ���Ʒ��� �����̴� ��


    private int layerMask = (1 << 8);
    private int currentCollisionNum;
    private int preCollisionNum;
    private float smoothingFactor = 0.5f;                                        // y ��ǥ �̵� �⺻ ��
    private bool isMoving = false;                                             // �ڸ�ƾ ��ü �����̴� �� ����
    private bool isStartToChangeText;

    [Header("Base Size")]
    public float BaseWidth;
    public float BaseHeight;

    public GameObject PillarObject;                                             // �߰� ��� ������Ʈ
    public GameObject BaseObject;                                               // �ٴ� ������Ʈ

    [Space(5)]
    [Header("Move Info")]
    public int NeedNumToMove;
    public float MoveAmount;
    private float MoveRatio;                                                     // y ���� ��
    public bool isBaseMoveToUp;

    private Text NumText;

    private Vector2 originPosition;
    private Vector2 currentPosition = Vector2.zero;

    RaycastHit2D[] raycastHits;                                                      // base�� �پ� �ִ� ������Ʈ �� ���� ��ü�� �ִ� ��ü���� ���������� raycast �迭 ����

    [SerializeField] private Collider2D[] hitCollisionAll;                           // base�� �ε��� ��� �ڽ� ����
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

    // ���ڿ� �ε��� ��� ��ü�� �迭�� ���� =>�ε��� �迭�� �÷��̾ �ִٸ� ��ü ����
    private Collider2D[] GetOverlapBoxAll() {

        Vector2 actualSize = Basecollider.bounds.size;

        Collider2D[] _hitCollisionAll = Physics2D.OverlapBoxAll(BaseObject.transform.position, actualSize, 0, layerMask);

        // ���ʿ��� ��ҵ��� �����ϴ� ���
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

    // �ʿ� �ο��� ���� ���� 
    private void ChangeText() {

        int changeNum = (NeedNumToMove - currentCollisionNum);

        if (changeNum <= 0) changeNum = 0;

        NumText.text = changeNum.ToString();
    }

    // �ʿ� �ο��� ���� Ȯ���� �ο��� ������� ���Ʒ� ������
    private void ChangeTransformPosition() {
        if (isMoving) return;

        currentPosition = transform.position;
        int calculatorNum = (currentCollisionNum <= NeedNumToMove) ? currentCollisionNum : NeedNumToMove;

        // ��ǥ ��ġ
        Vector2 PositionIWantToGo;
        if (isBaseMoveToUp) {
            PositionIWantToGo = originPosition + new Vector2(0, calculatorNum * MoveRatio);
        }
        else {
            PositionIWantToGo = originPosition - new Vector2(0, calculatorNum * MoveRatio);
        }


        if (PositionIWantToGo.y > currentPosition.y) {     // ������ġ�� ������ �ö󰡾���
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



    #region ??? ����
    // �迭�� ���鼭 ���� ���� �ִ��� Ȯ�� : ���� ����
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

    // ã�� ��ü�� ���� �ٸ� ��ü�� ������ true;
    private bool CheckHitCollisionArray(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CheckCollision checkCollision)) {
            if (checkCollision.GetObjectHasDirection(HasCollDirection.up)) {
                return true;
            }
        }
        return false;
    }

    // ã�� ��ü ���� ��ü�� ������ �ش� ������Ʈ���� ���κи� raycast�� ���
    private RaycastHit2D[] FindCollisionObject(GameObject _gameObject) {
        Vector3 rayDirection = _gameObject.transform.position;
        RaycastHit2D[] _raycastHits = Physics2D.RaycastAll(rayDirection, Vector2.up, layerMask);

        foreach (var item in _raycastHits) {
            //Debug.Log(item.collider.name);
        }

        // ���ʿ��� ��ҵ��� �����ϴ� ���
        if (_raycastHits.Length > 0) {
            if (_raycastHits[0].collider.gameObject == _gameObject) {
                _raycastHits = _raycastHits.Where(_raycastHits => _raycastHits.collider.gameObject != _gameObject).ToArray();
            }
        }

        return _raycastHits;
    }

    // ã�� �迭�� ���鼭 ���Ʒ� �پ��ִ� ��ü�� ã�Ƽ� ���� return
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

    // Gizmos�� ����Ͽ� �浹 ������ ������� �� �ִ� ���
    private void OnDrawGizmos() {
        // Gizmos�� ����Ͽ� ��� �浹 �ڽ��� �׸��ϴ�.
        Gizmos.color = Color.red;

        // Collider2D�� ũ�� ���
        Collider2D collider = BaseObject.GetComponent<Collider2D>();
        if (collider != null) {
            Vector3 boxCenter = collider.bounds.center;
            Vector3 boxSize = collider.bounds.size;

            // �浹 �ڽ��� �׸��ϴ�.
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }

        // Hit Colliders�� �׸��ϴ�.
        if (hitCollisionAll != null) {
            Gizmos.color = Color.green;
            foreach (Collider2D hitCollider in hitCollisionAll) {
                if (hitCollider != null) {
                    // �ݶ��̴��� �߽��� ǥ��
                    Gizmos.DrawWireCube(hitCollider.bounds.center, hitCollider.bounds.size);
                }
            }
        }
    }


}

/*
 1. ���� : ������ �Ÿ��� �����ӿ� �ʿ��� ���ڸ� Ȯ���ؼ� ������ŭ ���Ʒ��� �����̴� ��
 2. ����
    1. �� �Ʒ� �ݶ��̴� Ȯ��
        ���ϴ� tag list �ߺ� ���� ����
    2. �ش� ��ü �� �������� raycastȮ��
        raycast 2�� �̻��� ��츸 �ݶ��̴��� ���� �ִ��� Ȯ��
    3. ���� �߰�

    4. ���� ��
        1. ���� ����
        2. ������� ���Ʒ� ������



==========================================================================================
- ������� ���Ʒ� ������ ���ô�
1. ���� �ö󰡾��� ��츸 �����غ���

������ġ y - ������ġ y  = �� �ο� x ����

���� : �ο� x ���� ���� ������? ���� �ö󰡾��� 



 
 */