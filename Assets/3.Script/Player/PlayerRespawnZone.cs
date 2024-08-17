using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnZone : MonoBehaviour
{
    // �÷��̾ �������� x��ǥ�� �����Ÿ� �ڷ� , y��ǥ�� ������ ������ ������

    private Camera mainCamera;

    private float respawnXDelta = 2f;
    private float respawnYPosition = 5.5f;

    private void Awake() {
        mainCamera = FindObjectOfType<Camera>();
        if (mainCamera == null) Debug.LogWarning("PlayerRespawnZone��ũ��Ʈ | Awake | ���� ī�޶� ��ã�� ");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            if(collision.gameObject.TryGetComponent(out RectTransform playerRecttransform)) {

                Vector3 respawnPosition = new Vector3(playerRecttransform.position.x - respawnXDelta, respawnYPosition);

                // maincamera ���� ������ �ȳ����� ����
                Vector3 playerPosToCameraViewPos = Camera.main.WorldToViewportPoint(respawnPosition);
                if(playerPosToCameraViewPos.x <= 0) { playerPosToCameraViewPos.x = 0.05f; }


                Vector3 playerPosChange = Camera.main.ViewportToWorldPoint(playerPosToCameraViewPos);
                playerPosChange.z = 0;

                playerRecttransform.position = playerPosChange;
            }
        }
    }
}
