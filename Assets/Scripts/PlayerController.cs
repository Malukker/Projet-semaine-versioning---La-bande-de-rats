using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Public
    [Header("References")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    [Header("Gameplay")]
    [SerializeField] float speed = 5;


    // Private
    bool canMoove = true;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        // float xVelocity = Input.GetAxisRaw("Horizontal");
        // float yVelocity = Input.GetAxisRaw("Vertical");

        float xVelocity = Input.GetAxis("Horizontal");
        float yVelocity = Input.GetAxis("Vertical");

        if (canMoove)
        {
            rigidbody.velocity = Vector2.ClampMagnitude(new Vector2(xVelocity * speed, yVelocity * speed), speed);
        }

        // animator.SetFloat("xVelocityAbs", Mathf.Abs(rb.velocity.x));
        // animator.SetFloat("yVelocity", rb.velocity.y);

        // if (rb.velocity.x < 0) spriteRenderer.flipX = true;
        // else if (rb.velocity.x > 0) spriteRenderer.flipX = false;
    }

    public void SetCanMoove(bool value)
    {
        canMoove = value;
    }
}
