using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed;
    [SerializeField] private float jump;


    private bool IsGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public float xInput;




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

        HandleCollisions();

        HandleAnimations();

        HandleMovement();
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
            }
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            // if the variable isn't empty (we have a reference to our SpriteRenderer
            if(spriteRenderer != null)
            {
                 // flip the sprite
                 spriteRenderer.flipX = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && IsGrounded)
        {
            Debug.Log("YEEEEEEEEEEEEEEEE Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
    }
    private void HandleCollisions()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("IsGrounded", IsGrounded);
    }   

    private void HandleMovement()
    {
        HandleInput();
        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
