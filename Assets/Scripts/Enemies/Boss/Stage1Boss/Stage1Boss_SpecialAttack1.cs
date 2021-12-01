using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_SpecialAttack1 : RangedAttackState
{
    private Stage1Boss boss;

    public Stage1Boss_SpecialAttack1(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, Transform attackPosition, D_RangedAttackState stateData)
        : base(stateMachine, boss, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        TriggerAttack();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(boss.meleeAttack);
            }
            else
            {
                stateMachine.ChangeState(boss.rangedAttack);
            }

            boss.incomingSpecialAttack = boss.specialAttack2;
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
        if (Vector3.Distance(boss.specialAttack1LeftPoint.position, boss.transform.position) > Vector3.Distance(boss.specialAttack1RightPoint.position, boss.transform.position))
        {
            boss.StartCoroutine(AttackLeftFirst());
        }
        else
        {
            boss.StartCoroutine(AttackRightFirst());
        }    

    }

    private IEnumerator AttackLeftFirst()
    {
        Vector2 position = boss.specialAttack1LeftPoint.position;
        for (int i = 0; i < 10; i++)
        {
            //switch (GameManager.Instance.currentGameDifficulty)
            //{
            //    case GameDifficulty.EASY:
            //        for (float j = Mathf.PI / 4; j < 2 * Mathf.PI; j += Mathf.PI / 2)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.NORMAL:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 8)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.HARD:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 12)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.LUNATIC:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 20)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //}           

            yield return new WaitForSeconds(0.5f);
            position.x += 1.5f * Mathf.Cos(0f);
        }

        yield return new WaitForSeconds(1f);

        position = boss.specialAttack1RightPoint.position;
        for (int i = 0; i < 10; i++)
        {
            //switch (GameManager.Instance.currentGameDifficulty)
            //{
            //    case GameDifficulty.EASY:
            //        for (float j = Mathf.PI / 4; j < 2 * Mathf.PI; j += Mathf.PI / 2)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.NORMAL:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 8)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.HARD:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 12)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.LUNATIC:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 20)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //}

            yield return new WaitForSeconds(0.5f);
            position.x += 1.5f * Mathf.Cos(Mathf.PI);
        }

        yield return new WaitForSeconds(5f);

        FinishAttack();
    }

    private IEnumerator AttackRightFirst()
    {
        Vector2 position = boss.specialAttack1RightPoint.position;
        for (int i = 0; i < 10; i++)
        {
            //switch (GameManager.Instance.currentGameDifficulty)
            //{
            //    case GameDifficulty.EASY:
            //        for (float j = Mathf.PI / 4; j < 2 * Mathf.PI; j += Mathf.PI / 2)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.NORMAL:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 8)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.HARD:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 12)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.LUNATIC:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 20)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //}

            yield return new WaitForSeconds(0.5f);
            position.x += 1.5f * Mathf.Cos(Mathf.PI);
        }

        yield return new WaitForSeconds(1f);

        position = boss.specialAttack1LeftPoint.position;
        for (int i = 0; i < 10; i++)
        {
            //switch (GameManager.Instance.currentGameDifficulty)
            //{
            //    case GameDifficulty.EASY:
            //        for (float j = Mathf.PI / 4; j < 2 * Mathf.PI; j += Mathf.PI / 2)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.NORMAL:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 8)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.HARD:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 12)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //    case GameDifficulty.LUNATIC:
            //        for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 20)
            //        {
            //            Bullet.GetBullet(BulletOwner.Enemy, position, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //        }
            //        break;
            //}

            yield return new WaitForSeconds(0.5f);
            position.x += 1.5f * Mathf.Cos(0f);
        }

        yield return new WaitForSeconds(5f);

        FinishAttack();
    }
}
