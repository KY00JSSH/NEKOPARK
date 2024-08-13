using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverController : MonoBehaviour, IPointerEnterHandler {
    Image thisHoverOutline;
    ConfirmManager confirmManager;

    private void Awake() {
        thisHoverOutline = gameObject.GetComponentInChildren<Image>();
        confirmManager = FindObjectOfType<ConfirmManager>();
    }

    public void EnableImage() {
        thisHoverOutline.enabled = true;
    }

    public void DisEnableImage() {
        thisHoverOutline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(gameObject.name.Equals("Yes")) {
            confirmManager.SetIsHoverYes(true);
        }
        else {
            confirmManager.SetIsHoverYes(false);
        }
    }
}
