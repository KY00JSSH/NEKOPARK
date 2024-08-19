using UnityEngine;

public class LocalGameManager : MonoBehaviour {
    [SerializeField] private GameObject localPlayer;

    private void Start() {
        if (PlayerPrefs.GetInt("localGame") == 1)
            Instantiate(localPlayer, gameObject.transform);
    }
}
