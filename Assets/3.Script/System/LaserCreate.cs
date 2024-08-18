using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCreate : MonoBehaviour {

    public GameObject LineRendererPrefab;

    [Header("Laser Info")]
    public float LaserInterval;             // 레이저 간격
    public int LaserNum;                    // 레이저 개수
    public bool isVertical;
    private void Awake() {
        if (isVertical) {
            
            for (int i = 0; i < LaserNum; i++) {
                Vector2 interval = new Vector2(transform.position.x , transform.position.y + LaserInterval * i);

                GameObject lineRenderer = Instantiate(LineRendererPrefab, interval, Quaternion.identity);
                lineRenderer.transform.SetParent(transform);
                lineRenderer.name = LineRendererPrefab.name;
            }
        }
        else {
            for (int i = 0; i < LaserNum; i++) {
                Vector2 interval = new Vector2(transform.position.x + LaserInterval * i, transform.position.y);

                GameObject lineRenderer = Instantiate(LineRendererPrefab, interval, Quaternion.Euler(0, 0, 90));
                lineRenderer.transform.SetParent(transform);
                lineRenderer.name = LineRendererPrefab.name;
            }
        }

    }


}
