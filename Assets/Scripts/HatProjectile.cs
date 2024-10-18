using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class HatProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new CircleCollider2D collider;

    [Header("General")]
    [SerializeField]
    float speed = 3;

    bool move = false;

    public static event Action<HatProjectile> HitWall;

    void Awake()
    {
        if(TryGetComponent<Rigidbody2D>(out rigidbody)) {}
        else
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (move && rigidbody)
        {
            rigidbody.velocity = speed * transform.up;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            OnHitWall(transform.position);
            collider.isTrigger = true;
            move = false;
            Destroy(rigidbody);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("^^^^^^ CHANGE CONDITION TO ENEMY ABOVE ^^^^^^");
            // Add hat to enemy (cat) -> must have rb2d to generate trigger event
        }
    }

    public void StartMoving()
    {
        move = true;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    void OnHitWall(Vector2 position)
    {
        HitWall?.Invoke(this);
        Debug.Log("hit wall");
    }
}
