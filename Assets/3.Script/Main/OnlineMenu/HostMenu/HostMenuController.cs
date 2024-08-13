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
    private HostMenuValueController[] hostMenuValueControllers;

    private void Awake() {
        SetSelectHostMenu(HostMenuType.TYPE);
        hostMenuValueControllers = FindObjectsOfType<HostMenuValueController>();
    }

    private void Update() {
        if (Input.GetButtonDown("Horizontal")) {
            if ((int)selectHostMenu > 2) {
                SetSelectHostMenu((int)selectHostMenu == 3 ? HostMenuType.CANCEL : HostMenuType.CREATE);
            }
            else {
                float horizontalInput = Input.GetAxis("Horizontal");

                switch (selectHostMenu) {
                    case HostMenuType.TYPE:
                        hostMenuValueControllers[0].changeValueText(horizontalInput > 0 ? true : false);
                        break;
                    case HostMenuType.COLOR:
                        break;
                    case HostMenuType.COUNT:
                        hostMenuValueControllers[2].changeValueText(horizontalInput > 0 ? true : false);
                        break;
                }
            }
            return;
        }

        if (Input.GetButtonDown("Vertical")) {
            float verticalInput = Input.GetAxis("Vertical");
            if ((int)selectHostMenu > 2) {
                if (verticalInput > 0) {
                    SetSelectHostMenu(HostMenuType.COUNT);
                    return;
                }
                else {
                    SetSelectHostMenu(HostMenuType.TYPE);
                    return;
                }
            }
            else {
                if (verticalInput > 0) {
                    int menuNum = (int)selectHostMenu;
                    if (menuNum == 0) {
                        menuNum = 4;
                    }
                    else {
                        menuNum -= 1;
                    }
                    SetSelectHostMenu((HostMenuType)menuNum);
                    return;
                }
                else {
                    int menuNum = (int)selectHostMenu;
                    menuNum += 1;
                    SetSelectHostMenu((HostMenuType)menuNum);
                    return;
                }
            }
        }

        if (Input.GetButtonDown("Select") || Input.GetButtonDown("menu")) {
            if ((int)selectHostMenu == 3) {
                startHost();
            }
            else if((int)selectHostMenu == 4) {
                FindObjectOfType<OnlineMenuManager>().CloseHostMenu();
            }
            return;
        }

        CheckHoverImage();
    }

    public void SetSelectHostMenu(HostMenuType type) {
        selectHostMenu = type;
        CheckHoverImage();
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

    private void startHost() {
        FindObjectOfType<HostCreateController>().OpenCreateLoading();
    }
}
