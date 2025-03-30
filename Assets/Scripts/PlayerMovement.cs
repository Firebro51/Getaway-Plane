using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementImproved : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Trail")]
    private TrailRenderer lightTrail;



    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Buffer & Coyote jump")]
    [SerializeField] private float bufferJumpWindow = .25f;
    private float bufferJumpActivated = -1;
    [SerializeField] private float coyoteJumpWindow = .5f;
    private float coyoteJumpActivated = -1;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 1;
    [SerializeField] private Vector2 knockbackPower;
    private bool isKnocked;


    [Header("Collision")]
    [SerializeField] private float groundCheckDistnace;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Vector2 wallCheckOffSet = new Vector2(0f, 0.5f);
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool isAirborne;
    private bool isWallDetected;


    [Header("Animation")]
    [SerializeField] private float fallAnimationStopDistance = 1f; // How close to ground to stop fall animation
    private float animYVelocity;
    
    private float xInput;
    private float yInput;

    private bool facingRight = true;
    private int facingDir = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        lightTrail = GetComponent<TrailRenderer>();
    }


    private void Update()
    { 
        UpdateAirbornStatus();

        if (isKnocked)
            return;

        HandleInput();
        HandleWallSlide();
        HandleMovement();
        // HandleTrail();
        HandleFlip();
        HandleCollision();
        HandleAnimations();
    }

    public void Knockback()
    {
        if(isKnocked)
            return;

        StartCoroutine(KnockbackRoutine());
        anim.SetTrigger("knockback");
        rb.linearVelocity = new Vector2(knockbackPower.x * -facingDir, knockbackPower.y);
    }
    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }


    private void UpdateAirbornStatus()
    {
        if (isGrounded && isAirborne)
            HandleLanding();

        if (!isGrounded && !isAirborne)
            BecomeAirborne();
    }
    private void BecomeAirborne()
    {
        isAirborne = true;

        if (rb.linearVelocity.y < 0)
            ActivateCoyoteJump();
    }

    private void HandleLanding()
    {
        isAirborne = false;

        AttemptBufferJump();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        

        // KeyCode.JoystickButton0 = A button on xbox controller
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            JumpButton();
            RequestBufferJump();
        }
    }

    #region Coyote & Buffer Jump

    private void RequestBufferJump()
    {
        if (isAirborne)
            bufferJumpActivated = Time.time;
    }
    private void AttemptBufferJump()
    {
        if (Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            Jump();
        }
    }
    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;
    private void CancelCoyoteJump() => coyoteJumpActivated = Time.time - 1;

    #endregion

    private void JumpButton()
    {
        bool coyoteJumpAvalible = Time.time < coyoteJumpActivated + coyoteJumpWindow;

        if (isGrounded || coyoteJumpAvalible)
        {
            Jump();
        }

        CancelCoyoteJump();
    }



    private void Jump() => rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocity.y < 0;
        float yModifer = yInput < 0 ? 1 : .05f;

        if (canWallSlide == false)
            return;


        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yModifer);
    }

    // Wall jump

    // conditions - Touching a wall, Airborne then you can wall jump

    private void WallJump()
    {
        
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistnace, whatIsGround);
        Vector2 wallCheckPosition = (Vector2)transform.position + wallCheckOffSet;
        isWallDetected = Physics2D.Raycast(wallCheckPosition, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsWallDetected", isWallDetected);

        HandleFallAnimation();
    }

    private void HandleFallAnimation()
    {
        // Cast a ray to check distance to ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, fallAnimationStopDistance, whatIsGround);
        
        // If falling but close to ground, adjust the yVelocity that's sent to the animator
        if (!isGrounded && rb.linearVelocity.y < 0 && hit.collider != null)
        {
            // This will be sent to the animator instead of the actual yVelocity
            animYVelocity = 0; // Or a small negative value that doesn't trigger your fall animation
        }
        else
        {
            // Use the actual velocity
            animYVelocity = rb.linearVelocity.y;
        }
    }

    private void HandleMovement()
    {
        if (isWallDetected)
            return;

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if (xInput < 0 && facingRight || xInput > 0 && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistnace));
        
        Vector2 wallCheckPosition = (Vector2)transform.position + wallCheckOffSet;
        Gizmos.DrawLine(wallCheckPosition, new Vector2(wallCheckPosition.x + (wallCheckDistance * facingDir), wallCheckPosition.y));

        // Draw the fall animation 
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - fallAnimationStopDistance));
    }
}
