using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _player;
    public GameObject Player {  get { return _player; } } 

    public void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(Instance);

        Instance = this;

        _player = GameObject.FindGameObjectWithTag("Player");
    }
}
