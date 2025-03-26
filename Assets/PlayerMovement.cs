using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed;
    [SerializeField] private float jump;


    public float bufferJumpPressed;

    private bool IsGrounded;
    private bool IsAirborne;
    private bool isWallDetected;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public float xInput;

    private bool facingRight;
    private float facingDir = 1; // 1 is right, -1 is left


    [SerializeField] private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAirborne();

        HandleCollisions();

        HandleInput();

        handleWallSlide();

        HandleMovement();

        HandleAnimations();
    }

    private void UpdateAirborne()
    {
        // !IsAirborn is only here so it toggles once and not forever
        // Think of it as only checking the first condition
        if (!IsGrounded && !IsAirborne)
        {
            BecomeAirborne();
        }
        // IsAirborn is only here so it toggles once and not forever
        // Think of it as only checking the first condition
        if (IsGrounded && IsAirborne)
        {
            HandleLanding();
        }
    }

    private void BecomeAirborne()
    {
        IsAirborne = true;
    }

    private void HandleLanding()
    {
        IsAirborne = false;
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");        
        
        if(Input.GetKeyDown(KeyCode.A))
        {
            // if the variable isn't empty (we have a reference to our SpriteRenderer
            if(spriteRenderer != null)
            {
                // flip the sprite
                spriteRenderer.flipX = true;
                facingDir = -1;
                facingRight = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            // if the variable isn't empty (we have a reference to our SpriteRenderer
            if(spriteRenderer != null)
            {
                // flip the sprite
                spriteRenderer.flipX = false;
                facingDir = 1;
                facingRight = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && IsGrounded)
        {
            Debug.Log("YEEEEEEEEEEEEEEEE Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
    }

    private void handleWallSlide()
    {
        if (isWallDetected && rb.linearVelocityY < 0)
        {
            // Wall Slide when falling
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
        }
    }


    private void HandleCollisions()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("IsGrounded", IsGrounded);
    }   

    private void HandleMovement()
    {
        // Dont keeping runing if move facing fall infront of player
        if (isWallDetected)
        {
            return;
        }
        HandleInput();
        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }
}
