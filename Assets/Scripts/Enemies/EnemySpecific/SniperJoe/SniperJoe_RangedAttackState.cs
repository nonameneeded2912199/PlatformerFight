using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperJoe_RangedAttackState : RangedAttackState
{
    private SniperJoe enemy;
    private float shootCooldown = 3f;
    private float lastTimeShoot = 0;
    private bool isAttacking = false;

    private bool isPlayerInCircleRange;

    public SniperJoe_RangedAttackState(FiniteStateMachine stateMachine, SniperJoe enemy, string animBoolName, Transform attackPosition, D_RangedAttackState stateData)
        : base(stateMachine, enemy, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.shield.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.shield.SetActive(true);
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        if (isAnimationFinished)
        {
            if (enemy.CheckPlayerInMaxArgoRange())
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        enemy.StartCoroutine(ShootPlayer());
    }

    private IEnumerator ShootPlayer()
    {
        isAttacking = true;
        float targetAngle = enemy.facingRight ? 0 : Mathf.PI;
        float startingAngle = 0;
        BulletDetails[] bulletDetails = stateData.bulletDetails;

        switch (enemy.GameStateSO.CurrentDifficulty)
        {
            case GameDifficulty.EASY:
                /*Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletLifeSpan,
                    stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor);*/
                enemy.BulletEventChannel.RaiseBulletEvent(entity.tag, attackPosition.position, bulletDetails[0].bulletSpeed, targetAngle, bulletDetails[0].bulletAcceleration,
                    bulletDetails[0].bulletLifeSpan, bulletDetails[0].damageMultiplier * enemy.CharacterStats.CurrentAttack / 2, 0.5f, bulletDetails[0].hitRadius, 
                    bulletDetails[0].bulletSprite, bulletDetails[0].animatorOverrideController);               
                break;
            case GameDifficulty.NORMAL:
                enemy.BulletEventChannel.RaiseBulletEvent(entity.tag, attackPosition.position, bulletDetails[0].bulletSpeed, targetAngle, bulletDetails[0].bulletAcceleration,
                    bulletDetails[0].bulletLifeSpan, bulletDetails[0].damageMultiplier * enemy.CharacterStats.CurrentAttack, 0.5f, bulletDetails[0].hitRadius,
                    bulletDetails[0].bulletSprite, bulletDetails[0].animatorOverrideController);                
                break;
            case GameDifficulty.HARD:
                startingAngle = targetAngle - Mathf.PI / 18;
                for (float i = 0; i < 3; i++)
                {
                    enemy.BulletEventChannel.RaiseBulletEvent(entity.tag, attackPosition.position, bulletDetails[0].bulletSpeed, startingAngle, bulletDetails[0].bulletAcceleration,
                    bulletDetails[0].bulletLifeSpan, bulletDetails[0].damageMultiplier * enemy.CharacterStats.CurrentAttack, 0.5f, bulletDetails[0].hitRadius,
                    bulletDetails[0].bulletSprite, bulletDetails[0].animatorOverrideController);                    
                    startingAngle += Mathf.PI / 18;
                }
                break;
            case GameDifficulty.LUNATIC:
                startingAngle = targetAngle - Mathf.PI / 4;
                float delta = Mathf.PI / 16;
                for (float i = 0; i < 8; i++)
                {
                    enemy.BulletEventChannel.RaiseBulletEvent(entity.tag, attackPosition.position, bulletDetails[0].bulletSpeed * 2, startingAngle, bulletDetails[0].bulletAcceleration,
                    bulletDetails[0].bulletLifeSpan, bulletDetails[0].damageMultiplier * enemy.CharacterStats.CurrentAttack * 2, 0.5f, bulletDetails[0].hitRadius,
                    bulletDetails[0].bulletSprite, bulletDetails[0].animatorOverrideController);                   
                    startingAngle += delta;
                }
                break;
        }

        yield return new WaitForSeconds(0.5f);

        FinishAttack();
    }
}
