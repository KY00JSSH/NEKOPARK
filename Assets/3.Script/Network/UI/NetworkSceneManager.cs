using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : MonoBehaviour
{

    public void LoadMainScene() {
        SceneManager.LoadScene("Main");
    }
}
