using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class OnlineButtonController : MonoBehaviour, IPointerEnterHandler {
    private Image thisHoverOutline;
    private OnlineMenuManager onlineMenuManager;


    private void Awake() {
        thisHoverOutline = gameObject.GetComponentInChildren<Image>();
        onlineMenuManager = FindObjectOfType<OnlineMenuManager>();
    }

    public void EnableImage() {
        thisHoverOutline.enabled = true;
    }

    public void DisEnableImage() {
        thisHoverOutline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (gameObject.name.Equals("Public")) {
            onlineMenuManager.setMenuType(OnlineMenuType.PUBLIC);
        }
        else if (gameObject.name.Equals("Friend")) {
            onlineMenuManager.setMenuType(OnlineMenuType.FREIND);
        }
        else if (gameObject.name.Equals("Host")) {
            onlineMenuManager.setMenuType(OnlineMenuType.HOST);
        }
        else {
            onlineMenuManager.setMenuType(OnlineMenuType.OPTION);
        }
    }
}
