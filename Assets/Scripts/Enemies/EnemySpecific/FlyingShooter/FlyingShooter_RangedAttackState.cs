using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShooter_RangedAttackState : RangedAttackState
{
    private FlyingShooter enemy;
    private float shootCooldown = 3f;
    private float lastTimeShoot = 0;
    private bool isAttacking = false;

    private bool isPlayerInCircleRange;

    public FlyingShooter_RangedAttackState(FiniteStateMachine stateMachine, FlyingShooter enemy, string animBoolName, Transform attackPosition, D_RangedAttackState stateData)
        : base(stateMachine, enemy, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInCircleRange = enemy.CheckPlayerInMaxRangedCircle();
    }

    public override void Enter()
    {
        base.Enter();
        //isAttacking = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        isAttacking = false;
        lastTimeShoot = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        int facingDirection = enemy.facingRight ? 1 : -1;
        if ((Player.Instance.transform.position.x > enemy.transform.position.x && facingDirection == -1)
            || (Player.Instance.transform.position.x < enemy.transform.position.x && facingDirection == 1))
        {
            enemy.Flip();
        }    

        if (Time.time >= lastTimeShoot + shootCooldown && !isAttacking)
        {
            if (isPlayerInCircleRange)
                TriggerAttack();
            else if (!isPlayerInCircleRange)
                stateMachine.ChangeState(enemy.moveState);
        }
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
        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 myPos = enemy.transform.position;
        float angle = Mathf.Atan2(playerPos.y - myPos.y, playerPos.x - myPos.x);
        BulletDetails bulletDetails = stateData.bulletDetails[0];
        for (int j = 0; j < 5; j++)
        {
            switch (entity.GameStateSO.CurrentDifficulty)
            {
                case GameDifficulty.EASY:
                    /*Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletLifeSpan,
                        stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor);*/
                    for (int i = 0; i < 1; i++)
                    {
                        //var bulletCom = PoolManager.SpawnObject(GameManager.Instance.CommonBullet).GetComponent<Bullet>();
                        //var bulletCom = PoolManager.SpawnObject(Bullet.OriginalBullet).GetComponent<Bullet>();
                        entity.BulletEventChannel.RaiseBulletEvent(enemy.tag, attackPosition.position, bulletDetails.bulletSpeed, angle, 
                            bulletDetails.bulletAcceleration, bulletDetails.bulletLifeSpan, bulletDetails.damageMultiplier * enemy.CharacterStats.CurrentAttack, 
                            0.5f, bulletDetails.hitRadius, bulletDetails.bulletSprite,
                            bulletDetails.animatorOverrideController);                        
                    }
                    break;
                case GameDifficulty.NORMAL:
                    for (float i = 0; i < 2 * Mathf.PI; i += (2f * Mathf.PI / 6))
                    {                        
                        entity.BulletEventChannel.RaiseBulletEvent(enemy.tag, attackPosition.position, bulletDetails.bulletSpeed, angle + i, 
                            bulletDetails.bulletAcceleration, bulletDetails.bulletLifeSpan, bulletDetails.damageMultiplier * enemy.CharacterStats.CurrentAttack, 
                            0.5f, bulletDetails.hitRadius, bulletDetails.bulletSprite, bulletDetails.animatorOverrideController);                      
                    }
                    break;
                case GameDifficulty.HARD:
                    for (float i = 0; i < 2 * Mathf.PI; i += (2f * Mathf.PI / 12))
                    {
                        entity.BulletEventChannel.RaiseBulletEvent(enemy.tag, attackPosition.position, bulletDetails.bulletSpeed, angle + i, 
                            bulletDetails.bulletAcceleration, bulletDetails.bulletLifeSpan, bulletDetails.damageMultiplier * enemy.CharacterStats.CurrentAttack, 
                            0.5f, bulletDetails.hitRadius, bulletDetails.bulletSprite, bulletDetails.animatorOverrideController);                        
                    }
                    break;
                case GameDifficulty.LUNATIC:
                    for (float i = 0; i < 2 * Mathf.PI; i += (2f * Mathf.PI / 18))
                    {
                        entity.BulletEventChannel.RaiseBulletEvent(enemy.tag, attackPosition.position, bulletDetails.bulletSpeed, angle + i, 
                            bulletDetails.bulletAcceleration, bulletDetails.bulletLifeSpan, bulletDetails.damageMultiplier * enemy.CharacterStats.CurrentAttack * 2, 0.5f, 
                            bulletDetails.hitRadius, bulletDetails.bulletSprite, bulletDetails.animatorOverrideController);                       
                    }
                    break;
            }

            yield return new WaitForSeconds(0.2f);
        }

        FinishAttack();
    }
}
