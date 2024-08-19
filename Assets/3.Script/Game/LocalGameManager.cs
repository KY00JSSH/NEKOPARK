using UnityEngine;

public class LocalGameManager : MonoBehaviour {
    [SerializeField] private GameObject localPlayer;
    [SerializeField] private GameObject[] boxList;

    private void Start() {
        if (GameListManager.instance.IsLocalGame) {
            Instantiate(localPlayer, gameObject.transform);

            if (GameListManager.instance.MajorStageIndex == 0 && GameListManager.instance.MinorStageIndex == 1) {
                for (int i = 0; i < boxList.Length; i++) {
                    boxList[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
