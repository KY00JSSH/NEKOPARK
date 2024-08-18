using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameMainButtonController : MonoBehaviour, IPointerEnterHandler {
    private Image[] images;

    private GameMainListController gameMainList;

    private void Awake() {
        images = GetComponentsInChildren<Image>();
        gameMainList = FindObjectOfType<GameMainListController>();
    }

    private void openCrown() {
        images[2].enabled = true;
    }

    private void closeCrown() {
        images[2].enabled = false;
    }

    public void EnableOutline() {
        images[0].enabled = true;
    }

    public void DisEnableOutline() {
        images[0].enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameMainList.CheckHoverIndex(gameObject.name);
    }
}
