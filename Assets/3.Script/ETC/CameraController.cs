using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<Transform> PlayersTransform;       //�÷��̾���� Ʈ������
    
    private Vector2 Camera_minPosition;             //�� ���� ī�޶� �ּ���ġ
    private Vector2 Camera_maxPosition;             //�� ���� ī�޶� �ִ���ġ
    private Vector3 Camera_Offset;                  //ī�޶� ������
    
    private float Camera_MovingSmoothTime = 0.5f;   //ī�޶� �ε巴�� ���󰡴µ� �ɸ��� �ð�
    public float Camera_FixedZoom = 7f;             //ī�޶� �ܰ�(���������ϰ�)

    private Vector3 Velocity;

    private Vector3 GetcenterPoint()                //�÷��̾� �ټ��� ���, ī�޶� �߾�������
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
        Camera.main.orthographicSize = Camera_FixedZoom;    //ī�޶� �ܰ� �ʱ�ȭ

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

    private void Move()                                //ī�޶� �̵�
    {
        Vector3 playersCenterPoint = GetcenterPoint(); //�÷��̾���� �߰����� ���

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

        PlayersTransform.Clear();       //������ ����Ʈ�� �ʱ�ȭ
        foreach(var player in gamePlayers)
        {
            PlayersTransform.Add(player.transform);
        }
    }
}
