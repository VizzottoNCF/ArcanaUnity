using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region vars
    [Header("References")]
    public PlayerMovementStats moveStats;
    [SerializeField] private Collider2D _feetCollider;
    [SerializeField] private Collider2D _bodyCollider;

    private Rigidbody2D _rb;

    // move vars
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    // collision check vars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    // jump vars
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFalling;
    private bool _isFastFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    // jump apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    // jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    // coyote time vars
    private float _coyoteTimer;

    #endregion
    private void Awake()
    {
        _isFacingRight = true;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rf_JumpChecks();
        rf_CountTimers();
    }

    private void FixedUpdate()
    {
        rf_CollisionChecks();
        rf_Jump();

        if (_isGrounded)
        {
            rf_Move(moveStats.groundAcceleration, moveStats.groundDeceleration, InputManager.Movement);
        }
        else
        {
            rf_Move(moveStats.airAcceleration, moveStats.airDeceleration, InputManager.Movement);
        }
    }


    #region Movement

    /// <summary>
    /// Function called to make the player move around
    /// </summary>
    /// <param name="acceleration"> Rate of which the character accelerates.</param>
    /// <param name="deceleration"> Rate of which the character decelerates.</param>
    /// <param name="moveInput"> The Input Direction being given by the player.</param>
    private void rf_Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            // first check if player needs to turn around
            rf_TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            if (InputManager.runIsHeld) { targetVelocity = new Vector2(moveInput.x, 0f) * moveStats.maxRunSpeed; }
            else { targetVelocity = new Vector2(moveInput.x, 0f) * moveStats.maxWalkSpeed; }

            // lerp move velocity from current to target velocity and then apply to rigidbody
            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }

        // if there is no movement input, change move velocity to nothing
        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }

    }

    private void rf_TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0) { rf_Turn(false); }
        else if (!_isFacingRight && moveInput.x > 0) { rf_Turn(true); }
    }

    private void rf_Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Jump
    /// <summary> What happens when the jump button is pressed. </summary>
    private void rf_JumpChecks()
    {
        // WHEN JUMP BUTTON IS PRESSED
        if (InputManager.jumpWasPressed)
        {
            _jumpBufferTimer = moveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }

        // WHEN JUMP BUTTON IS RELEASED
        if (InputManager.jumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = moveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }

        // INITIATE JUMP WITH JUMP BUFFERING AND COYOTE TIME
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            rf_InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }


        // DOUBLE JUMP (AND MORE)
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < moveStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            rf_InitiateJump(1);
        }

        // AIR JUMP AFTER COYOTE TIME LAPSED (take off an extra jump so we don't get a bonus jump)
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < moveStats.NumberOfJumpsAllowed - 1)
        {
            rf_InitiateJump(2);
            _isFastFalling = false;
        }

        // LANDED
        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void rf_InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = moveStats.InitialJumpVelocity;
    }
    /// <summary>
    /// Jump logic 
    /// </summary>
    private void rf_Jump()
    {
        // APPLY GRAVITY WHILE JUMPING
        if (_isJumping)
        {
            // CHECK FOR HEAD BUMP
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            // GRAVITY ON ASCENDING
            if (VerticalVelocity >= 0f)
            {
                // APEX CONTROLS
                _apexPoint = Mathf.InverseLerp(moveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > moveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < moveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }
                // GRAVITY ON ASCENDING, BUT NOT PAST APEX THRESHOLD
                else
                {
                    VerticalVelocity += moveStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }
            // GRAVITY ON DESCENDING
            else if (!_isFastFalling)
            {
                VerticalVelocity += moveStats.Gravity * moveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        // JUMP CUT
        if (_isFastFalling)
        {
            if (_fastFallTime >= moveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += moveStats.Gravity * moveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < moveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / moveStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        // NORMAL GRAVITY WHILE FALLING 
        if (!_isGrounded && !_isJumping)
        {
            if (_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += moveStats.Gravity * Time.fixedDeltaTime;
        }

        // CLAMP FALL SPEED APPLY TO THE RIGID BODY VELOCITY
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -moveStats.MaxFallSpeed, 50f);

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
    }

    #endregion

    #region Timers
    private void rf_CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        if (!_isGrounded) { _coyoteTimer -= Time.deltaTime; }
        else { _coyoteTimer = moveStats.JumpCoyoteTime; }
    }
    #endregion

    #region Collision Checks

    private void rf_IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, _feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x, moveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, moveStats.GroundDetectionRayLength, moveStats.GroundLayer);
        if (_groundHit.collider != null) { _isGrounded = true; }
        else { _isGrounded = false; }

        #region Debug Visualisation
        if (moveStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if (_isGrounded) { rayColor = Color.blue; }
            else { rayColor = Color.magenta; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * moveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - moveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    private void rf_BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_bodyCollider.bounds.center.x, _bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_bodyCollider.bounds.size.x * moveStats.HeadWidth, moveStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, moveStats.HeadDetectionRayLength, moveStats.GroundLayer);
        if (_headHit.collider != null) { _bumpedHead = true; }
        else { _bumpedHead = false; }

        #region Debug Visualisation
        if (moveStats.DebugShowHeadBumpBox)
        {
            float headWidth = moveStats.HeadWidth;

            Color rayColor;
            if (_bumpedHead) { rayColor = Color.green; }
            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - ((boxCastSize.x / 2) * headWidth), boxCastOrigin.y), Vector2.up * moveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + ((boxCastSize.x / 2) * headWidth), boxCastOrigin.y), Vector2.up * moveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, _bodyCollider.bounds.max.y + moveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        }
        #endregion
    }

    private void rf_CollisionChecks()
    {
        rf_IsGrounded();
        rf_BumpedHead();
    }


    #endregion
}