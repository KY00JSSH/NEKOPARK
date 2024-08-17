using UnityEngine;
using UnityEngine.UI;

public class InfoDetailController : MonoBehaviour {
    private Text[] texts;

    private void Awake() {
        texts = GetComponentsInChildren<Text>();
    }

    private void OnEnable() {
        if (SQLManager.instance.UserData != null) {
            for (int i = 0; i < texts.Length; i++) {
                if (texts[i].name.Equals("ID")) {
                    texts[i].text = SQLManager.instance.UserData.userId;
                }
                else if (texts[i].name.Equals("NickName")) {
                    texts[i].text = SQLManager.instance.UserData.userNick;
                }
            }
        }
    }
}
