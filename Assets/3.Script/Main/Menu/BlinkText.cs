using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {
    //∏ﬁ¿Œ ≈ÿΩ∫∆Æ ±Ù∫˝¿”

    private Text blinkText;

    private void Start() {
        blinkText = gameObject.GetComponent<Text>();
        StartCoroutine(BlinkTextCo());
    }

    private IEnumerator BlinkTextCo() {
        while (true) {
            blinkText.text = "";
            yield return new WaitForSeconds(.5f);
            blinkText.text = "PRESS ENTER KEY";
            yield return new WaitForSeconds(.5f);
        }
    }
}
