using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public FilterToggle ft;
    // Start is called before the first frame update
    float previousTimeScale = 1;
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject optionsMarker;
    public GameObject PSIButton;

    public GameObject initialOptionsPanel;
   
    bool pauseActivated;

    public bool isPaused;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
      

    }

    public void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPaused = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            pausePanel.SetActive(false);
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnOptionsClick()
    {
        optionsPanel.transform.position = optionsMarker.transform.position;
    }
    public void LeavingOptions()
    {
        optionsPanel.transform.position = initialOptionsPanel.transform.position;
    }

    public void OnMMenuClick()
    {
        Application.Quit();

    }
    public void OnPS1()
    {
        if(ft.isOldTyme == false)
        {
            ft.renderTextureMode(true);
            ft.isOldTyme = true;
        }
        else
        {
            ft.renderTextureMode(false);
            ft.isOldTyme = false;
        }
    }
}
