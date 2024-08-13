using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class KeyTest : MonoBehaviour
{
    public string actionName = "Horizontal_2P"; // The action you want to display the key for
    public Text keyDisplayText; // UI Text element to show the key

    void Start() {
        PrintInputMappings(actionName);
    }

    void PrintInputMappings(string actionName) {
        // InputManager 클래스의 private 필드에 접근하기 위한 Reflection 사용
        var inputManagerType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InputManager");
        if (inputManagerType == null) {
            Debug.LogError("InputManager type not found!");
            return;
        }

        var inputManagerInstance = GetInputManagerInstance(inputManagerType);
        if (inputManagerInstance == null) {
            Debug.LogError("InputManager instance not found!");
            return;
        }

        var axes = GetAxes(inputManagerInstance);
        if (axes == null) {
            Debug.LogError("Axes array not found!");
            return;
        }

        foreach (var axis in axes) {
            var axisName = (string)axis.GetType().GetField("name", BindingFlags.Public | BindingFlags.Instance).GetValue(axis);
            if (axisName == actionName) {
                var positiveButton = (string)axis.GetType().GetField("positiveButton", BindingFlags.Public | BindingFlags.Instance).GetValue(axis);
                var negativeButton = (string)axis.GetType().GetField("negativeButton", BindingFlags.Public | BindingFlags.Instance).GetValue(axis);

                Debug.Log($"Action Name: {actionName}");
                Debug.Log($"Positive Button: {positiveButton}");
                Debug.Log($"Negative Button: {negativeButton}");
                return;
            }
        }

        Debug.LogWarning($"Action '{actionName}' not found in InputManager.");
    }

    private object GetInputManagerInstance(System.Type inputManagerType) {
        var instanceField = inputManagerType.GetField("instance", BindingFlags.NonPublic | BindingFlags.Static);
        return instanceField?.GetValue(null);
    }

    private object[] GetAxes(object inputManagerInstance) {
        var axesField = inputManagerInstance.GetType().GetField("m_Axes", BindingFlags.NonPublic | BindingFlags.Instance);
        return axesField?.GetValue(inputManagerInstance) as object[];
    }
}
