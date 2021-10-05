using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, knockbackSpeedX, knockbackSpeedY, knockbackDuration;

    [SerializeField]
    private float knockbackDeathSpeedX, knockbackDeathSpeedY, deathTorque;

    private float currentHealth, knockbackStart;

    [SerializeField]
    private bool applyKnockback;

    private bool playerOnLeft, knockback;

    private CharacterController player;

    private GameObject aliveGO, brokenTopGO, brokenBottomGO;
    private Rigidbody2D aliveRB, brokenTopRB, brokenBottomRB;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        player = GameObject.Find("Player").GetComponent<CharacterController>();

        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("BrokenTop").gameObject;
        brokenBottomGO = transform.Find("BrokenBottom").gameObject;

        animator = aliveGO.GetComponent<Animator>();

        aliveRB = aliveGO.GetComponent<Rigidbody2D>();
        brokenTopRB = brokenTopGO.GetComponent<Rigidbody2D>();
        brokenBottomRB = brokenBottomGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBottomGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckKnockback();
    }

    private void TakeDamage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];
        playerOnLeft = attackDetails[1] > transform.position.x ? false : true;

        animator.SetBool("PlayerOnLeft", playerOnLeft);
        animator.SetTrigger("Damage");

        if (applyKnockback && currentHealth > 0.0f)
        {
            ApplyKnockback(attackDetails[1] < transform.position.x ? 1 : -1);
        }

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    } 
    
    private void ApplyKnockback(float playerDir)
    {
        knockback = true;
        knockbackStart = Time.time;
        aliveRB.velocity = new Vector2(knockbackSpeedX * playerDir, knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            aliveRB.velocity = new Vector2(0.0f, aliveRB.velocity.y);
        }
    }

    private void Die()
    {
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBottomGO.SetActive(true);

        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBottomGO.transform.position = aliveGO.transform.position;

        int playerDir = player.FacingRight ? 1 : -1;

        brokenBottomRB.velocity = new Vector2(knockbackSpeedX * playerDir, knockbackSpeedY);
        brokenTopRB.velocity = new Vector2(knockbackDeathSpeedX * playerDir, knockbackDeathSpeedY);

        brokenBottomRB.AddTorque(deathTorque * -playerDir, ForceMode2D.Impulse);
    }
}
