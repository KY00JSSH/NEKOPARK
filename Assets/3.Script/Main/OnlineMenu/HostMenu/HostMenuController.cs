using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HostMenuType {
    TYPE = 0,
    COLOR,
    COUNT,
    CREATE,
    CANCEL
}
public class HostMenuController : MonoBehaviour {
    private HostMenuType selectHostMenu;

    [SerializeField] private Image[] hoverImage;

    private void Awake() {
        SetSelectHostMenu(HostMenuType.TYPE);
    }

    private void Update() {


        if (Input.GetButtonDown("Horizontal")) {
            if ((int)selectHostMenu > 2) {
                SetSelectHostMenu((int)selectHostMenu == 3 ? HostMenuType.CANCEL : HostMenuType.CREATE);
            }
            else {

            }
            return;
        }
        if (Input.GetButton("Vertical")) {
            if ((int)selectHostMenu > 2) {
                float verticalInput = Input.GetAxis("Vertical");
                if (verticalInput > 0) {
                    SetSelectHostMenu(HostMenuType.COUNT);
                }
                else {
                    SetSelectHostMenu(HostMenuType.TYPE);
                }
            }
            else {

            }
            return;
        }

        CheckHoverImage();
    }

    public void SetSelectHostMenu(HostMenuType type) {
        selectHostMenu = type;
    }

    public void CheckHoverImage() {
        switch (selectHostMenu) {
            case HostMenuType.TYPE:
                enableIndexImage(0);
                break;
            case HostMenuType.COLOR:
                enableIndexImage(1);
                break;
            case HostMenuType.COUNT:
                enableIndexImage(2);
                break;
            case HostMenuType.CREATE:
                enableIndexImage(3);
                break;
            case HostMenuType.CANCEL:
                enableIndexImage(4);
                break;
        }
    }

    private void enableIndexImage(int num) {
        for (int i = 0; i < hoverImage.Length; i++) {
            if (i == num) {
                hoverImage[i].enabled = true;
            }
            else {
                hoverImage[i].enabled = false;
            }
        }
    }
}
