using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //SceneManager sceneManager = new SceneManager();
    public enum Gamestate {
        Start,
        Gameplay,
        Win,
    }

    public Gamestate currentGamestate = Gamestate.Start;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 144;

    }


    public GameObject startScreen;
    public GameObject winScreen;
    public GameObject pauseScreen;

    public void Accept() {
        if(currentGamestate == Gamestate.Start) {
            Destroy(startScreen);
            currentGamestate = Gamestate.Gameplay;
            playerInput.SwitchCurrentActionMap("Gameplay");
        }


        if(currentGamestate == Gamestate.Win) {
            Application.Quit();

            //SceneManager.LoadScene(0);
        }
    }


    public void WinGame() {
        winScreen.SetActive(true);
        currentGamestate = Gamestate.Win;
        playerInput.SwitchCurrentActionMap("Menu");
    }

    public void Pause() {
        pauseScreen.SetActive(true);
        playerInput.SwitchCurrentActionMap("Pause");
    }

    public void Continue() {
        pauseScreen.SetActive(false);
        playerInput.SwitchCurrentActionMap("Gameplay");
    }

}
