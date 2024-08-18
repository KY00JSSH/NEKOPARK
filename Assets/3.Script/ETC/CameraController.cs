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
        if(PlayersTransform.Count == 1)
        {
            return PlayersTransform[0].position;
        }

        var bounds = new Bounds(PlayersTransform[0].position, Vector3.zero);
        for(int i = 0; i < PlayersTransform.Count; i++)
        {
            bounds.Encapsulate(PlayersTransform[i].position);
        }
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

        FindAllPlayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()                                //카메라 이동
    {
        Vector3 playersCenterPoint = GetcenterPoint(); //플레이어들의 중간지점 계산

        Vector3 cameraPosition = playersCenterPoint + Camera_Offset;

        cameraPosition.x = 
            Mathf.Clamp(cameraPosition.x, Camera_minPosition.x, Camera_maxPosition.x);
        cameraPosition.y = 
            Mathf.Clamp(cameraPosition.y, Camera_minPosition.y, Camera_maxPosition.y);

        transform.position =
            Vector3.SmoothDamp(transform.position, cameraPosition, ref Velocity, Camera_MovingSmoothTime);
    }

    private void FindAllPlayers()
    {
        GamePlayer[] gamePlayers = FindObjectsOfType<GamePlayer>();

        PlayersTransform.Clear();       //기존의 리스트를 초기화
        foreach(var player in gamePlayers)
        {
            PlayersTransform.Add(player.transform);
        }
    }
}
