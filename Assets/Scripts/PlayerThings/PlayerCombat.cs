using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool isAttacking;

    public bool isJumpAttacking;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float interval = 2f;

    private float timer;

    [SerializeField]
    private float attackRange = 0.5f;

    [SerializeField]
    private float stunDamageAmount = 1f;

    [SerializeField]
    private float jumpAttackDMG;

    [SerializeField]
    private float[] attackDMG;

    private AttackDetails attackDetails;

    private CharacterController playerController;

    public LayerMask enemiesLayer;

    private SpriteRenderer spriteRenderer;

    private Animator myAnimator;
    public Animator MyAnimator { get => myAnimator; }

    public int combo = 0;

    public static PlayerCombat Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (isJumpAttacking && playerController.IsGrounded())
            Finish_Jump_Attack();
    }

    public void HandleAttacking()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);

        if (isJumpAttacking)
            attackDetails.damageAmount = jumpAttackDMG;
        else attackDetails.damageAmount = attackDMG[combo - 1];

        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;
        attackDetails.invincibleTime = 0.5f;

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.transform.SendMessage("TakeDamage", attackDetails);
        }
    }

    public void HandleInput()
    {
        if (Input.GetKey(KeyCode.X))
        {
            if (playerController.IsGrounded() && !isAttacking)
            {
                playerController.DisableMove();
                isAttacking = true;
                combo++;
                Debug.Log(combo);
                if (combo > 3)
                {
                    combo = 1;
                }
                timer = interval;
                myAnimator.SetTrigger("Attack");
                myAnimator.SetInteger("ComboCount", combo);
            }
            else if (!playerController.IsGrounded() && !isJumpAttacking)
            {
                isJumpAttacking = true;
                myAnimator.SetBool("JumpAttack", true);
            }
        }

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                combo = 0;
            }
        }    
    }

    public void Start_Combo()
    {
        isAttacking = false;
        if (combo < 2)
        {
            combo++;
        }
    }    

    public void Finish_Attack()
    {
        isAttacking = false;
        /*for (int i = 0; i < 2; i++)
        {
            myAnimator.SetBool("Attack" + i, false);
        }    */
        Debug.Log("Finished");
        //combo = 0;
        isAttacking = false;
    }

    public void Finish_Jump_Attack()
    {
        isJumpAttacking = false;
        myAnimator.SetBool("JumpAttack", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
