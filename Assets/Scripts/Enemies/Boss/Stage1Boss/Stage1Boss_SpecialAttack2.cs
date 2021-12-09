using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_SpecialAttack2 : RangedAttackState
{
    private Stage1Boss boss;
    private float startAttackTime;

    Vector2 attackPosLeft;
    Vector2 attackPosRight;

    public Stage1Boss_SpecialAttack2(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, Transform attackPosition, D_RangedAttackState stateData)
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
        attackPosLeft = boss.specialAttack2LeftPoint.position;
        attackPosRight = boss.specialAttack2RightPoint.position;
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

            boss.incomingSpecialAttack = boss.specialAttack3;
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
        startAttackTime = Time.time;
        //switch (GameManager.Instance.currentGameDifficulty)
        //{
        //    case GameDifficulty.EASY:
        //        boss.StartCoroutine(AttackEasy());
        //        break;
        //    case GameDifficulty.NORMAL:
        //        boss.StartCoroutine(AttackNormal());
        //        break;
        //    case GameDifficulty.HARD:
        //        boss.StartCoroutine(AttackHard());
        //        break;
        //    case GameDifficulty.LUNATIC:
        //        boss.StartCoroutine(AttackLunatic());
        //        break;
        //}
    }

    private IEnumerator AttackEasy()
    {
        float angle = 0;
        float angle2 = 0;

        while (Time.time <= startAttackTime + stateData.attackTime)
        {
            /*Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                            stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                            stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                            stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

            Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                            stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                            stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                            stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);*/

            angle += Mathf.PI / 4;

            angle2 -= Mathf.PI / 4;

            attackPosLeft.x += 1.5f * Time.deltaTime;
            attackPosRight.x -= 1.5f * Time.deltaTime;

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        FinishAttack();
    }

    private IEnumerator AttackNormal()
    {
        float angle = 0;
        float angle2 = 0;

        while (Time.time <= startAttackTime + stateData.attackTime)
        {
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

            angle += Mathf.PI / 8;

            angle2 -= Mathf.PI / 8;

            attackPosLeft.x += 1.5f * Time.deltaTime;
            attackPosRight.x -= 1.5f * Time.deltaTime;

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);
        FinishAttack();
    }

    private IEnumerator AttackHard()
    {
        float angle = 0;
        float angle2 = 0;

        while (Time.time <= startAttackTime + stateData.attackTime)
        {
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

            angle += Mathf.PI / 12;

            angle2 -= Mathf.PI / 12;

            attackPosLeft.x += 1.5f * Time.deltaTime;
            attackPosRight.x -= 1.5f * Time.deltaTime;

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);
        FinishAttack();
    }

    private IEnumerator AttackLunatic()
    {
        float angle = 0;
        float angle2 = 0;

        while (Time.time <= startAttackTime + stateData.attackTime)
        {
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosLeft, stateData.bulletShootTypes[0].bulletSpeed, angle + 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 2 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            //Bullet.GetBullet(BulletOwner.Enemy, attackPosRight, stateData.bulletShootTypes[0].bulletSpeed, angle2 - 4 * Mathf.PI / 3, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
            //                stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

            angle += Mathf.PI / 18;

            angle2 -= Mathf.PI / 18;

            attackPosLeft.x += 1.5f * Time.deltaTime;
            attackPosRight.x -= 1.5f * Time.deltaTime;

            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(1f);
        FinishAttack();
    }
}
