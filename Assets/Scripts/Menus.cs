using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    private GameObject _pauseMenu;
    private GameObject _pauseButton;

    private void Awake()
    {
        _pauseMenu = GameManager.Instance.PauseMenu;
        _pauseButton = GameManager.Instance.PauseButton;
    }

    public void PauseTheGame()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0f;
            _pauseMenu.SetActive(true);
            _pauseButton.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            _pauseMenu.SetActive(false);
            _pauseButton.SetActive(true);
        }
    } 

    public void Victory()
    {

    }

    public void Defeat()
    {

    }
}
