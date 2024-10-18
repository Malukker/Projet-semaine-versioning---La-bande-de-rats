using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    new Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    GameObject hat;

    [Header("General")]
    [SerializeField] bool smoothMovement;
    [SerializeField] float speed = 5;

    bool paused = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hat = GetComponentsInChildren<SpriteRenderer>().Where(component => component.gameObject != this.gameObject).First().gameObject;
        Menus.OnPause += SetPause;
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

        if (!paused)
        {
            rigidbody.velocity = Vector2.ClampMagnitude(new Vector2(xVelocity * speed, yVelocity * speed), speed);

        }

        if (Mathf.Abs(xVelocity) > Mathf.Abs(yVelocity))
        {
            if (Input.GetAxis("Horizontal") < 0)
            { 
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);   
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }

        // spriteRenderer.flipX = (Input.GetAxis("Horizontal") > 0);
        // spriteRenderer.flipY = (Input.GetAxis("Vertical") > 0);
        // animator.SetBool("Sideways", Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(Input.GetAxis("Horizontal")));

        if (Input.GetKeyDown(KeyCode.Space) && !paused)
        {
            SendHat();
        }
    }

    void SendHat()
    {
        if (hat)
        {
            hat.transform.SetParent(null, true);

            Vector2 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
            Quaternion direction = Quaternion.LookRotation(Vector3.forward, offset); // * Quaternion.Euler(0, 0, 90);
            hat.transform.SetPositionAndRotation(hat.transform.position, direction);

            hat.GetComponent<HatProjectile>().StartMoving();
            hat = null;
        }
    }

    void SetPause(bool value)
    {
        paused = value;
    }
}
