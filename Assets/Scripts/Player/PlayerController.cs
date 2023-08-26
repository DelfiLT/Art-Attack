using SuperMaxim.Messaging;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float currentVelocity;
    private float currentJumpForce;

    [Header("General Settings")]
    [SerializeField] private bool canDoubleJump;

    [Header("Default values")]
    [SerializeField] private float defaultVelocity = 8;
    [SerializeField] private float defaultJumpForce = 18;
    [SerializeField] private float defaultGravity = 5;
    [SerializeField] private float defaultCooldown = 5;

    [Header("Water values")]
    [SerializeField] private float waterSpeedReduction = 3;

    private float movX;

    private bool isGrounded;
    private bool isSwimming;
    private bool doubleJump;

    EarthPower earthPower;
    IcePower icePower;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetDefaultValues();

        earthPower = GetComponent<EarthPower>();
        icePower = GetComponent<IcePower>();
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
            } else if (canDoubleJump && doubleJump)
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
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (movX < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void ChangeElement()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !earthPower.CanUse())
        {
            earthPower.Use();
            Messenger.Default.Publish(new PowerCooldownMessage(earthPower.CooldownTime, PowerType.Earth));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !icePower.CanUse())
        {
            icePower.Use();
            Messenger.Default.Publish(new PowerCooldownMessage(icePower.CooldownTime, PowerType.Ice));
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJump = false;
        }
    }
}
