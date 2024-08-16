using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnZone : MonoBehaviour
{
    // 플레이어가 떨어지면 x좌표는 일정거리 뒤로 , y좌표는 일정한 값으로 리스폰

    private Camera mainCamera;

    private float respawnXDelta = 2f;
    private float respawnYPosition = 5.5f;

    private void Awake() {
        mainCamera = FindObjectOfType<Camera>();
        if (mainCamera == null) Debug.LogWarning("PlayerRespawnZone스크립트 | Awake | 메인 카메라 못찾음 ");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            if(collision.gameObject.TryGetComponent(out RectTransform playerRecttransform)) {

                Vector3 respawnPosition = new Vector3(playerRecttransform.position.x - respawnXDelta, respawnYPosition);

                // maincamera 영역 밖으로 안나가게 조정
                Vector3 playerPosToCameraViewPos = Camera.main.WorldToViewportPoint(respawnPosition);
                if(playerPosToCameraViewPos.x <= 0) { playerPosToCameraViewPos.x = 0.05f; }


                Vector3 playerPosChange = Camera.main.ViewportToWorldPoint(playerPosToCameraViewPos);
                playerPosChange.z = 0;

                playerRecttransform.position = playerPosChange;
            }
        }
    }
}
