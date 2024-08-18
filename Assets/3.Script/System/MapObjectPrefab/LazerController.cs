using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour
{
    // �������� ���� �÷��̾ ������ die ȣ��
    [Header("Laser Info")]
    public float LaserDistance;             // ������ ��� �Ÿ�
    public float LaserInterval;             // ������ ����
    public int LaserNum;                    // ������ ����

    public GameObject LineRendererPrefab;

    private LineRenderer LineRenderer;
    private Transform laserFirePoint;           // laser start ����
    private Vector2 finishFirePoint;

    private void Awake() {

        laserFirePoint = LineRendererPrefab.GetComponent<Transform>();
        
        LineRenderer = LineRendererPrefab.GetComponentInChildren<LineRenderer>();

        finishFirePoint = new Vector2(LineRendererPrefab.transform.position.x + LaserDistance, LineRendererPrefab.transform.position.y);

        for (int i = 0; i < LaserNum; i++) {
            Vector2 interval =  new Vector2(transform.position.x + LaserInterval * i, transform.position.y);
            GameObject lineRenderer = Instantiate(LineRendererPrefab, interval, Quaternion.identity);
            lineRenderer.transform.SetParent(transform);
            lineRenderer.name = LineRendererPrefab.name;
        }
    }
    private void Update() {
        ShootLaser();
    }
    private void ShootLaser() {
        Vector2 direction = ((Vector2)finishFirePoint - (Vector2)laserFirePoint.position).normalized;
        RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, direction);

        //TODO: [�����] ���� �̻���
        Debug.Log(direction);

        if (_hit.collider.CompareTag("Player")) {
            Debug.Log("Laser : _hit | " + _hit.collider.name);
            DrawLaser(laserFirePoint.position, _hit.point);

            if (_hit.collider.TryGetComponent(out PlayerMove playerMove)) {                
                playerMove.Die();
            }
        }
        //TODO: ���и����� tag �߰��ؼ� ���� ��
        DrawLaser(laserFirePoint.position, finishFirePoint);
    }


    private void DrawLaser(Vector2 startPosition, Vector2 endPosition) {
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(0, startPosition);
        LineRenderer.SetPosition(1, endPosition);
    }
}
