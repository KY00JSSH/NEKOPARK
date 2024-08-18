using UnityEngine;

public class GameMainListController : MonoBehaviour {
    private GameMainButtonController[] buttons;
    private GameListUIManager gameListUIManager;

    private void Awake() {
        buttons = FindObjectsOfType<GameMainButtonController>();
        gameListUIManager = FindObjectOfType<GameListUIManager>();
    }
    private void OnEnable() {
        if (LoadDataManager.instance != null) {

            bool completeFirst = false;
            bool completeSecond = false;
            bool completeThird = false;

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
        else {
            for (int i = 0; i < 3; i++) {
                buttons[i].CloseCrown();
            }
        }
    }


    public void CheckHoverIndex(string name) {
        if (name.Contains("01")) {
            buttons[0].EnableOutline();
            buttons[1].DisEnableOutline();
            buttons[2].DisEnableOutline();
        }
        else if (name.Contains("02")) {
            buttons[0].DisEnableOutline();
            buttons[1].EnableOutline();
            buttons[2].DisEnableOutline();
        }
        else {
            buttons[0].DisEnableOutline();
            buttons[1].DisEnableOutline();
            buttons[2].EnableOutline();
        }
    }

    public void OpenStage(int index) {
        gameListUIManager.OpenDetailList();
    }
}
