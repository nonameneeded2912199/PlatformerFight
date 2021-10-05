using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance { get; private set; }

    [Header("Stat")]
    public float hp;
    public float maxHP;
    public Image imageHP;
    public Text textHP;

    [Header("Component")]
    private Rigidbody2D rb;
    private Animator myAnimator;

    private float horizontal;
    private float dashTimeLeft;
    private float lastDash = -100f;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private bool isTouchingWall;
    private bool facingRight = true;
    private bool isWallSliding;
    private bool allowMove = true;
    private bool isGrounded;
    private bool isDashing;
    private bool allowDash = true;
    private bool knockback;
    private bool wallJumping = false;
    private bool isInvincible;

    [Header("Physics Parameter")]
    [SerializeField]
    private Vector2 knockbackSpeed;
    public float speed, jumpHeight;
    public float jumpHeightModifier = 0.5f;
    public float groundCheckRadius;
    public float wallCheckRadius;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float dashTime;
    public float dashSpeed;
    public float dashCooldown;
    public Vector2 wallForce;
    public float wallJumpTime;
    public Transform groundPoint;
    public Transform wallPoint;
    public LayerMask groundLayer;

    public bool FacingRight => facingRight;

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
        myAnimator = GetComponent<Animator>();

        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = GetComponent<InputManagerCharacter>().horizontal;
        HandleHPBar();
        HandleInput();
    }

    private void HandleMovement()
    {
        if (allowMove && !knockback)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (isWallSliding && allowMove)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }  

    private void HandleHPBar()
    {
        imageHP.fillAmount = hp / maxHP;
        textHP.text = hp + " / " + maxHP;
    }
    
    private void Jump()
    {
        if (isGrounded && !isWallSliding && allowMove)
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);

        if (isWallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }       
    }   

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
    
    private void HandleAnimations()
    {
        myAnimator.SetBool("Move", (horizontal != 0 && allowMove));
        myAnimator.SetBool("Grounded", isGrounded);
        myAnimator.SetFloat("Jump", rb.velocity.y);
        myAnimator.SetBool("WallSlide", isWallSliding);
    }    

    private void HandleFlip()
    {
        if (((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) && allowMove && !knockback)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }    
    }    

    private void HandleWallJump()
    {
        if (wallJumping)
        {
            int dir = facingRight == true ? -1 : 1;
            rb.velocity = new Vector2(wallForce.x * dir, wallForce.y);
        }
    }

    private void FixedUpdate()
    {
        CheckSurroundings();

        HandleMovement();

        HandleFlip();

        HandleWallJump();

        HandleAnimations();

        CheckIfWallSliding();

        CheckDash();

        CheckKnockback();
    }


    private void HandleInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightModifier);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time >= (lastDash + dashCooldown) && allowDash)
                AttemptToDash();
        }
    }    

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundCheckRadius, groundLayer);
        float directionCheck = (facingRight == true) ? 1 : -1;
        isTouchingWall = Physics2D.Raycast(wallPoint.position, transform.right * directionCheck, wallCheckRadius, groundLayer);
    } 

    private void CheckDash()
    {
        if (isDashing)
        {
            allowDash = false;
            if (dashTimeLeft > 0)
            {
                Shadow.Instance.UpdateTimer();
                allowMove = false;
                rb.velocity = new Vector2(dashSpeed * ((facingRight == true) ? 1 : -1), 0);
                dashTimeLeft -= Time.deltaTime;
            }

            if (dashTimeLeft <= 0 || isTouchingWall )
            {
                isDashing = false;
                allowMove = true;
            }
        }
        else
        {
            if (isTouchingWall || isGrounded)
                allowDash = true;
        }
    }
    
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    public bool CheckAnimationIsPlaying(string c)
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(c))
            return true;
        return false;
    }

    public void EnableMove()
    {
        allowMove = true;
    }

    public void DisableMove()
    {
        allowMove = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void ToIdle()
    {
        myAnimator.speed = 1f;
        if (CheckAnimationIsPlaying("LieDown"))
        {

        }
        else if (isGrounded)
        {

        }
    }

    private void TakeDamage(AttackDetails attackDetails)
    {
        if (!isDashing && !isInvincible)
        {
            hp -= attackDetails.damageAmount;

            if (hp > 0)
            {
                StartCoroutine(BecomeInvicible(1.5f));

                int direction = attackDetails.position.x < transform.position.x ? 1 : -1;

                Knockback(direction);
            }
            else
            {
                Die();
            }
        }
    }

    public void Knockback(int direction)
    {
        knockback = true;
        DisableMove();
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback && isGrounded)
        {
            knockback = false;
            EnableMove();
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }    

    public void Die()
    {
        hp = 0;

        HandleHPBar();

        gameObject.SetActive(false);

        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.Follow = null;

        GameManager.Instance.RestartLevel();
        //myAnimator.SetBool("Dead", true);

    }

    public void Respawn(Vector3 position)
    {
        print("Last check point is: " + position);

        hp = maxHP;

        transform.position = position;

        //myAnimator.SetBool("Dead", false);
    }

    private IEnumerator BecomeInvicible(float seconds)
    {
        isInvincible = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        for (float i = 0; i < seconds; i += seconds / 10)
        {
            if (spriteRenderer.enabled)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }

            yield return new WaitForSeconds(seconds / 10);
        }
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    public void Regenerate()
    {
        hp = maxHP;
    } 
    
    public void PlayAnimation(string c)
    {
        
    }

    public void StopWaitForSecondsToPlayIdle()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundPoint.position, groundCheckRadius);
        Gizmos.DrawLine(wallPoint.position, new Vector3(wallPoint.position.x + wallCheckRadius * ((facingRight == true) ? 1 : -1), 
            wallPoint.position.y, wallPoint.position.z));
    }

}
