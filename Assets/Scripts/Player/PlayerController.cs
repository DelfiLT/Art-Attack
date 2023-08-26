using SuperMaxim.Messaging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerController : MonoBehaviour, IPlayerController
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

    //void Update() 
    //{

    //    //Movement();
    //    //Jump();
    //    //Flip();
    //    //ChangeElement();
    //}

    public Vector3 Velocity { get; private set; }
    public FrameInput Input { get; private set; }
    public bool JumpingThisFrame { get; private set; }
    public bool LandingThisFrame { get; private set; }
    public Vector3 RawMovement { get; private set; }
    public bool Grounded => _colDown;

    private Vector3 _lastPosition;
    private float _currentHorizontalSpeed, _currentVerticalSpeed;

    // This is horrible, but for some reason colliders are not fully established when update starts...
    private bool _active;
    void Awake() => Invoke(nameof(Activate), 0.5f);
    void Activate() => _active = true;

    private void Update()
    {
        if (!_active) return;
        // Calculate velocity
        Velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;

        GatherInput();
        RunCollisionChecks();

        CalculateWalk(); // Horizontal movement
        CalculateJumpApex(); // Affects fall speed, so calculate before gravity
        CalculateGravity(); // Vertical movement
        CalculateJump(); // Possibly overrides vertical

        MoveCharacter(); // Actually perform the axis movement
    }

    private void GatherInput()
    {
        Input = new FrameInput
        {
            JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
            JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
            X = UnityEngine.Input.GetAxisRaw("Horizontal")
        };
        if (Input.JumpDown)
        {
            _lastJumpPressed = Time.time;
        }
    }

    [Header("COLLISION")][SerializeField] private Bounds _characterBounds;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _detectorCount = 3;
    [SerializeField] private float _detectionRayLength = 0.1f;
    [SerializeField][Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

    private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
    private bool _colUp, _colRight, _colDown, _colLeft;

    private float _timeLeftGrounded;

    // We use these raycast checks for pre-collision information
    private void RunCollisionChecks()
    {
        // Generate ray ranges. 
        CalculateRayRanged();

        // Ground
        LandingThisFrame = false;
        var groundedCheck = RunDetection(_raysDown);
        if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
        else if (!_colDown && groundedCheck)
        {
            _coyoteUsable = true; // Only trigger when first touching
            LandingThisFrame = true;
        }

        _colDown = groundedCheck;

        // The rest
        _colUp = RunDetection(_raysUp);
        _colLeft = RunDetection(_raysLeft);
        _colRight = RunDetection(_raysRight);

        bool RunDetection(RayRange range)
        {
            return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
        }
    }

    private void CalculateRayRanged()
    {
        // This is crying out for some kind of refactor. 
        var b = new Bounds(transform.position + _characterBounds.center, _characterBounds.size);

        _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
        _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
        _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
        _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
    }


    private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
    {
        for (var i = 0; i < _detectorCount; i++)
        {
            var t = (float)i / (_detectorCount - 1);
            yield return Vector2.Lerp(range.Start, range.End, t);
        }
    }

    [Header("WALKING")][SerializeField] private float _acceleration = 90;
    [SerializeField] private float _moveClamp = 13;
    [SerializeField] private float _deAcceleration = 60f;
    [SerializeField] private float _apexBonus = 2;

    private void CalculateWalk()
    {
        if (Input.X != 0)
        {
            // Set horizontal move speed
            _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

            // clamped by max frame movement
            _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

            // Apply bonus at the apex of a jump
            var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
            _currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else
        {
            // No input. Let's slow the character down
            _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
        }

        if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
        {
            // Don't walk through walls
            _currentHorizontalSpeed = 0;
        }
    }

    [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
    [SerializeField] private float _minFallSpeed = 80f;
    [SerializeField] private float _maxFallSpeed = 120f;
    private float _fallSpeed;

    private void CalculateGravity()
    {
        if (_colDown)
        {
            // Move out of the ground
            if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
        }
        else
        {
            // Add downward force while ascending if we ended the jump early
            var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

            // Fall
            _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

            // Clamp
            if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
        }
    }

    [Header("JUMPING")][SerializeField] private float _jumpHeight = 30;
    [SerializeField] private float _jumpApexThreshold = 10f;
    [SerializeField] private float _coyoteTimeThreshold = 0.1f;
    [SerializeField] private float _jumpBuffer = 0.1f;
    [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
    private bool _coyoteUsable;
    private bool _endedJumpEarly = true;
    private float _apexPoint; // Becomes 1 at the apex of a jump
    private float _lastJumpPressed;
    private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
    private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

    private void CalculateJumpApex()
    {
        if (!_colDown)
        {
            // Gets stronger the closer to the top of the jump
            _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
            _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
        }
        else
        {
            _apexPoint = 0;
        }
    }

    private void CalculateJump()
    {
        // Jump if: grounded or within coyote threshold || sufficient jump buffer
        if (Input.JumpDown && CanUseCoyote || HasBufferedJump)
        {
            _currentVerticalSpeed = _jumpHeight;
            _endedJumpEarly = false;
            _coyoteUsable = false;
            _timeLeftGrounded = float.MinValue;
            JumpingThisFrame = true;
        }
        else
        {
            JumpingThisFrame = false;
        }

        // End the jump early if button released
        if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0)
        {
            // _currentVerticalSpeed = 0;
            _endedJumpEarly = true;
        }

        if (_colUp)
        {
            if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
        }
    }

    [Header("MOVE")]
    [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    private int _freeColliderIterations = 10;

    // We cast our bounds before moving to avoid future collisions
    private void MoveCharacter()
    {
        var pos = transform.position + _characterBounds.center;
        RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
        var move = RawMovement * Time.deltaTime;
        var furthestPoint = pos + move;

        // check furthest movement. If nothing hit, move and don't do extra checks
        var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
        if (!hit)
        {
            transform.position += move;
            return;
        }

        // otherwise increment away from current pos; see what closest position we can move to
        var positionToMoveTo = transform.position;
        for (int i = 1; i < _freeColliderIterations; i++)
        {
            // increment to check all but furthestPoint - we did that already
            var t = (float)i / _freeColliderIterations;
            var posToTry = Vector2.Lerp(pos, furthestPoint, t);

            if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer))
            {
                transform.position = positionToMoveTo;

                // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                if (i == 1)
                {
                    if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                    var dir = transform.position - hit.transform.position;
                    transform.position += dir.normalized * move.magnitude;
                }

                return;
            }

            positionToMoveTo = posToTry;
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

    //void Movement()
    //{
    //    movX = Input.GetAxis("Horizontal");
    //    rb.velocity = new Vector2(movX * currentVelocity, rb.velocity.y);
    //}

    //void Jump()
    //{
    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        if(isGrounded || isSwimming)
    //        {
    //            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
    //            isGrounded = false;
    //            doubleJump = true;
    //        } else if (canDoubleJump && doubleJump)
    //        {
    //            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
    //            doubleJump = false;
    //        }
    //    }
    //}

    //void Flip()
    //{
    //    if (movX > 0)
    //    {
    //        transform.localScale = new Vector3(1f, 1f, 1f);
    //    }
    //    if (movX < 0)
    //    {
    //        transform.localScale = new Vector3(-1f, 1f, 1f);
    //    }
    //}

    //void ChangeElement()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1) && !earthPower.CanUse())
    //    {
    //        earthPower.Use();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2) && !icePower.CanUse())
    //    {
    //        icePower.Use();
    //    }
    //}

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