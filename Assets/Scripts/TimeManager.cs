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

    // Start is called before the first frame update
    void Start()
    {
        time = maxTime;
        Destroy(this, maxTime);
    }

    private void Update()
    {
        time -= Time.deltaTime;
        UpdateTimer?.Invoke(time);
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        OnLose?.Invoke();
    }
}
