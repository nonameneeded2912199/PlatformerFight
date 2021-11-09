using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : BaseCharacter
{
    public static Player Instance { get; private set; }

    [Header("Component")]
    private BetterJumping betterJumping;
    private PlayerInputAction playerInputAction;

    private bool isInvincible;

    private float horizontal;

    [Space]
    [Header("MovementStats")]
    public float movementSpeed = 10;
    public float jumpForce = 50;
    public float wallJumpForce = 1.5f;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public int totalJumps = 2;
    [SerializeField]
    private int availableJumps;

    [Space]
    [Header("Checkers")]
    public bool canMove;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;
    public bool jumpStop;

    [Space]
    private bool hasDashed;

    [Space]
    [Header("AbilityChecks")]
    public bool canGrab;
    public bool canDoubleJump;
    public bool canDash;

    #region Properties
    public float Horizontal => horizontal;

    public bool IsDashing => isDashing;

    public bool WallSlide => wallSlide;

    public bool CanMove => canMove;
    #endregion


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerInputAction = new PlayerInputAction();

        playerInputAction.Player.Move.performed += ctx => MovementInput(ctx);
        playerInputAction.Player.Jump.performed += ctx => OnJumpPerformedInput();
        playerInputAction.Player.Jump.canceled += ctx => OnJumpCancelledInput();
        playerInputAction.Player.Dash.performed += ctx => Dash();

        playerInputAction.Enable();
    }

    protected override void Start()
    {
        base.Start();
        betterJumping = GetComponent<BetterJumping>();
        

        //playerInput.

        availableJumps = totalJumps;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isGrounded && !isDashing)
        {
            wallJumped = false;
            betterJumping.enabled = true;
        }    

        if (!onWall || isGrounded)
            wallSlide = false;

        HandleFlip();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        /*if (canDash)
            StartCoroutine(Dash());*/
        if (!isDashing)
        {
            HandleMovement();
            if (isGrounded)
            {
                hasDashed = false;
                isDashing = false;
                jumpStop = false;
                if (!(rb.velocity.y > 0f))
                    availableJumps = totalJumps;
            }
            else
            {
                if (onWall && !isGrounded)
                {
                    if (rb.velocity.y < 0)
                    {
                        if (horizontal != 0)
                        {
                            wallSlide = true;
                            availableJumps = totalJumps;
                            WallSliding();
                        }
                        else
                        {
                            wallSlide = false;
                        }
                    }
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (!canMove)
            return;

        rb.velocity = new Vector2(horizontal * movementSpeed, rb.velocity.y);
    }

    private void HandleFlip()
    {
        if ((facingRight && horizontal < 0) || (!facingRight && horizontal > 0))
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0f, facingRight ? 0 : 180f, 0f);
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        if (canMove && !isDashing)
            horizontal = context.ReadValue<Vector2>().x;
        else
            horizontal = 0;
    }

    /*public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!wallSlide)
                Jump();
            else
                WallJump();
        }
        if (context.canceled)
        {
            jumpStop = true;
        }
    }*/

    public void OnJumpPerformedInput()
    {
        if (!wallSlide)
            Jump();
        else
            WallJump();
    }

    public void OnJumpCancelledInput()
    {
        jumpStop = true;
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Dash();
        }
    }

    private void Dash()
    {
        if (!canDash)
            return;

        if (hasDashed)
            return;

        hasDashed = true;

        rb.velocity = Vector2.zero;
        int direction = facingRight ? 1 : -1;
        Vector2 dir = new Vector2(direction, 0);

        rb.velocity += dir.normalized * dashSpeed;

        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());

        float originalGravity = rb.gravityScale;

        rb.gravityScale = 0;
        betterJumping.enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(0.3f);

        rb.gravityScale = originalGravity;
        betterJumping.enabled = true;

        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (isGrounded)
        {
            hasDashed = false;
        }
    }

    private void WallJump()
    {
        if (!canGrab)
            return;

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = onRightWall ? Vector2.left : Vector2.right;

        //Jump((Vector2.up / wallJumpForce + wallDir / wallJumpForce), true);

        wallJumped = true;
    }

    private void WallSliding()
    {
        if (!canGrab)
            return;

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && onRightWall) || (rb.velocity.x < 0 && !onRightWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;
        animator.SetBool("WallSlide", true);
        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            availableJumps--;
            rb.velocity = Vector2.up * jumpForce;
        }
        else
        {
            if (availableJumps > 0)
            {
                availableJumps--;
                rb.velocity = Vector2.up * jumpForce;
            }
        }
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public void EnableMove()
    {
        canMove = true;
    }

    public void DisableMove()
    {
        canMove = false;
    }
}
