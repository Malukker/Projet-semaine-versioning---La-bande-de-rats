using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] int maxTime;
    float time;

    public static event Action<float> UpdateTimer;
    public static event Action OnLose;

    void Start()
    {
        time = maxTime;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        UpdateTimer?.Invoke(time);
        
        if (time <= 0)
        {
            TimedOut();
        }
    }

    void TimedOut()
    {
        OnLose?.Invoke();
        Destroy(gameObject);
    }
}
