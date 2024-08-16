using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinFailController : MonoBehaviour
{
    private Text description;

    private void Awake() {
        description = GetComponentInChildren<Text>();    
    }

    public void SetDescription(bool isColor) {
        if (isColor) {
            description.text = "This color is already being used by other users\n\nPlease set it again";
        }
        else {
            description.text = "Connection failed. Please Retry Again";
        }
    }
}
