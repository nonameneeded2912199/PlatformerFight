using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_DiamondChase : RangedAttackState
{
    Stage1Midboss boss;

    Stage1Midboss_Phase1 phase;

    BulletDetails initialBullet;

    BulletDetails chaseBullet;

    Coroutine attackingCoroutine;

    public Stage1Midboss_DiamondChase(FiniteStateMachine stateMachine, Stage1Midboss entity, string animBoolName, Transform attackPosition, Stage1Midboss_Phase1 phase, D_RangedAttackState stateData) 
        : base(stateMachine, entity, animBoolName, attackPosition, stateData)
    {
        this.boss = entity;
        this.phase = phase;
        //initalBullet = boss.ProjectilesDetails[1];
        //chaseBullet = boss.ProjectilesDetails[2];
        initialBullet = stateData.bulletDetails[0];
        chaseBullet = stateData.bulletDetails[1];
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
        boss.StopCoroutine(attackingCoroutine);
        phase.randomShootTime = 0;
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        boss.stateMachine.ChangeState(phase.teleportOutState);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
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
        switch (boss.GameStateSO.CurrentDifficulty)
        {
            case GameDifficulty.EASY:
                attackingCoroutine = boss.StartCoroutine(ChaseEasy());
                break;
            case GameDifficulty.NORMAL:
                attackingCoroutine = boss.StartCoroutine(ChaseNormal());
                break;
            case GameDifficulty.HARD:
                attackingCoroutine = boss.StartCoroutine(ChaseHard());
                break;
            case GameDifficulty.LUNATIC:
                attackingCoroutine = boss.StartCoroutine(ChaseLunatic());
                break;
        }
    }

    private IEnumerator ChaseEasy()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                for (float j = 0; j < Mathf.PI * 2; j += Mathf.PI * 2 / 5)
                {
                    Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, initialBullet.bulletSpeed, j,
                                initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan, initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                                initialBullet.hitRadius, initialBullet.bulletSprite, initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    BulletCommand changeDirectionCommand = bullet.gameObject.AddComponent<BulletCommand>();
                    changeDirectionCommand.update = update;
                    void update()
                    {
                        if (changeDirectionCommand.frame == 50)
                        {
                            Player player = GameObject.FindObjectOfType<Player>();
                            float angleToPlayer = Mathf.Atan2(player.transform.position.y - bullet.transform.position.y,
                                player.transform.position.x - bullet.transform.position.x);
                            bullet.ChangeSprite(chaseBullet.bulletSprite, chaseBullet.animatorOverrideController);
                            bullet.SetAttributes(bullet.transform.position, chaseBullet.bulletSpeed * 0.75f, angleToPlayer,
                                chaseBullet.bulletAcceleration, chaseBullet.bulletLifeSpan, boss.CharacterStats.CurrentAttack * chaseBullet.damageMultiplier, 0.5f,
                                0, chaseBullet.destroyOnInvisible);
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(2f);
        FinishAttack();
    }

    private IEnumerator ChaseNormal()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                for (float j = 0; j < Mathf.PI * 2; j += Mathf.PI * 2 / 10)
                {
                    Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, initialBullet.bulletSpeed, j,
                                initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan, initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                                initialBullet.hitRadius, initialBullet.bulletSprite, initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    BulletCommand changeDirectionCommand = bullet.gameObject.AddComponent<BulletCommand>();
                    changeDirectionCommand.update = update;
                    void update()
                    {
                        if (changeDirectionCommand.frame == 50)
                        {
                            Player player = GameObject.FindObjectOfType<Player>();
                            float angleToPlayer = Mathf.Atan2(player.transform.position.y - bullet.transform.position.y,
                                player.transform.position.x - bullet.transform.position.x);
                            bullet.ChangeSprite(chaseBullet.bulletSprite, chaseBullet.animatorOverrideController);
                            bullet.SetAttributes(bullet.transform.position, chaseBullet.bulletSpeed, angleToPlayer,
                                chaseBullet.bulletAcceleration, chaseBullet.bulletLifeSpan, boss.CharacterStats.CurrentAttack * chaseBullet.damageMultiplier, 0.5f,
                                0, chaseBullet.destroyOnInvisible);
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(2f);
        FinishAttack();
    }

    private IEnumerator ChaseHard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                for (float j = 0; j < Mathf.PI * 2; j += Mathf.PI * 2 / 15)
                {
                    Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, initialBullet.bulletSpeed, j,
                                initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan, initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                                initialBullet.hitRadius, initialBullet.bulletSprite, initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    BulletCommand changeDirectionCommand = bullet.gameObject.AddComponent<BulletCommand>();
                    changeDirectionCommand.update = update;
                    void update()
                    {
                        if (changeDirectionCommand.frame == 70)
                        {
                            Player player = GameObject.FindObjectOfType<Player>();
                            float angleToPlayer = Mathf.Atan2(player.transform.position.y - bullet.transform.position.y,
                                player.transform.position.x - bullet.transform.position.x);
                            bullet.ChangeSprite(chaseBullet.bulletSprite, chaseBullet.animatorOverrideController);
                            bullet.SetAttributes(bullet.transform.position, chaseBullet.bulletSpeed, angleToPlayer,
                                chaseBullet.bulletAcceleration, chaseBullet.bulletLifeSpan, boss.CharacterStats.CurrentAttack * chaseBullet.damageMultiplier, 0.5f,
                                0, chaseBullet.destroyOnInvisible);
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(2f);
        FinishAttack();
    }

    private IEnumerator ChaseLunatic()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                for (float j = 0; j < Mathf.PI * 2; j += Mathf.PI * 2 / 20)
                {
                    Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, initialBullet.bulletSpeed * 1.5f, j,
                                initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan, initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                                initialBullet.hitRadius, initialBullet.bulletSprite, initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    BulletCommand changeDirectionCommand = bullet.gameObject.AddComponent<BulletCommand>();
                    changeDirectionCommand.update = update;
                    void update()
                    {
                        if (changeDirectionCommand.frame == 90)
                        {
                            Player player = GameObject.FindObjectOfType<Player>();
                            float angleToPlayer = Mathf.Atan2(player.transform.position.y - bullet.transform.position.y,
                                player.transform.position.x - bullet.transform.position.x);
                            bullet.ChangeSprite(chaseBullet.bulletSprite, chaseBullet.animatorOverrideController);
                            bullet.SetAttributes(bullet.transform.position, chaseBullet.bulletSpeed * 1.5f, angleToPlayer,
                                chaseBullet.bulletAcceleration, chaseBullet.bulletLifeSpan, boss.CharacterStats.CurrentAttack * chaseBullet.damageMultiplier, 0.5f,
                                0, chaseBullet.destroyOnInvisible);
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(2f);
        FinishAttack();
    }
}
