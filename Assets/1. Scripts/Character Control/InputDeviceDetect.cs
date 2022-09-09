using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceDetect : MonoBehaviour
{

    public PlayerInput playerInput;
    public Player player;
    

    public enum CurrentDevice {
        MouseKeyboard,
        Gamepad
    }

    public CurrentDevice currentDevice = CurrentDevice.MouseKeyboard;

    private void Update() {

        if(playerInput.currentControlScheme == "MouseKeyboard") {
            currentDevice = CurrentDevice.MouseKeyboard;
            player.cameraSensX = player.cameraSensMouseX;
            player.cameraSensY = player.cameraSensMouseY;
            
        } else {
            currentDevice = CurrentDevice.Gamepad;
            player.cameraSensX = player.cameraSensGamepadX;
            player.cameraSensY = player.cameraSensGamepadY;
            
        }


    }

}
