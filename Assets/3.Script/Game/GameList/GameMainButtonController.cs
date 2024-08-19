using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;

public class GameMainButtonController : MonoBehaviour, IPointerEnterHandler {
    
    private Image[] images;
    private Button button;

    private GameMainListController gameMainList;

    private void Start() {
        button = GetComponent<Button>();
        images = GetComponentsInChildren<Image>();
        gameMainList = FindObjectOfType<GameMainListController>();
        CloseCrown();

    }

    public void OpenCrown() {
        images[2].enabled = true;
    }

    public void CloseCrown() {
        images[2].enabled = false;
    }

    public void EnableOutline() {
        images[0].enabled = true;
    }

    public void DisableOutline() {
        images[0].enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameMainList.CheckHoverIndex(gameObject.name);
    }

    public void SetInteractable(bool yn) {
        button.interactable = yn;
        if (!yn) {
            images[0].color= HexToRGB("E3B49D", yn);
        }
        else {
            images[0].color = HexToRGB("FF864D", yn);
        }
    }

    private Color32 HexToRGB(string hexCode, bool yn) {
        byte r = byte.Parse(hexCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hexCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hexCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        if (yn) {
            return new Color32(r, g, b, 255);
        }
        else {
            return new Color32(r, g, b, 135);
        }
    }
}
