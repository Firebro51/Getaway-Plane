using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator animator;

    [SerializeField] private float speed;
    [SerializeField] private float jump;


    private bool IsGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public float xInput;

    public bool isMoving;



    [SerializeField] private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);


        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Debug.Log("YEEEEEEEEEEEEEEEE Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }

        HandleCollisions();

        HandleAnimations();

        HandleMovement();
    }

    private void HandleCollisions()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleAnimations()
    {
        isMoving = rb.linearVelocity.x != 0;
        animator.SetBool("IsMoving", isMoving);
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
