/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private GameObject characterBody;

    public int direction;

    private int movingDirection;

    public float runSpeed;

    public float jumpForce;

    public float jumpMoveForce;

    public bool isGrounded;

    public bool isWall;

    public bool isBodyGrounded;

    public bool isBodyWall;

    private int canJump;

    private int jumpTimes;

    public int jumpTimesSum;

    private int isHurt;

    private int canFlip;

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;

    [SerializeField]
    private BoxCollider2D bodyCollider;

    [SerializeField]
    private BoxCollider2D bodyThroughCollider;

    [SerializeField]
    private BoxCollider2D feetCollider;

    [SerializeField]
    private BoxCollider2D triggerBodyCollider;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        canJump = 1;
        jumpTimes = jumpTimesSum;

        canFlip = 1;

        isHurt = 0;

        direction = 1;

        movingDirection = 1;        
    }

    public bool CheckAnimationIsPlaying(string c)
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(c))
            return true;
        return false;
    }

    private void LateUpdate()
    {
        Flip();
        CommonMoveable();
        Run();
        DoJumpStop();
        Jump();
        Fall();
        IsGrounded();
        CollidersControl();
        Idle();
        Dash();
    }

    private void Flip()
    {
        if (canFlip == 1)
        {
            direction = transform.GetComponent<InputManagerCharacter>().horizontal;

            if (direction == 1)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            if (direction == -1)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void Run()
    {
        if (direction != 0)
            myAnimator.SetBool("Run", true);
        else
            myAnimator.SetBool("Run", false);
    }

    private void Jump()
    {
        if (CheckAnimationIsPlaying("Idle") || CheckAnimationIsPlaying("Jump") || CheckAnimationIsPlaying("Run"))
        {
            canJump = 1;
        }
        else
        {
            canJump = 0;
        }
    }
}*/
