using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinHintController : MonoBehaviour
{
    private void Update() {
        if(Input.GetButtonDown("Select") || Input.GetButtonDown("menu")){
            clickOkButton();
        }
    }

    public void clickOkButton() {
        loadOnlineScene();
    }

    private void loadOnlineScene() {
        SceneManager.LoadScene("Feat_NetworkGameRoom");
    }
}
