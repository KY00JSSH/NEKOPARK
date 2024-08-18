using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListManager : MonoBehaviour {
    public static GameListManager instance = null;
    private bool isSingle = true;
    public bool IsSingle { get { return isSingle; } }
    public void SetIsSingleMode(bool yn) { isSingle = yn; }

    private bool[,] stageData = new bool[3, 4];

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    private void stageDataiInit() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 4; j++) {
                stageData[i, j] = false;
            }
        }
    }

}