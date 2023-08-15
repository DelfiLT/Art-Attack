using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private float jumpForce;
    private float movX;

    private bool isGrounded;
    private bool doubleJump;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        Movement();
        Jump();
        Flip();
    }

    void Movement()
    {
        movX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movX * velocity, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isGrounded = false;
                doubleJump = true;
            } else if (doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                doubleJump = false;
            }
        }
    }

    void Flip()
    {
        if (movX > 0)
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
        }
        if (movX < 0)
        {
            transform.localScale = new Vector3(-2f, 2f, 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJump = false;
        }
    }
}
