using UnityEngine;

public class LocalGameManager : MonoBehaviour {
    [SerializeField] private GameObject networkGameCore;
    [SerializeField] private GameObject networkPlayer;
    [SerializeField] private GameObject localPlayer;

    private void Awake() {
        if (GameListUIManager.instance.IsLocalGame) {
            networkGameCore.gameObject.SetActive(false);
            if(networkPlayer!=null)
                networkPlayer.SetActive(false);
        }
    }

    private void Start() {
        Instantiate(localPlayer, gameObject.transform);
    }
}
