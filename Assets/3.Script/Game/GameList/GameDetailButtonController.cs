using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameDetailButtonController : MonoBehaviour, IPointerEnterHandler {
    private Image crown;

    private Button button;
    private Image outline;
    private Text numText;
    private Outline numOutline;

    private GameDetailListController gameDetailList;

    private void Awake() {
        crown = GetComponentInChildren<Image>();
        button = GetComponentInChildren<Button>();

        outline = button.gameObject.GetComponentsInChildren<Image>()[1];
        numText = button.gameObject.GetComponentInChildren<Text>();
        numOutline = numText.gameObject.GetComponentInChildren<Outline>();
        gameDetailList = FindObjectOfType<GameDetailListController>();
    }

    private void Start() {
        DisEnableOutline();
    }

    public void OpenStage() {
        openCrown();
        button.interactable = true;

        numOutline.effectColor = HexToRGB("FF864D");
        numText.color = HexToRGB("FF864D");
    }

    public void CloseStage() {
        closeCrown();
        button.interactable = false;

        numOutline.effectColor = HexToRGB("E3B49D");
        numText.color = HexToRGB("E3B49D");
    }

    private Color32 HexToRGB(string hexCode) {
        byte r = byte.Parse(hexCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hexCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hexCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r, g, b, 255);
    }

    private void openCrown() {
        crown.enabled = true;
    }

    private void closeCrown() {
        crown.enabled = false;
    }

    public void EnableOutline() {
        outline.enabled = true;
    }

    public void DisEnableOutline() {
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameDetailList.CheckHoverIndex(gameObject.name);
    }
}
