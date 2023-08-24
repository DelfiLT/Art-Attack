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

    [Header("Spawn")]
    [SerializeField] private GameObject earthBlock;
    [SerializeField] private Transform earthSpawn;

    private float movX;
    private float time;

    private bool isGrounded;
    private bool isSwimming;
    private bool doubleJump;
    private bool onBlock;

    Power earthPower;
    //Power icePower;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetDefaultValues();

        //earthPower = GetComponent<EarthPower>();

        time = 5;
    }

    void Update() 
    {
        Movement();
        Jump();
        Flip();

        time += Time.deltaTime;

        if(time >= defaultCooldown)
        {
            ChangeElement();
        }
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
            if(isGrounded || isSwimming || onBlock)
            {
                rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
                isGrounded = false;
                doubleJump = true;
                onBlock = false;
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
        if (Input.GetKeyDown(KeyCode.Alpha1) && earthPower.CanUse())
        {
            earthPower.Use();
            //Mover a nueva Clase EarthPower
            //GameObject newBlock = Instantiate(earthBlock, earthSpawn.position, earthSpawn.rotation);
            //time = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var message = new ElementChange(Element.Water);

            Messenger.Default.Publish(message);
            time = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Hacer lo mismo con IcePower
            var message = new ElementChange(Element.Ice);
            Messenger.Default.Publish(message);

            time = 0;
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
        if (collision.gameObject.CompareTag("Block"))
        {
            onBlock = true;
            isGrounded = true;
        }
    }
}
