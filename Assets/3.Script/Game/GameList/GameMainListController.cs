using UnityEngine;
using Mirror;

public class GameMainListController : MonoBehaviour {
    [SerializeField] private GameObject[] gameButton;
    private GameMainButtonController[] buttons;
    private GameListUIManager gameListUIManager;

    private bool completeFirst = false;
    private bool completeSecond = false;
    private bool completeThird = false;

    private void Awake() {
        buttons = FindObjectsOfType<GameMainButtonController>();
        gameListUIManager = FindObjectOfType<GameListUIManager>();
    }
    private void Start() {
        foreach (var each in gameButton) each.SetActive(true);

    }
    private void OnEnable() {
        foreach (var each in gameButton) each.SetActive(true);
        if (LoadDataManager.instance != null) {

            bool[,] stageData = LoadDataManager.instance.StageData;

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (i == 0 && stageData[i, j]) {
                        completeFirst = true;
                    }
                    else if (i == 1 && stageData[i, j]) {
                        completeSecond = true;
                    }
                    else if (i == 2 && stageData[i, j]) {
                        completeThird = true;
                    }
                }
            }

            if(!completeFirst || !completeSecond && !completeThird) {
                for (int i = 0; i < 3; i++) {
                    buttons[i].CloseCrown();
                }
                buttons[1].SetInteractable(false);
                buttons[2].SetInteractable(false);
            }
            else {
                if (completeFirst) {
                    buttons[0].OpenCrown();
                }
                else {
                    buttons[0].CloseCrown();
                }

                if (completeSecond) {
                    buttons[1].OpenCrown();
                }
                else {
                    buttons[1].CloseCrown();
                }

                if (completeThird) {
                    buttons[2].OpenCrown();
                }
                else {
                    buttons[2].CloseCrown();
                }
            }
        }
        else {
            for (int i = 0; i < 3; i++) {
                buttons[i].CloseCrown();
            }
            buttons[1].SetInteractable(false);
            buttons[2].SetInteractable(false);
        }
    }

    public void CheckHoverIndex(string name) {
        for (int i = 0; i < 3; i++) {
            if (buttons[i].name.Equals(name)) {
                buttons[i].EnableOutline();
            }
            else {
                buttons[i].DisableOutline();
            }
        }
    }

    public void OpenStage(int index) {
        gameListUIManager.OpenDetailList(index);
    }
}
