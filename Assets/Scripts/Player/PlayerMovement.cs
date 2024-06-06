using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    private bool landingGrounded;
    
    [Header("Player")] [Space] //Player customize movement
    public float groundSpeed;
    public float jumpSpeed;
    public float acceleration;
    private bool jump = false;
    [Range(0f,1f)] public float groundDecay;
    public bool grounded;
    
    private float xInput;
    private float yInput; //dont delete bc i dont want to do Eiei
    
    [Header("Animation")] [Space]//For animation variable
    public Animator animator;
    private float horizontalMove = 0f;
    
    [Header("Events")] [Space]
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        if (OnLandEvent == null)//Check 
            OnLandEvent = new UnityEvent();
    }

    void Update() {
        GetInput();
        HandleJump();
        RunAnimation();
    }
    
    private void FixedUpdate() {
        CheckGrounded();
        HandleMovement();
        ApplyFriction();
        StopJumpAnimation();
        
    }

    void GetInput() //Get in put for playerMove
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }
    
    void HandleMovement() //Make player can move better
    {
        if (Mathf.Abs(xInput) > 0)
        {
            float increment = xInput * acceleration ;
            float newSpeed = Mathf.Clamp(body.velocity.x + increment, -groundSpeed, groundSpeed);
            body.velocity = new Vector2(newSpeed, body.velocity.y);

            FlipInput();
        }
    }
    void RunAnimation()
        {
            horizontalMove = Input.GetAxis("Horizontal") * groundSpeed;
            animator.SetFloat("PlayerSpeed",Mathf.Abs(horizontalMove));
        }
    
    void FlipInput() //Flip player left and right by A D
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void HandleJump() //Jump input and animation
    {
        if (Input.GetButtonDown("Jump") && grounded) {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            jump = true;
            animator.SetBool("IsJump", true);
        }
    }

    void StopJumpAnimation() //This method use to stop Jumping Animation by invoke OnLanding Events
    {
        bool wasGrounded = landingGrounded;
        landingGrounded = false;
        landingGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        if (landingGrounded)
        {
            landingGrounded = true;
            if (!wasGrounded)
                OnLandEvent.Invoke();
        }
    }
    public void OnLanding()  //this method use to make landing stop that use with Events
    {
        jump = false;
        animator.SetBool("IsJump", false);
    }
    void ApplyFriction() //Make friction  to improve movement
    {
        if (grounded && xInput ==0 && body.velocity.y <= 0) {
            body.velocity *= groundDecay;
        }
    }
    void CheckGrounded() //use to check the ground for Jumping
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        
    }

    
}
