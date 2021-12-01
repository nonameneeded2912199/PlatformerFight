using CharacterThings;
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
        for (int j = 0; j < 5; j++)
        {
            switch (GameManager.Instance.currentGameDifficulty)
            {
                case GameDifficulty.EASY:
                    /*Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletLifeSpan,
                        stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor);*/
                    for (int i = 0; i < 1; i++)
                    {
                        var bulletCom = PoolManager.SpawnObject(GameManager.Instance.CommonBullet).GetComponent<Bullet>();
                        bulletCom.SetAllegiance(enemy.tag);
                        bulletCom.SetAttributes(attackPosition.position, 8, angle, 0, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, 0.5f);
                    }               
                    break;
                case GameDifficulty.NORMAL:
                    for (float i = 0; i < 2 * Mathf.PI; i += (2f * Mathf.PI / 6))
                    {
                        //Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle + i, stateData.bulletShootTypes[0].bulletLifeSpan,
                        // stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor,
                        // stateData.bulletShootTypes[0].destroyOnInvisible);
                        var bulletCom = PoolManager.SpawnObject(GameManager.Instance.CommonBullet).GetComponent<Bullet>();
                        bulletCom.SetAllegiance(enemy.tag);
                        bulletCom.SetAttributes(attackPosition.position, 8, angle + i, 0, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, 0.5f);
                    }
                    break;
                case GameDifficulty.HARD:
                    for (float i = 0; i < 2 * Mathf.PI; i += (2f * Mathf.PI / 12))
                    {
                        //Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle + i, stateData.bulletShootTypes[0].bulletLifeSpan,
                        //stateData.bulletShootTypes[0].bulletDamage * 1.5f, stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor,
                        //stateData.bulletShootTypes[0].destroyOnInvisible);
                        var bulletCom = PoolManager.SpawnObject(GameManager.Instance.CommonBullet).GetComponent<Bullet>();
                        bulletCom.SetAllegiance(enemy.tag);
                        bulletCom.SetAttributes(attackPosition.position, 8, angle + i, 0, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, 0.5f);
                    }
                    break;
                case GameDifficulty.LUNATIC:
                    for (float i = 0; i < 2 * Mathf.PI; i += (2f * Mathf.PI / 18))
                    {
                        //Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle + i, stateData.bulletShootTypes[0].bulletLifeSpan,
                        //stateData.bulletShootTypes[0].bulletDamage * 2f, stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor,
                        //stateData.bulletShootTypes[0].destroyOnInvisible);
                        var bulletCom = PoolManager.SpawnObject(GameManager.Instance.CommonBullet).GetComponent<Bullet>();
                        bulletCom.SetAllegiance(enemy.tag);
                        bulletCom.SetAttributes(attackPosition.position, 8, angle + i, 0, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, 0.5f);
                    }
                    break;
            }

            yield return new WaitForSeconds(0.2f);
        }

        FinishAttack();
    }
}
