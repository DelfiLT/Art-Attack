using SuperMaxim.Messaging;
using UnityEngine;

public class player : MonoBehaviour
{
    private float currentVelocity;
    private float currentJumpForce;

    [Header("Default values")]
    [SerializeField] private float defaultVelocity = 8;
    [SerializeField] private float defaultJumpForce = 18;
    [SerializeField] private float defaultGravity = 5;

    [Header("Water values")]
    [SerializeField] private float waterSpeedReduction = 3;

    private float movX;

    private bool isGrounded;
    private bool isSwimming;
    private bool doubleJump;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetDefaultValues();
    }

    void Update() 
    {
        Movement();
        Jump();
        Flip();
        ChangeElement();
    }

    private void SetDefaultValues()
    {
        rb.gravityScale = defaultGravity;
        currentJumpForce = defaultJumpForce;
        currentVelocity = defaultVelocity;
    }

    private void SetWaterValues()
    {
        rb.gravityScale = defaultGravity / waterSpeedReduction;
        currentJumpForce = defaultJumpForce / waterSpeedReduction;
        currentVelocity /= waterSpeedReduction;
        
        rb.velocity /= waterSpeedReduction;
    }

    void Movement()
    {
        movX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movX * currentVelocity, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded || isSwimming)
            {
                rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
                isGrounded = false;
                doubleJump = true;
            } else if (doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isSwimming = true;
            SetWaterValues();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isSwimming = false;
            SetDefaultValues();
        }
    }

    void ChangeElement()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var message = new ElementChange(Element.Ice);

            Messenger.Default.Publish(message);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var message = new ElementChange(Element.Water);

            Messenger.Default.Publish(message);
        }
    }
}
