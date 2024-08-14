using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class JoinButtonHoverController : MonoBehaviour, IPointerEnterHandler {
    private Image thisHoverOutline;
    private JoinGameManager joinGameManager;

    private void Awake() {
        thisHoverOutline = gameObject.GetComponentInChildren<Image>();
        joinGameManager = FindObjectOfType<JoinGameManager>();
    }

    public void EnableImage() {
        thisHoverOutline.enabled = true;
    }

    public void DisEnableImage() {
        thisHoverOutline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        joinGameManager.GetHoverComponent(gameObject.name);
    }
}
