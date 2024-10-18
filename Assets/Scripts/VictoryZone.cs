using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class VictoryZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoxCollider2D victoryZone;

    public static event Action OnVictory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            OnVictory?.Invoke();
        }
    }
}
