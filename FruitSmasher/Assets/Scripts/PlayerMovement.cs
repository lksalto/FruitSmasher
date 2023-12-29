using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 7f;
    public float hangingTime = 0.2f; // Time player can hang on the edge
    public float coyoteTime = 0.2f; // Time after leaving the ground where the player can still jump
    public float inputBufferTime = 0.1f; // Time to buffer jump input
    public float horizontalInput = 0f;
    public SpriteRenderer sr;
    public LayerMask groundLayer;
    bool singleSClick = false;
    public bool isGrounded;
    bool canMove = true;
    [SerializeField] bool canDoubleJump;
    private bool isJumping;

    [SerializeField] bool canPassThroughPlatform;
    public float platSec = 0.3f;
    private float hangTimer;
    private float coyoteTimer;
    private float inputBufferTimer;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private bool canDropThroughPlatform = true;
    public float dropCooldown = 0.5f; // Adjust the cooldown duration as needed

    private int originalLayer; // To store the original layer of the player

    public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        canMove = true;
    }

    void Update()
    {
        HandleInput();
        CheckGrounded();
        CheckHanging();
        CheckDoubleJump();
        CheckPassThroughPlatform();
        UpdateTimers();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        if(canMove)
        {

        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if(horizontalInput < 0)
        {
            sr.flipX = true;
        }
        else if (horizontalInput > 0) 
        {
            sr.flipX = false;
        }
        // Move horizontally
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || coyoteTimer > 0))
        {
            Jump();
        }

        // Double jump
        if (Input.GetKeyDown(KeyCode.W) && canDoubleJump && !(isGrounded || coyoteTimer > 0))
        {
            DoubleJump();
        }

        // Drop through platform
        //canPassThroughPlatform = Input.GetKey(KeyCode.S);
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            DropThroughPlatform();
        }

        }
        else
        {
            horizontalInput = 0;
        }

    }

    void CheckDoubleJump()
    {

    }

    void DropThroughPlatform()
    {
        canPassThroughPlatform = true;
        Invoke("ResetDropCooldown", dropCooldown);
        

    }
    void ResetDropCooldown()
    {
        // Reset the player's layer to the original layer
        canPassThroughPlatform = false;
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        canDoubleJump = true;
        coyoteTimer = 0f;
    }

    void DoubleJump()
    {
        canDoubleJump = false;
        rb.velocity = Vector3.zero;
        PerformJump();
        Invoke("PerformJump", 0.07f);
    }

    void Move()
    {
        // Add additional movement logic here if needed
    }

    void CheckGrounded()
    {
        // Raycast to check if the player is grounded
        RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, groundLayer);

        // Debug raycast
        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + 0.1f), Color.red);

        // Update isGrounded based on the raycast hit
        isGrounded = hit.collider != null;

        // Reset coyote time when grounded
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else if (!isGrounded && !isJumping)
        {
            coyoteTimer += Time.deltaTime;
        }
    }

    void CheckHanging()
    {
        /*
        if (!isGrounded && !isJumping)
        {
            hangTimer += Time.deltaTime;

            if (hangTimer < hangingTime)
            {
                isHanging = true;
                canDoubleJump = true;
            }
            else
            {
                isHanging = false;
                hangTimer = 0f;
            }
        }
        else
        {
            isHanging = false;
            hangTimer = 0f;
        }
        */
    }

    void CheckPassThroughPlatform()
    {
        //Debug.Log(rb.velocity.y > 0.1f);
        if(rb.velocity.y > 0.1f)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), true);
            //Debug.Log("xau");
        }
        else if(rb.velocity.y < 0.1f && !canPassThroughPlatform)
        {
            
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), false);
        }
        else if(canPassThroughPlatform)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platform"), true);

        }


    }

    void UpdateTimers()
    {
        // Update timers
        inputBufferTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
    }

    public void SetCanMove(bool b)
    {
        canMove = b;
    }

    void PerformJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, doubleJumpForce, 0);
        
    }

}
