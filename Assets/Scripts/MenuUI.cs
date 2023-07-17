using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;

public class MenuUI : MonoBehaviour
{

    //Main Menu Animation and Functionality
    public Image menuHighlight;
    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject quitButton;
    public GameObject fullMenu;

    //Options Menu Animation and Functionality
    public GameObject optionsPanel;
    Vector3 initialOptionsPanel;

    //Start Game
    public GameObject startGameFader;
    public GameObject loadTrigger;
    

    /*Vector3 initialStart = new Vector3(-1050, -35);
    Vector3 initialOptions = new Vector3(-1050, -210);
    Vector3 initialQuit = new Vector3(-1050, -375);
    Vector3 initialHighlight = new Vector3(-2500, -200);*/

    private PlayableDirector director;
    public GameObject cutsceneManager;
    public GameObject textBackdrop;
    bool cutscenePlayed;

    Vector3 initialStart;
    Vector3 initialOptions;
    Vector3 initialQuit;
    Vector3 initialHighlight;
    Vector3 initialFullMenu;
    public void Awake()
    {
        director = cutsceneManager.GetComponent<PlayableDirector>();
        director.played += Director_Played;
        cutscenePlayed = false;

        //Making sure all UI elements go to their appropriate spots after cutscene
       initialStart = startButton.transform.position;
       initialOptions = optionsButton.transform.position;
       initialQuit = quitButton.transform.position;
       initialHighlight = menuHighlight.transform.position;
       initialFullMenu = fullMenu.transform.position;
       initialOptionsPanel = optionsPanel.transform.position;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || director.time >= director.playableAsset.duration)
        {
            if (cutscenePlayed == false)
            {
                Director_Played(director);
            }
        }

        if (startGameFader.transform.position == loadTrigger.transform.position)
        {
            LoadGame();
        }
    }

    private void LoadGame()
    {
        //Change to Level 1 once it's completed
        SceneManager.LoadScene("Level2Scene");
    }

    private void Director_Played(PlayableDirector obj)
    {
        fullMenu.transform.position = initialFullMenu;
        startButton.transform.position = initialStart;
        optionsButton.transform.position = initialOptions;
        quitButton.transform.position = initialQuit;
        cutsceneManager.SetActive(false);
        textBackdrop.SetActive(false);
        cutscenePlayed = true;
        Debug.Log(fullMenu.transform.position);
    }

    public void HoverStart()
    {
        menuHighlight.transform.position = startButton.transform.position;
        menuHighlight.transform.LeanMoveLocal(new Vector3(-920, -35), 0.05f);
        startButton.transform.LeanMoveLocal(new Vector3(-920, -35), 0.05f);

    }

    public void LeaveStart()
    {
        startButton.transform.position = initialStart;
        menuHighlight.transform.LeanMoveLocal(new Vector3(-1550,-35), 0.05f);
    }

    public void HoverOptions()
    {
        menuHighlight.transform.position = optionsButton.transform.position;
        menuHighlight.transform.LeanMoveLocal(new Vector3(-920, -210), 0.05f);
        optionsButton.transform.LeanMoveLocal(new Vector3(-920, -210), 0.05f);
    }

    public void LeaveOptions()
    {
        optionsButton.transform.position = initialOptions;
        menuHighlight.transform.LeanMoveLocal(new Vector3(-1550, -210), 0.05f);
    }

    public void HoverQuit()
    {
        menuHighlight.transform.position = quitButton.transform.position;
        menuHighlight.transform.LeanMoveLocal(new Vector3(-920, -375), 0.05f);
        quitButton.transform.LeanMoveLocal(new Vector3(-920, -375), 0.05f);
    }

    public void LeaveQuit()
    {
        quitButton.transform.position = initialQuit;
        menuHighlight.transform.LeanMoveLocal(new Vector3(-1550, -375), 0.05f);
    }

    public void OnOptionsClick()
    {
        optionsPanel.transform.LeanMoveLocal(new Vector3(0, 5), 0.25f);
    }

    public void LeavingOptions()
    {
        optionsPanel.transform.LeanMoveLocal(new Vector3(0, -1080), 0.25f);
    }

    public void OnStartClick()
    {
        startGameFader.transform.LeanMoveLocalY(0, 3.25f);
    }
}
