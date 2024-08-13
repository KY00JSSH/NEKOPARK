using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;  

public class PlayerKeyMapper : MonoBehaviour
{
    public string actionName = "Horizontal_2P"; // Action name to remap
    public string newPositiveButton = "right"; // New positive button
    public string newNegativeButton = "left"; // New negative button

    void Start() {
        RemapInput(actionName, newPositiveButton, newNegativeButton);
    }

    void RemapInput(string actionName, string positiveButton, string negativeButton) {
        // Access the private InputManager type and instance via reflection
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
                // Set new positive and negative button values
                axis.GetType().GetField("positiveButton", BindingFlags.Public | BindingFlags.Instance).SetValue(axis, positiveButton);
                axis.GetType().GetField("negativeButton", BindingFlags.Public | BindingFlags.Instance).SetValue(axis, negativeButton);

                Debug.Log($"Remapped Action '{actionName}': Positive Button -> {positiveButton}, Negative Button -> {negativeButton}");
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
