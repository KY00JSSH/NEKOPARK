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
    [SerializeField] private GameObject[] valueObjects;
    private HostCreateController hostCreateController;

    private void Awake() {
        SetSelectHostMenu(HostMenuType.TYPE);
        hostCreateController = FindObjectOfType<HostCreateController>();
    }

    private void Update() {
        //가로 이동
        if (Input.GetButtonDown("Horizontal")) {
            if ((int)selectHostMenu > 2) {
                SetSelectHostMenu((int)selectHostMenu == 3 ? HostMenuType.CANCEL : HostMenuType.CREATE);
            }
            else {
                float horizontalInput = Input.GetAxis("Horizontal");
                valueObjects[(int)selectHostMenu]?.GetComponent<HostMenuValueController>().
                    ChangeValueText(horizontalInput > 0);
            }
            return;
        }

        //세로 이동
        if (Input.GetButtonDown("Vertical")) {
            float verticalInput = Input.GetAxis("Vertical");
            if ((int)selectHostMenu > 2) {
                if (verticalInput < 0) {
                    SetSelectHostMenu(HostMenuType.COUNT);
                    return;
                }
                else {
                    SetSelectHostMenu(HostMenuType.TYPE);
                    return;
                }
            }
            else {
                if (verticalInput < 0) {
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

        //메뉴 선택
        if (Input.GetButtonDown("Select") || Input.GetButtonDown("menu")) {
            if ((int)selectHostMenu == 3) {
                StartHost();
            }
            else if ((int)selectHostMenu == 4) {
                FindObjectOfType<OnlineMenuManager>().CloseHostMenu();
            }
            return;
        }

        //마우스 오버 체크
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

    public void StartHost() {
        int gameTypeValue = valueObjects[1].GetComponent<HostMenuValueController>().GetValueNum();
        PlayerPrefs.SetInt("GameType", gameTypeValue);
        
        int colorValue = valueObjects[1].GetComponent<HostMenuValueController>().GetColorIndex();
        PlayerPrefs.SetInt("HostColor", colorValue);

        int countValue = valueObjects[2].GetComponent<HostMenuValueController>().GetValueNum();
        PlayerPrefs.SetInt("MaxPlayer", countValue+2);

        hostCreateController.OpenCreateLoading();
    }
}
