using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameButtonController : MonoBehaviour, IPointerEnterHandler {
    private Image outline;
    private GameListController gameListController;

    private void Awake() {
        outline = GetComponentInChildren<Image>();
        gameListController = FindObjectOfType<GameListController>();
    }

    public void EnableOutline() {
        outline.enabled = true;
    }

    public void DisEnableOutline() {
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameListController.CheckHoverIndex(gameObject.name);
    }
}
