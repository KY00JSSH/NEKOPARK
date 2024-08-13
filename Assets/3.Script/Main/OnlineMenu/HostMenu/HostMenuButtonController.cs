using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class HostMenuButtonController : MonoBehaviour, IPointerEnterHandler {
    private HostMenuController hostMenuController;

    private void Awake() {
        hostMenuController = FindObjectOfType<HostMenuController>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (gameObject.name.Equals("Create")) {
            hostMenuController.SetSelectHostMenu(HostMenuType.CREATE);
        }
        else {
            hostMenuController.SetSelectHostMenu(HostMenuType.CANCEL);
        }
    }

}
