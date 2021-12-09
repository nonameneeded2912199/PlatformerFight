using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_RangedAttack : RangedAttackState
{
    private Stage1Boss boss;

    public Stage1Boss_RangedAttack(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, Transform attackPosition, D_RangedAttackState stateData)
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
            stateMachine.ChangeState(boss.incomingSpecialAttack);
        }

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        int facingDirection = boss.facingRight ? 1 : -1;
        if ((Player.Instance.transform.position.x > boss.transform.position.x && facingDirection == -1)
            || (Player.Instance.transform.position.x < boss.transform.position.x && facingDirection == 1))
        {
            boss.Flip();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
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

    public IEnumerator AttackEasy()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Player.Instance.isActiveAndEnabled)
            {
                Vector2 playerPos = Player.Instance.transform.position;
                Vector2 myPos = boss.transform.position;
                float angle = Mathf.Atan2(playerPos.y - myPos.y, playerPos.x - myPos.x);
                /*Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletAcceleration,
                    stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType,
                    stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);*/

                yield return new WaitForSeconds(1f);
            } 
            else
            {
                break;
            }                
        }

        yield return new WaitForSeconds(3f);
        FinishAttack();
    }

    public IEnumerator AttackNormal()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Player.Instance.isActiveAndEnabled)
            {
                Vector2 playerPos = Player.Instance.transform.position;
                Vector2 myPos = boss.transform.position;
                float angle = Mathf.Atan2(playerPos.y - myPos.y, playerPos.x - myPos.x);
                //Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletAcceleration,
                //    stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType,
                //    stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(3f);
        FinishAttack();
    }

    public IEnumerator AttackHard()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Player.Instance.isActiveAndEnabled)
            {
                //Vector2 playerPos = Player.Instance.transform.position;
                //Vector2 myPos = boss.transform.position;
                //float angle = Mathf.Atan2(playerPos.y - myPos.y, playerPos.x - myPos.x);
                //GameObject bulletOBJ = Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletAcceleration,
                //    stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType,
                //    stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

                //BulletCommand command = bulletOBJ.AddComponent<BulletCommand>();
                //void update()
                //{
                //    if (command.frame == 45)
                //    {
                //        for (float j = 0; j < 2 * Mathf.PI; j += 2f * Mathf.PI / 8f)
                //        {
                //            Bullet.GetBullet(BulletOwner.Enemy, bulletOBJ.transform.position, stateData.bulletShootTypes[1].bulletSpeed, j, stateData.bulletShootTypes[1].bulletAcceleration,
                //                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                //                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);
                //        }

                //        bulletOBJ.SetActive(false);
                //    }    
                //}
                //command.update = update;                

                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(6f);
        FinishAttack();
    }

    public IEnumerator AttackLunatic()
    {
        for (int i = 0; i < 8; i++)
        {
            if (Player.Instance.isActiveAndEnabled)
            {
                //Vector2 playerPos = Player.Instance.transform.position;
                //Vector2 myPos = boss.transform.position;
                //float angle = Mathf.Atan2(playerPos.y - myPos.y, playerPos.x - myPos.x);
                //GameObject bulletOBJ = Bullet.GetBullet(BulletOwner.Enemy, attackPosition.position, stateData.bulletShootTypes[0].bulletSpeed, angle, stateData.bulletShootTypes[0].bulletAcceleration,
                //    stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage, stateData.bulletShootTypes[0].bulletType,
                //    stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);

                //BulletCommand command = bulletOBJ.AddComponent<BulletCommand>();
                //void update()
                //{
                //    if (command.frame == 45)
                //    {
                //        for (float j = 0; j < 2 * Mathf.PI; j += 2f * Mathf.PI / 8f)
                //        {
                //            GameObject bulletOBJChild = Bullet.GetBullet(BulletOwner.Enemy, bulletOBJ.transform.position, stateData.bulletShootTypes[1].bulletSpeed, j, stateData.bulletShootTypes[1].bulletAcceleration,
                //                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                //                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

                //            BulletCommand commandChild = bulletOBJChild.AddComponent<BulletCommand>();

                //            void updateChild()
                //            {
                //                if (commandChild.frame == 60)
                //                {
                //                    Vector2 playerPos = Player.Instance.transform.position;
                //                    Vector2 myPos = bulletOBJChild.transform.position;
                //                    float angle = Mathf.Atan2(playerPos.y - myPos.y, playerPos.x - myPos.x);

                //                    bulletOBJChild.GetComponent<Bullet>().Direction = angle;
                //                }    
                //            }

                //            commandChild.update = updateChild;
                //        }

                //        bulletOBJ.SetActive(false);
                //    }
                //}
                //command.update = update;

                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(6f);
        FinishAttack();
    }
}
