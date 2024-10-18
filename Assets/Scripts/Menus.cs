using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    private GameObject _pauseObject;
    private GameObject _playObject;

    private void Awake()
    {
        _pauseObject = GameManager.Instance.PauseMenu;
        _playObject = GameManager.Instance.PauseButton;
    }

    public void PauseTheGame()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0f;
            _pauseObject.SetActive(true);
            _playObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            _pauseObject.SetActive(false);
            _playObject.SetActive(true);
        }
    } 

    public void Victory()
    {

    }

    public void Defeat()
    {

    }
}
