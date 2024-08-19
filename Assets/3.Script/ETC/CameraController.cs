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
        if (PlayersTransform.Count == 0)
        {            
            return Vector3.zero;
        }

        if (PlayersTransform.Count == 1)
        {
            //Debug.Log($"�÷��̾� ��ġ: {PlayersTransform[0].position}");
            return PlayersTransform[0].position;
        }

        var bounds = new Bounds(PlayersTransform[0].position, Vector3.zero);
        for(int i = 0; i < PlayersTransform.Count; i++)
        {
            bounds.Encapsulate(PlayersTransform[i].position);
            //Debug.Log($"�÷��̾� {i} ��ġ: {PlayersTransform[i].position}");
        }
        //Debug.Log($"���� �߽� ��ġ: {bounds.center}"); // �߽� ��ġ ���
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

        SetCameraBounds();

        FindAllPlayers();
        Invoke("FindAllPlayers", 0.5f);

        AudioManager.instance.StopBGM(0);
        AudioManager.Bgm selectedBGM = (Random.value > 0.5f) ? AudioManager.Bgm.GameBGM1 : AudioManager.Bgm.GameBGM2;
        AudioManager.instance.PlayBGM(selectedBGM, 0);
    }

    private void LateUpdate()
    {
        Move();
    }

    private void SetCameraBounds()
    {
        // BackGround ������Ʈ ã��
        GameObject background = GameObject.Find("BackGround");

        if (background != null)
        {
            // BackGround�� Transform ������Ʈ���� Scale ���� ������
            Transform bgTransform = background.transform;

            // Scale ���� ���� �ּ� �� �ִ� ��ġ ����
            Camera_minPosition = new Vector2(bgTransform.position.x - bgTransform.localScale.x / 2, bgTransform.position.y - bgTransform.localScale.y / 2);
            Camera_maxPosition = new Vector2(bgTransform.position.x + bgTransform.localScale.x / 2, bgTransform.position.y + bgTransform.localScale.y / 2);

            // �ʱ� ī�޶� ��ġ�� BackGround�� Position�� �������� ����
            Vector3 initialCameraPosition = new Vector3(bgTransform.position.x, bgTransform.position.y, Camera_Offset.z);
            transform.position = initialCameraPosition;

            //Debug.Log($"Camera_minPosition: {Camera_minPosition}, Camera_maxPosition: {Camera_maxPosition}");
        }
        else
        {
            Debug.LogWarning("BackGround ������Ʈ�� ã�� �� �����ϴ�. Camera_minPosition�� Camera_maxPosition�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void Move()                                //ī�޶� �̵�
    {
        Vector3 playersCenterPoint = GetcenterPoint(); //�÷��̾���� �߰����� ���
        //Debug.Log($"�÷��̾� �߽� ��ġ: {playersCenterPoint}");

        Vector3 cameraPosition = playersCenterPoint + Camera_Offset;

        cameraPosition.x = 
            Mathf.Clamp(cameraPosition.x, Camera_minPosition.x, Camera_maxPosition.x);
        cameraPosition.y = 
            Mathf.Clamp(cameraPosition.y, Camera_minPosition.y, Camera_maxPosition.y);

        //Debug.Log($"ī�޶� ��ǥ ��ġ: {cameraPosition}");
        //Debug.Log($"ī�޶� ��ǥ ��ġ(Ŭ���� ����): {cameraPosition}");
        transform.position =
            //Vector3.SmoothDamp(transform.position, cameraPosition, ref Velocity, Camera_MovingSmoothTime);
            Vector3.Lerp(transform.position, cameraPosition, Camera_MovingSmoothTime);
    }

    private void FindAllPlayers()
    {
        PlayersTransform.Clear();       //������ ����Ʈ�� �ʱ�ȭ

        PlayerMove[] gamePlayers = FindObjectsOfType<PlayerMove>();

        if(gamePlayers.Length > 0)
        {
            foreach (var player in gamePlayers)
            {
                PlayersTransform.Add(player.transform);
            }
            Debug.Log($"{PlayersTransform.Count}���� �÷��̾ ã�ҽ��ϴ�.");
        }
        else
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager != null)
            {
                // GameManager�� ��� �ڽ� ������Ʈ���� "LocalPlayer(Clone)"�� ã��
                Transform[] allTransforms = gameManager.GetComponentsInChildren<Transform>();
                foreach (Transform child in allTransforms)
                {
                    if (child.name.Contains("LocalPlayer(Clone)"))
                    {
                        PlayersTransform.Add(child);
                    }
                }
                Debug.Log($"{PlayersTransform.Count}���� LocalPlayer(Clone)�� ã�ҽ��ϴ�.");
            }

            if (PlayersTransform.Count == 0)
            {
                PlayersTransform.Add(new GameObject("Placeholder").transform);
                Debug.LogWarning("�÷��̾ ã�� ���߽��ϴ�. �÷��̽�Ȧ�� �߰�");
            }
        }        
    }
}
