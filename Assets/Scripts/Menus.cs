using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject _pauseObject;
    [SerializeField] private GameObject _playObject;
    [SerializeField] private TMP_Text _timeText;

    private void Awake()
    {
        VictoryZone.OnVictory += Victory;
        TimeManager.OnLose += Defeat;
        TimeManager.UpdateTimer += UpdateTime;
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
        SceneManager.LoadScene("VictoryScreen");
    }

    public void Defeat()
    {
        SceneManager.LoadScene("DefeatScreen");
    }

    public void UpdateTime(float i)
    {
        TimeSpan time = TimeSpan.FromSeconds(i);
        _timeText.text = time.ToString(@"mm\:ss");
    }
}
