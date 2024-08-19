using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<Transform> PlayersTransform;       //플레이어들의 트랜스폼
    
    private Vector2 Camera_minPosition;             //맵 상의 카메라 최소위치
    private Vector2 Camera_maxPosition;             //맵 상의 카메라 최대위치
    private Vector3 Camera_Offset;                  //카메라 오프셋
    
    private float Camera_MovingSmoothTime = 0.5f;   //카메라가 부드럽게 따라가는데 걸리는 시간
    public float Camera_FixedZoom = 7f;             //카메라 줌값(수정가능하게)

    private Vector3 Velocity;

    private Vector3 GetcenterPoint()                //플레이어 다수일 경우, 카메라 중앙포지션
    {
        if (PlayersTransform.Count == 0)
        {            
            return Vector3.zero;
        }

        if (PlayersTransform.Count == 1)
        {
            Debug.Log($"플레이어 위치: {PlayersTransform[0].position}");
            return PlayersTransform[0].position;
        }

        var bounds = new Bounds(PlayersTransform[0].position, Vector3.zero);
        for(int i = 0; i < PlayersTransform.Count; i++)
        {
            bounds.Encapsulate(PlayersTransform[i].position);
            Debug.Log($"플레이어 {i} 위치: {PlayersTransform[i].position}");
        }
        Debug.Log($"계산된 중심 위치: {bounds.center}"); // 중심 위치 출력
        return bounds.center;
    }

    private void Awake()
    {
        Velocity = Vector3.zero;
        PlayersTransform = new List<Transform>();
    }

    private void Start()
    {
        Camera.main.orthographicSize = Camera_FixedZoom;    //카메라 줌값 초기화

        if(Camera_Offset == Vector3.zero)
        {
            Camera_Offset = new Vector3(0, 0, -10);
        }

        SetCameraBounds();

        FindAllPlayers();
        Invoke("FindAllPlayers", 0.5f);
    }

    private void LateUpdate()
    {
        Move();
    }

    private void SetCameraBounds()
    {
        // BackGround 오브젝트 찾기
        GameObject background = GameObject.Find("BackGround");

        if (background != null)
        {
            // BackGround의 Transform 컴포넌트에서 Scale 값을 가져옴
            Transform bgTransform = background.transform;

            // Scale 값에 따라 최소 및 최대 위치 설정
            Camera_minPosition = new Vector2(bgTransform.position.x - bgTransform.localScale.x / 2, bgTransform.position.y - bgTransform.localScale.y / 2);
            Camera_maxPosition = new Vector2(bgTransform.position.x + bgTransform.localScale.x / 2, bgTransform.position.y + bgTransform.localScale.y / 2);

            // 초기 카메라 위치를 BackGround의 Position을 기준으로 설정
            Vector3 initialCameraPosition = new Vector3(bgTransform.position.x, bgTransform.position.y, Camera_Offset.z);
            transform.position = initialCameraPosition;

            Debug.Log($"Camera_minPosition: {Camera_minPosition}, Camera_maxPosition: {Camera_maxPosition}");
        }
        else
        {
            Debug.LogWarning("BackGround 오브젝트를 찾을 수 없습니다. Camera_minPosition과 Camera_maxPosition이 설정되지 않았습니다.");
        }
    }

    private void Move()                                //카메라 이동
    {
        Vector3 playersCenterPoint = GetcenterPoint(); //플레이어들의 중간지점 계산
        Debug.Log($"플레이어 중심 위치: {playersCenterPoint}");

        Vector3 cameraPosition = playersCenterPoint + Camera_Offset;

        cameraPosition.x = 
            Mathf.Clamp(cameraPosition.x, Camera_minPosition.x, Camera_maxPosition.x);
        cameraPosition.y = 
            Mathf.Clamp(cameraPosition.y, Camera_minPosition.y, Camera_maxPosition.y);

        //Debug.Log($"카메라 목표 위치: {cameraPosition}");
        Debug.Log($"카메라 목표 위치(클램프 적용): {cameraPosition}");
        transform.position =
            //Vector3.SmoothDamp(transform.position, cameraPosition, ref Velocity, Camera_MovingSmoothTime);
            Vector3.Lerp(transform.position, cameraPosition, Camera_MovingSmoothTime);
    }

    private void FindAllPlayers()
    {
        PlayersTransform.Clear();       //기존의 리스트를 초기화

        GamePlayer[] gamePlayers = FindObjectsOfType<GamePlayer>();

        if(gamePlayers.Length > 0)
        {
            foreach (var player in gamePlayers)
            {
                PlayersTransform.Add(player.transform);
            }
            Debug.Log($"{PlayersTransform.Count}명의 플레이어를 찾았습니다.");
        }
        else
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager != null)
            {
                // GameManager의 모든 자식 오브젝트에서 "LocalPlayer(Clone)"을 찾기
                Transform[] allTransforms = gameManager.GetComponentsInChildren<Transform>();
                foreach (Transform child in allTransforms)
                {
                    if (child.name.Contains("LocalPlayer(Clone)"))
                    {
                        PlayersTransform.Add(child);
                    }
                }
                Debug.Log($"{PlayersTransform.Count}명의 LocalPlayer(Clone)을 찾았습니다.");
            }

            if (PlayersTransform.Count == 0)
            {
                PlayersTransform.Add(new GameObject("Placeholder").transform);
                Debug.LogWarning("플레이어를 찾지 못했습니다. 플레이스홀더 추가");
            }
        }        
    }
}
