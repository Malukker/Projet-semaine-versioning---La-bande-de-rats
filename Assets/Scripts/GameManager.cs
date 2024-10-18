using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _player;
    [SerializeField] private GameObject _pauseMenu, _pauseButton;

    public GameObject Player {  get { return _player; } }
    public GameObject PauseMenu { get { return _pauseMenu; } }
    public GameObject PauseButton { get { return _pauseButton; } }

    public void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(Instance);

        Instance = this;

        _player = GameObject.FindGameObjectWithTag("Player");
    }
}
