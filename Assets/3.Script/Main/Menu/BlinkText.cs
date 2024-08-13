using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {
    private Text blinkText;

    private void Start() {
        blinkText = gameObject.GetComponent<Text>();
        StartCoroutine(BlinkTextCo());
    }

    public IEnumerator BlinkTextCo() {
        while (true) {
            blinkText.text = "";
            yield return new WaitForSeconds(.5f);
            blinkText.text = "PRESS ENTER KEY";
            yield return new WaitForSeconds(.5f);
        }
    }
}
