using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCurrentJoystick : MonoBehaviour
{
    public Slider rightJoystickSensitivity;
    public Slider leftJoystickSensitivity;
    public Slider rightJoystickDeadzone;
    public Slider leftJoystickDeadzone;
    public Text rightJoystickSensitivityText;
    public Text leftJoystickSensitivityText;
    public Text rightJoystickDeadzoneText;
    public Text leftJoystickDeadzoneText;
    
    
    public void Update() {
        rightJoystickSensitivityText.text = rightJoystickSensitivity.value.ToString("#.#");
        leftJoystickSensitivityText.text = leftJoystickSensitivity.value.ToString("#.#");
        rightJoystickDeadzoneText.text = rightJoystickDeadzone.value.ToString("#.##");
        leftJoystickDeadzoneText.text = leftJoystickDeadzone.value.ToString("#.##");
    }
}
