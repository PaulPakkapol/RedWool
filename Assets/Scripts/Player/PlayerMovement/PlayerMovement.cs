using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    private bool _landingGrounded;
    
    private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;
    private float _jumpBufferTime = 0.2f;
    private float _jumpBufferTimeCounter;
    
    private float _xInput;
    private float _yInput; //dont delete bc i dont want to do Eiei

    [Header("PlayerMove")] [Space] //Player customize movement
    public float groundSpeed;
    public float jumpForce;
    public float acceleration;
    private bool _jump = false;
    [Range(0f,1f)] public float groundDecay;
    public bool grounded;

    [Header("PlayerDash")] [Space] 
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    [Header("PlayerWallSlide/Jump")] [Space]
    [SerializeField]private bool isWallSliding;
    [SerializeField]private float wallSlidingSpeed = 2f;
    [SerializeField]private Transform wallCheck;
    [SerializeField]private LayerMask wallMask;
    [SerializeField]private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    [SerializeField]private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    
    [Header("Animation")] [Space]//For animation variable
    public Animator animator;
    private float _horizontalMove = 0f;
    
    [Header("Events")] [Space]
    public UnityEvent onLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        if (onLandEvent == null) //Check 
        {
            onLandEvent = new UnityEvent();
        }
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        
    }
    void Update() 
    {
        GetInput();
        PlayerDash();
        HandleJump();
        RunAnimation();
        StopJumpAnimation();
        WallSlide();
        WallJump();
        
    }
    private void FixedUpdate() 
    {
        if (isDashing)
        {
            return;
        }
        CheckGrounded();
        HandleMovement();
        ApplyFriction();
    }

    void GetInput() //Get in put for playerMove
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");
    }
    
    void HandleMovement() //Make player can move better
    {
        if (Mathf.Abs(_xInput)>0) 
        {
            float increment = _xInput * acceleration ;
            float newSpeed = Mathf.Clamp(rb.velocity.x +increment, -groundSpeed, groundSpeed);
            rb.velocity = new Vector2(newSpeed, rb.velocity.y);

            if (!isWallJumping)
            {
                FlipInput();
            }
            
        }
    }
    
    void RunAnimation()
    {
        _horizontalMove = Input.GetAxis("Horizontal")* groundSpeed ;
        animator.SetFloat("PlayerSpeed",Mathf.Abs(_horizontalMove));
    }
    
    void FlipInput() //Flip player left and right by A D
    {
        float direction = Mathf.Sign(_xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void HandleJump() //Jump input and animation
    {
        CoyoteAndBufferTime();
        if (!isWallJumping)
        {
            if (_jumpBufferTimeCounter > 0f && _coyoteTimeCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                _jump = true;
                animator.SetBool("IsJump", true);
                _jumpBufferTimeCounter = 0f;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                _coyoteTimeCounter = 0f;
            }
        }
    }

    void CoyoteAndBufferTime() 
    {
        if (grounded) 
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump")) 
        {
            _jumpBufferTimeCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferTimeCounter -= Time.deltaTime;
        }
    }

    void StopJumpAnimation() //This method use to stop Jumping Animation by invoke OnLanding Events
    {
        bool wasGrounded = _landingGrounded;
        _landingGrounded = false;
        _landingGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        if (_landingGrounded) 
        {
            _landingGrounded = true;
            if (!wasGrounded)
                onLandEvent.Invoke();
        }
    }
    public void OnLanding()  //this method use to make landing stop that use with Events
    {
        _jump = false;
        animator.SetBool("IsJump", false);
    }
    
    void CheckGrounded() //use to check the ground for Jumping
    { 
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    bool CheckWall()
    {
       return Physics2D.OverlapCircle(wallCheck.position,0.2f,wallMask);
    }

    void WallSlide()
    {
        if (CheckWall() && !grounded && _xInput !=0f)
        {
            isWallSliding = true;
            animator.SetBool("WallSlide", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("WallSlide", false);
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            
            wallJumpDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump")&& wallJumpingCounter >0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                float direction = Mathf.Sign(_xInput);
                transform.localScale = new Vector3(direction, 1, 1);
            }
            Invoke(nameof(StopWallJumping),wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }
    void ApplyFriction() //Make friction  to improve movement
    {
        if (grounded && _xInput ==0 && rb.velocity.y <= 0) 
        {
            rb.velocity *= groundDecay;
        }
    }

    void PlayerDash()
    {
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)&& canDash && _xInput != 0f )
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


}
