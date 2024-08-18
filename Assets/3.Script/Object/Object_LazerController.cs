using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_LazerController : MonoBehaviour
{
    // �������� ���� �÷��̾ ������ die ȣ��
    public float LaserDistance;             // ������ ��� �Ÿ�


    private LineRenderer LineRenderer;
    private Transform laserFirePoint;           // laser start ����
    private Vector2 finishFirePoint;

    private void Awake() {

        laserFirePoint = GetComponent<Transform>();
        
        LineRenderer = GetComponentInChildren<LineRenderer>();

        finishFirePoint = new Vector2(transform.position.x + LaserDistance, transform.position.y);


    }
    private void Update() {
        ShootLaser();
    }
    private void ShootLaser() {
        Vector2 direction = ((Vector2)finishFirePoint - (Vector2)laserFirePoint.position);
        RaycastHit2D[] _hit = Physics2D.RaycastAll(laserFirePoint.position, direction);

        Debug.DrawRay(laserFirePoint.position, direction, Color.red);

        foreach (RaycastHit2D item in _hit) {

            if (item.collider.CompareTag("Player")) {
                Debug.Log("Laser : _hit | " + item.collider.name);
                DrawLaser(laserFirePoint.position, item.point);

                if (item.collider.TryGetComponent(out PlayerMove playerMove)) {
                    playerMove.Die();
                }
            }
        }

        //TODO: ���и����� tag �߰��ؼ� ���� ��
        DrawLaser(laserFirePoint.position, finishFirePoint);
    }


    private void DrawLaser(Vector2 startPosition, Vector2 endPosition) {
        Vector2 distancePosition = (endPosition - startPosition);
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(0, Vector2.zero);
        LineRenderer.SetPosition(1, distancePosition);
    }
}
