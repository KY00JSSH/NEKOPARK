using UnityEngine;

public class LocalGameManager : MonoBehaviour {
    [SerializeField] private GameObject localPlayer;

    private void Start() {
        Instantiate(localPlayer, gameObject.transform);
    }
}
