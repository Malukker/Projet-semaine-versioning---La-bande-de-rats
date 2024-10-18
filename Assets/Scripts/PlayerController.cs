using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] GameObject hat;

    [Header("General")]
    [SerializeField] bool smoothMovement;
    [SerializeField] float speed = 5;


    bool canMoove = true;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hat = GetComponentsInChildren<SpriteRenderer>().Where(component => component.gameObject != this.gameObject).First().gameObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        float xVelocity;
        float yVelocity;

        if (smoothMovement)
        {
             xVelocity = Input.GetAxis("Horizontal");
             yVelocity = Input.GetAxis("Vertical");
        }
        else
        {
             xVelocity = Input.GetAxisRaw("Horizontal");
             yVelocity = Input.GetAxisRaw("Vertical");
        }

        if (canMoove)
        {
            rigidbody.velocity = Vector2.ClampMagnitude(new Vector2(xVelocity * speed, yVelocity * speed), speed);
        }

        // animator.SetFloat("xVelocityAbs", Mathf.Abs(rb.velocity.x));
        // animator.SetFloat("yVelocity", rb.velocity.y);

        // if (rb.velocity.x < 0) spriteRenderer.flipX = true;
        // else if (rb.velocity.x > 0) spriteRenderer.flipX = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendHat();
        }
    }

    void SendHat()
    {
        if (hat)
        {
            hat.transform.SetParent(null, true);
            hat.transform.localScale = Vector3.one;

            Vector2 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
            Quaternion direction = Quaternion.LookRotation(Vector3.forward, offset); // * Quaternion.Euler(0, 0, 90);
            hat.transform.SetPositionAndRotation(hat.transform.position, direction);

            hat.GetComponent<HatProjectile>().StartMoving();
            hat = null;
        }
    }

    public void SetCanMoove(bool value)
    {
        canMoove = value;
    }
}
