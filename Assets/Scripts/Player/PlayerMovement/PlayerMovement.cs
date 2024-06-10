using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    private bool _landingGrounded;


    [Header("Player")] [Space] //Player customize movement
    public float groundSpeed;
    public float jumpSpeed;
    public float acceleration;
    private bool _jump = false;
    [Range(0f,1f)] public float groundDecay;
    public bool grounded;
    
    private float _xInput;
    private float _yInput; //dont delete bc i dont want to do Eiei
    
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
        HandleJump();
        RunAnimation();
        StopJumpAnimation();
    }
    private void FixedUpdate() 
    {
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

            FlipInput();
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
        if (Input.GetButtonDown("Jump") && grounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            _jump = true;
            animator.SetBool("IsJump", true);
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
    void ApplyFriction() //Make friction  to improve movement
    {
        if (grounded && _xInput ==0 && rb.velocity.y <= 0) {
            rb.velocity *= groundDecay;
        }
    }


}
