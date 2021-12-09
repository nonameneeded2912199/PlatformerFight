using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_SpecialAttack3 : MoveState
{
    private Stage1Boss boss;
    private D_RangedAttackState rangedData;
    float attackDuration;

    public Stage1Boss_SpecialAttack3(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_MoveState stateData, D_RangedAttackState rangedData) :
        base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
        this.rangedData = rangedData;
    }

    public override void Enter()
    {
        base.Enter();
        attackDuration = Random.Range(stateData.minMoveTime, stateData.maxMoveTime);

        //switch (GameManager.Instance.currentGameDifficulty)
        //{
        //    case GameDifficulty.EASY:
        //        boss.StartCoroutine(CyclingBarrierEasy());
        //        break;
        //    case GameDifficulty.NORMAL:
        //        boss.StartCoroutine(CyclingBarrierNormal());
        //        break;
        //    case GameDifficulty.HARD:
        //        boss.StartCoroutine(CyclingBarrierHard());
        //        break;
        //    case GameDifficulty.LUNATIC:
        //        boss.StartCoroutine(CyclingBarrierLunatic());
        //        break;
        //}    
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (wallCheck || !ledgeCheck)
        {
            boss.Flip();
            boss.SetVelocity(stateData.movementSpeed);
        }

        if (Time.time >= startTime + attackDuration + 5f)
        {
            if (playerInMinAgroRangeCheck)
            {               
                stateMachine.ChangeState(boss.meleeAttack);
            }
            else
            {
                stateMachine.ChangeState(boss.rangedAttack);
            }
            boss.incomingSpecialAttack = boss.moveToCenter;
        }    
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private IEnumerator CyclingBarrierEasy()
    {
        while (Time.time < startTime + attackDuration)
        {
            for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 4)
            {
                //GameObject bulletOBJ = Bullet.GetBullet(BulletOwner.Enemy, boss.transform.position, rangedData.bulletShootTypes[0].bulletSpeed, i, rangedData.bulletShootTypes[0].bulletAcceleration,
                //        rangedData.bulletShootTypes[0].bulletLifeSpan, rangedData.bulletShootTypes[0].bulletDamage, rangedData.bulletShootTypes[0].bulletType,
                //        rangedData.bulletShootTypes[0].bulletColor, rangedData.bulletShootTypes[0].destroyOnInvisible);

                //BulletCommand bulletCommand = bulletOBJ.AddComponent<BulletCommand>();
                //void update()
                //{
                //    Bullet bulletCom = bulletOBJ.GetComponent<Bullet>();
                //    if (bulletCommand.frame >= 30)
                //    {
                //        bulletCom.Direction += Mathf.Deg2Rad;
                //    }
                //}

                //bulletCommand.update = update;
            }
            yield return new WaitForSeconds(4f);
        }
    }

    private IEnumerator CyclingBarrierNormal()
    {
        while (Time.time < startTime + attackDuration)
        {
            for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 8)
            {
                //GameObject bulletOBJ = Bullet.GetBullet(BulletOwner.Enemy, boss.transform.position, rangedData.bulletShootTypes[0].bulletSpeed, i, rangedData.bulletShootTypes[0].bulletAcceleration,
                //        rangedData.bulletShootTypes[0].bulletLifeSpan, rangedData.bulletShootTypes[0].bulletDamage, rangedData.bulletShootTypes[0].bulletType,
                //        rangedData.bulletShootTypes[0].bulletColor, rangedData.bulletShootTypes[0].destroyOnInvisible);

                //BulletCommand bulletCommand = bulletOBJ.AddComponent<BulletCommand>();
                //void update()
                //{
                //    Bullet bulletCom = bulletOBJ.GetComponent<Bullet>();
                //    if (bulletCommand.frame >= 30)
                //    {
                //        bulletCom.Direction += Mathf.Deg2Rad;
                //    }
                //}

                //bulletCommand.update = update;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private IEnumerator CyclingBarrierHard()
    {
        while (Time.time < startTime + attackDuration)
        {
            for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 12)
            {
                //GameObject bulletOBJ = Bullet.GetBullet(BulletOwner.Enemy, boss.transform.position, rangedData.bulletShootTypes[0].bulletSpeed, i, rangedData.bulletShootTypes[0].bulletAcceleration,
                //        rangedData.bulletShootTypes[0].bulletLifeSpan, rangedData.bulletShootTypes[0].bulletDamage, rangedData.bulletShootTypes[0].bulletType,
                //        rangedData.bulletShootTypes[0].bulletColor, rangedData.bulletShootTypes[0].destroyOnInvisible);

                //BulletCommand bulletCommand = bulletOBJ.AddComponent<BulletCommand>();
                //void update()
                //{
                //    Bullet bulletCom = bulletOBJ.GetComponent<Bullet>();
                //    if (bulletCommand.frame >= 30)
                //    {
                //        bulletCom.Direction += Mathf.Deg2Rad;
                //    }
                //}

                //bulletCommand.update = update;
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator CyclingBarrierLunatic()
    {
        while (Time.time < startTime + attackDuration)
        {
            for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 16)
            {
                //GameObject bulletOBJ = Bullet.GetBullet(BulletOwner.Enemy, boss.transform.position, rangedData.bulletShootTypes[0].bulletSpeed, i, rangedData.bulletShootTypes[0].bulletAcceleration,
                //        rangedData.bulletShootTypes[0].bulletLifeSpan, rangedData.bulletShootTypes[0].bulletDamage, rangedData.bulletShootTypes[0].bulletType,
                //        rangedData.bulletShootTypes[0].bulletColor, rangedData.bulletShootTypes[0].destroyOnInvisible);

                //BulletCommand bulletCommand = bulletOBJ.AddComponent<BulletCommand>();
                //void update()
                //{
                //    Bullet bulletCom = bulletOBJ.GetComponent<Bullet>();
                //    if (bulletCommand.frame >= 30)
                //    {
                //        bulletCom.Direction += Mathf.Deg2Rad;
                //    }
                //}

                //bulletCommand.update = update;
            }
            yield return new WaitForSeconds(1f);
        }           
    }

}
