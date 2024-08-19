using UnityEngine;
using UnityEngine.UI;

public enum OnlineMenuType {
    PUBLIC = 0,
    FREIND,
    HOST,
    OPTION
}
public class OnlineMenuManager : MonoBehaviour {
    private Button[] buttons;
    private OnlineMenuType onMenuType;
    //private int buttonHover = 0;
    private bool isMenuSelect = false;
    private bool isServerFail = false;

    private MainManager mainManager;
    private HostMenuController hostMenuController;
    private HostLoadConfirmController hostLoadConfirm;
    private HostLoadListController hostLoadList;

    public void ServerFailFlagUpdate(bool yn) {
        isServerFail = yn;
    }

    public bool GetServerFailFlag() {
        return isServerFail;
    }

    private void Awake() {
        buttons = GetComponentsInChildren<Button>();
        mainManager = FindObjectOfType<MainManager>();
        hostMenuController = FindObjectOfType<HostMenuController>();
        hostLoadConfirm = FindObjectOfType<HostLoadConfirmController>();
        hostLoadList = FindObjectOfType<HostLoadListController>();

        setMenuType(OnlineMenuType.PUBLIC);
        hostMenuController.gameObject.SetActive(false);
        hostLoadConfirm.gameObject.SetActive(false);
        hostLoadList.gameObject.SetActive(false);        
    }

    private void Start()        //BGM 재생을 위해 추가 240819
    {
        AudioManager.instance.StopBGM(0);
        AudioManager.instance.PlayBGM(AudioManager.Bgm.MultiRoomBGM, 0);
    }

    public void setMenuType(OnlineMenuType type) {
        onMenuType = type;
        checkButtonOutline();
    }

    private void Update() {
        if (!isServerFail && !isMenuSelect && Input.GetButtonDown("Horizontal")) {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput > 0) {
                int selectMenuNum = (int)onMenuType + 1;
                if (selectMenuNum > 3) {
                    selectMenuNum = 0;
                }
                setMenuType((OnlineMenuType)selectMenuNum);
            }
            else {
                int selectMenuNum = (int)onMenuType - 1;
                if (selectMenuNum < 0) {
                    selectMenuNum = 3;
                }
                setMenuType((OnlineMenuType)selectMenuNum);
            }
            return;
        }

        if (!isServerFail && !isMenuSelect && Input.GetButtonDown("Vertical")) {
            int thisMenuNum = (int)onMenuType;
            if (thisMenuNum < 2) {
                int selectMenuNum = thisMenuNum + 2;
                setMenuType((OnlineMenuType)selectMenuNum);
            }
            else {
                int selectMenuNum = thisMenuNum - 2;
                setMenuType((OnlineMenuType)selectMenuNum);
            }
            return;
        }

        if ((!isServerFail && !isMenuSelect && Input.GetButtonDown("Select")) || (!isMenuSelect && Input.GetButtonDown("menu"))) {
            GoMenu();
            return;
        }
    }

    private void checkButtonOutline() {
        if (isMenuSelect)
            return;

        buttons[0].image.enabled = false;
        buttons[1].image.enabled = false;
        buttons[2].image.enabled = false;
        buttons[3].image.enabled = false;

        switch (onMenuType) {
            case OnlineMenuType.PUBLIC:
                buttons[0].image.enabled = true;
                break;
            case OnlineMenuType.FREIND:
                buttons[1].image.enabled = true;
                break;
            case OnlineMenuType.HOST:
                buttons[2].image.enabled = true;
                break;
            case OnlineMenuType.OPTION:
                buttons[3].image.enabled = true;
                break;
        }
    }

    public void GoMenu() {
        isMenuSelect = true;
        switch (onMenuType) {
            case OnlineMenuType.PUBLIC:
                mainManager.OpenJoinCanvas();
                break;
            case OnlineMenuType.FREIND:
                mainManager.OpenJoinCanvas();
                break;
            case OnlineMenuType.HOST:
                hostLoadConfirm.gameObject.SetActive(true);
                break;
            case OnlineMenuType.OPTION:
                mainManager.OpenDLCDialog();
                break;
        }
    }

    public void OpenHostMenu() {
        hostMenuController.gameObject.SetActive(true);
        CloseHostConfirm();
        isMenuSelect = true;
    }

    public void CloseHostMenu() {
        hostMenuController.gameObject.SetActive(false);
        isMenuSelect = false;
    }

    public void CloseHostConfirm() {
        hostLoadConfirm.gameObject.SetActive(false);
    }

    public void OpenLoadList() {
        CloseHostConfirm();
        hostLoadList.GetLoadList();
    }

    public void CloseLoadList() {
        hostLoadList.gameObject.SetActive(false);
    }
}
