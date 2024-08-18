using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HostLoadConfirmButton : MonoBehaviour, IPointerEnterHandler {
    private Image thisHoverOutline;
    private HostLoadConfirmController controller;

    private void Awake() {
        thisHoverOutline = gameObject.GetComponentInChildren<Image>();
    }
    public void EnableImage() {
        thisHoverOutline.enabled = true;
    }

    public void DisEnableImage() {
        thisHoverOutline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        controller.GetHoverComponent(gameObject.name);
    }
}
