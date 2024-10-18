using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _player;
    [SerializeField] private GameObject _pauseObject, _playObject;

    public GameObject Player {  get { return _player; } }
    public GameObject PauseMenu { get { return _pauseObject; } }
    public GameObject PauseButton { get { return _playObject; } }

    public void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(Instance);

        Instance = this;

        _player = GameObject.FindGameObjectWithTag("Player");
    }
}
