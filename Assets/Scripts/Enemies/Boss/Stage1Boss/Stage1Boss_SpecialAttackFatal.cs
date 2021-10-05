using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_SpecialAttackFatal : RangedAttackState
{
    private Stage1Boss boss;

    private Vector2 spawner1;
    private Vector2 spawner2;

    public Stage1Boss_SpecialAttackFatal(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, Transform attackPosition, D_RangedAttackState stateData)
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
        spawner1 = attackPosition.position;
        spawner2 = attackPosition.position;
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

            boss.incomingSpecialAttack = boss.specialAttack1;
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
        switch (GameManager.Instance.currentGameDifficulty)
        {
            case GameDifficulty.EASY:
                boss.StartCoroutine(AttackEasy());
                break;
            case GameDifficulty.NORMAL:
                boss.StartCoroutine(AttackNormal());
                break;
            case GameDifficulty.HARD:
                boss.StartCoroutine(AttackHard());
                break;
            case GameDifficulty.LUNATIC:
                boss.StartCoroutine(AttackLunatic());
                break;
        }
    }

    private IEnumerator AttackEasy()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 9; i++)
        {
            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner1, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner2, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            yield return new WaitForSeconds(0.1f);

            GameObject bulletOBJ = Bullet.GetBullet(spawner1, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command = bulletOBJ.AddComponent<BulletCommand>();

            GameObject bulletOBJ2 = Bullet.GetBullet(spawner2, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command2 = bulletOBJ2.AddComponent<BulletCommand>();

            command.update = update;
            command2.update = update2;

            void update()
            {
                if (command.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ.SetActive(false);
                }
            }

            void update2()
            {
                if (command2.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ2.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ2.SetActive(false);
                }
            }

            spawner1.x += 2f;
            spawner2.x -= 2f;
        }

        yield return new WaitForSeconds(10f);
        FinishAttack();
    }

    private IEnumerator AttackNormal()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 9; i++)
        {
            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner1, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner2, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            yield return new WaitForSeconds(0.1f);

            GameObject bulletOBJ = Bullet.GetBullet(spawner1, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command = bulletOBJ.AddComponent<BulletCommand>();

            GameObject bulletOBJ2 = Bullet.GetBullet(spawner2, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command2 = bulletOBJ2.AddComponent<BulletCommand>();

            command.update = update;
            command2.update = update2;

            void update()
            {
                if (command.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ.SetActive(false);
                }
            }

            void update2()
            {
                if (command2.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ2.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ2.SetActive(false);
                }
            }

            spawner1.x += 2f;
            spawner2.x -= 2f;
        }

        yield return new WaitForSeconds(10f);
        FinishAttack();
    }

    private IEnumerator AttackHard()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 9; i++)
        {
            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner1, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner2, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            yield return new WaitForSeconds(0.1f);

            GameObject bulletOBJ = Bullet.GetBullet(spawner1, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command = bulletOBJ.AddComponent<BulletCommand>();

            GameObject bulletOBJ2 = Bullet.GetBullet(spawner2, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command2 = bulletOBJ2.AddComponent<BulletCommand>();

            command.update = update;
            command2.update = update2;

            void update()
            {
                if (command.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ.SetActive(false);
                }
            }

            void update2()
            {
                if (command2.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ2.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ2.SetActive(false);
                }
            }

            spawner1.x += 2f;
            spawner2.x -= 2f;
        }

        yield return new WaitForSeconds(10f);
        FinishAttack();
    }

    private IEnumerator AttackLunatic()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 9; i++)
        {
            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner1, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            for (float j = 0; j < 2 * Mathf.PI; j += Mathf.PI / 16)
            {
                Bullet.GetBullet(spawner2, stateData.bulletShootTypes[0].bulletSpeed, j, stateData.bulletShootTypes[0].bulletLifeSpan, stateData.bulletShootTypes[0].bulletDamage,
                    stateData.bulletShootTypes[0].bulletType, stateData.bulletShootTypes[0].bulletColor, stateData.bulletShootTypes[0].destroyOnInvisible);
            }

            yield return new WaitForSeconds(0.1f);

            GameObject bulletOBJ = Bullet.GetBullet(spawner1, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command = bulletOBJ.AddComponent<BulletCommand>();

            GameObject bulletOBJ2 = Bullet.GetBullet(spawner2, stateData.bulletShootTypes[1].bulletSpeed, Mathf.PI / 2, stateData.bulletShootTypes[1].bulletAcceleration,
                                stateData.bulletShootTypes[1].bulletLifeSpan, stateData.bulletShootTypes[1].bulletDamage, stateData.bulletShootTypes[1].bulletType,
                                stateData.bulletShootTypes[1].bulletColor, stateData.bulletShootTypes[1].destroyOnInvisible);

            BulletCommand command2 = bulletOBJ2.AddComponent<BulletCommand>();

            command.update = update;
            command2.update = update2;

            void update()
            {
                if (command.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ.SetActive(false);
                }    
            }

            void update2()
            {
                if (command2.frame == 60)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float angle = Random.Range(235f * Mathf.Deg2Rad, 315f * Mathf.Deg2Rad);
                        Bullet.GetBullet(bulletOBJ2.transform.position, stateData.bulletShootTypes[2].bulletSpeed, angle, stateData.bulletShootTypes[2].bulletAcceleration,
                                stateData.bulletShootTypes[2].bulletLifeSpan, stateData.bulletShootTypes[2].bulletDamage, stateData.bulletShootTypes[2].bulletType,
                                stateData.bulletShootTypes[2].bulletColor, stateData.bulletShootTypes[2].destroyOnInvisible);
                    }
                    bulletOBJ2.SetActive(false);
                }
            }

            spawner1.x += 2f;
            spawner2.x -= 2f;
        }    

        yield return new WaitForSeconds(10f);
        FinishAttack();
    }
}
