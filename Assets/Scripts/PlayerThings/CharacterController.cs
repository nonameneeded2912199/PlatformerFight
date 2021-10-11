using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance { get; private set; }

    [Header("Component")]
    private Rigidbody2D rb;
    private Animator animator;

    [Header("LayerMask")]
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;

    private bool isInvincible;
    [Header("Movement")]
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float groundLinearDrag = 10f;
    [SerializeField]
    private bool facingRight = true;

    private float horizontal;
    [SerializeField]
    private bool canMove = true;

    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 12f;
    [SerializeField]
    private float airLinearDrag = 2.5f;
    [SerializeField]
    private float fallMultiplier = 8f;
    [SerializeField]
    private float lowJumpFallMultiplier = 5f;
    [SerializeField]
    private float jumpDelay = 0.15f;
    [SerializeField]
    private int extraJumps = 1;
    [SerializeField]
    private float hangTime = 1f;
    [SerializeField]
    private float jumpBufferLength = 0.1f;
    [SerializeField]
    private float timeBeforeMovable = 0.1f;
    [SerializeField]
    private float timeBeforeWallJump = 0.1f;
    private bool preparingToWallJump = false;

    private bool jump => jumpBufferCounter > 0f && (hangTimeCounter > 0f || extraJumpsSum > 0);

    private bool wallGrab => onWall && !isGrounded && ((horizontal > 0 && onRightWall) || (horizontal < 0 && !onRightWall));

    private int extraJumpsSum;
    private float hangTimeCounter;
    [SerializeField]
    private float jumpBufferCounter;

    [Header("Dash Variables")]
    [SerializeField]
    private float dashSpeed = 15f;
    [SerializeField]
    private float dashLength = 3f;
    [SerializeField]
    private float dashBufferLength = 0.1f;
    private float dashBufferCounter;
    private bool isDashing;
    private bool hasDashed;
    [SerializeField]
    private float dashCooldown = 1f;
    private float dashCooldownCounter = 0f;
    private bool canDash => dashBufferCounter > 0f && !hasDashed && dashCooldownCounter == 0;

    [Header("Ground Collision Check")]
    [SerializeField]
    private float groundRaycastLength;
    [SerializeField]
    private Vector3 groundRaycastOffset;
    private bool isGrounded;

    [Header("Wall Collision Check")]
    [SerializeField]
    private float wallRaycastLength;
    public bool onWall;
    public bool onRightWall;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleAnimation();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        HandleCollision();
        if (canDash)
            StartCoroutine(Dash());
        if (!isDashing)
        {
            if (canMove)
                HandleMovement();
            if (isGrounded)
            {
                extraJumpsSum = extraJumps;
                ApplyGroundLinearDrag();
                hangTimeCounter = hangTime;
                hasDashed = false;
            }
            else
            {
                ApplyAirLinearDrag();
                if (!preparingToWallJump)
                    FallMultiplier();
                hangTimeCounter -= Time.fixedDeltaTime;
            }
            if (jump && canMove)
            {
                if (wallGrab)
                {
                    PrepareToWallJump();
                }
                else
                {
                    Jump(Vector2.up);
                }
            }
        }              
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0f, facingRight ? 0 : 180f, 0f);
        Debug.Log("Flipped");
    }

    private void HandleAnimation()
    {
        animator.SetBool("Move", horizontal != 0);
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("Jump", rb.velocity.y);
        animator.SetBool("IsDashing", isDashing);
    }
    
    private void HandleFlip()
    {
        if ((horizontal < 0 && facingRight) || (horizontal > 0 && !facingRight))
        {
            Flip();
        }
    }

    private void HandleInput()
    {
        if (canMove && !isDashing)
            horizontal = Input.GetAxisRaw("Horizontal");
        else
            horizontal = 0;

        if (Input.GetButtonDown("Jump") && !isDashing)
            jumpBufferCounter = jumpBufferLength;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Dash"))
        {
            dashBufferCounter = dashBufferLength;
        }    
        else
        {
            dashBufferCounter -= Time.deltaTime;
        }    
    }

    private void Jump(Vector2 direction)
    {
        if (!isGrounded && !wallGrab)
            extraJumpsSum--;

        ApplyAirLinearDrag();
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;
    }

    private async void PrepareToWallJump()
    {
        preparingToWallJump = true;
        animator.SetBool("WallGrab", true);
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        float timeStartToPrepare = Time.time;
        while (Time.time < timeStartToPrepare + timeBeforeWallJump)
        {   
            await Task.Yield();
        }
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetBool("WallGrab", false);
        WallJump();
        preparingToWallJump = false;
    }

    private void WallJump()
    {
        Vector2 jumpDirection = onRightWall ? Vector2.left : Vector2.right;
        Jump(jumpDirection + Vector2.up);
        StartCoroutine(StunMoveWallJump());
        Flip();
    }

    private void FallMultiplier()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void HandleCollision()
    {
        // Ground collision
        isGrounded = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer)
            || Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer);

        // Wall collision
        onWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, wallLayer)
            || Physics2D.Raycast(transform.position, Vector2.left, wallRaycastLength, wallLayer);

        onRightWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, wallLayer);
    }

    private void HandleMovement()
    {
        rb.AddForce(Vector2.right * horizontal * acceleration);

        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
        }
    }

    private void ApplyGroundLinearDrag()
    {
        bool changingDirection = (rb.velocity.x > 0f && horizontal < 0f) || (rb.velocity.x < 0f && horizontal > 0f);
        if (Mathf.Abs(horizontal) < 0.4f || changingDirection)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;
    }

    private IEnumerator StunMoveWallJump()
    {
        canMove = false;
        yield return new WaitForSeconds(timeBeforeMovable);
        canMove = true;
    }

    private IEnumerator Dash()
    {
        float dashStartTime = Time.time;
        hasDashed = true;
        isDashing = true;
        isInvincible = true;

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.drag = 0f;
        extraJumpsSum = 0;

        Vector2 dir = facingRight ? Vector2.right : Vector2.left;

        while (Time.time < dashStartTime + dashLength)
        {
            rb.velocity = dir.normalized * dashSpeed;
            yield return null;
        }

        isDashing = false;
        isInvincible = false;
        EnterDashCooldown();
    } 

    private async void EnterDashCooldown()
    {
        dashCooldownCounter = dashCooldown;
        
        while (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
            await Task.Yield();
        }

        dashCooldownCounter = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + groundRaycastOffset, transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);
        Gizmos.DrawLine(transform.position - groundRaycastOffset, transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);

        // Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallRaycastLength);
    }



}
