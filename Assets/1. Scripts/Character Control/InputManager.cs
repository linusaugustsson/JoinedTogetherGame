using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public Vector2 moveVal;
    public Vector2 lookVal;

    public CharacterController characterController;

    public BodyPartManager bodyPartManager;

    public GameManager gameManager;

    public bool hasDoubleJump = false;
    public bool hasGlide = false;
    public bool hasShoot = false;


    public void OnJump() {
        characterController.Jump();
    }

    public void OnAttack() {
        characterController.Attack();
    }

    public void OnInteract() {

    }

    public void OnGlide() {
        if(hasGlide == true) {
            characterController.Glide();
        }

    }

    public void OnMove(InputValue _value) {
        moveVal = _value.Get<Vector2>();
    }

    public void OnLook(InputValue _value) {
        lookVal = _value.Get<Vector2>();
    }


    public void OnChangeHead() {
        
        bodyPartManager.ChangeHead();
    }

    public void OnChangeTorso() {
        
        bodyPartManager.ChangeTorso();
    }

    public void OnChangeArms() {
        
        bodyPartManager.ChangeArms();
    }

    public void OnChangeLegs() {
        
        bodyPartManager.ChangeLegs();
    }

    public void OnAccept() {
        gameManager.Accept();
    }

    public void OnPause() {
        gameManager.Pause();
    }

    public void OnExit() {
        Application.Quit();
    }

    public void OnContinue() {
        gameManager.Continue();
    }


}
