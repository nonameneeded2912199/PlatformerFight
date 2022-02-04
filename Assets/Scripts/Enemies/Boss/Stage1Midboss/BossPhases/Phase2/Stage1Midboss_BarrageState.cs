using PlatformerFight.CharacterThings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stage1Midboss_BarrageState : RangedAttackState
{
    private Stage1Midboss boss;

    private Stage1Midboss_Phase2 phase;

    private Coroutine attackingCoroutine;

    private BulletDetails initialBullet;

    private BulletDetails transformed1;

    private BulletDetails transformed2;

    public Stage1Midboss_BarrageState(FiniteStateMachine stateMachine, Stage1Midboss entity, string animBoolName, Stage1Midboss_Phase2 phase, Transform attackPosition, D_RangedAttackState stateData)
        : base(stateMachine, entity, animBoolName, attackPosition, stateData)
    {
        this.boss = entity;
        this.phase = phase;

        initialBullet = stateData.bulletDetails[0];
        transformed1 = stateData.bulletDetails[1];
        transformed2 = stateData.bulletDetails[2];
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
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        boss.transform.DOMove(new Vector3(boss.transform.position.x, boss.flightLevel1.position.z, boss.transform.position.z), 0.5f)
                .OnComplete(() =>
                {
                    stateMachine.ChangeState(phase.moveState);
                });
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
        switch (boss.thisDifficulty)
        {
            case GameDifficulty.EASY:
            //attackingCoroutine = boss.StartCoroutine(BarrageEasy());
            //break;
            case GameDifficulty.NORMAL:
                //attackingCoroutine = boss.StartCoroutine(BarrageNormal());
                //break;
            case GameDifficulty.HARD:
                attackingCoroutine = boss.StartCoroutine(BarrageHard());
                break;
            case GameDifficulty.LUNATIC:
                attackingCoroutine = boss.StartCoroutine(BarrageLunatic());
                break;
        }
    }

    //private IEnumerator BarrageNormal()
    //{
    //    Player target = GameObject.FindObjectOfType<Player>();
    //    float angleToPlayer = Mathf.Atan2(target.transform.position.y - boss.transform.position.y, target.transform.position.x - boss.transform.position.x);

    //    float oppositeAngle = angleToPlayer + Mathf.PI;

    //    float angle = angleToPlayer - Mathf.PI / 4;

    //    for (int i = 0; i < 3; i++)
    //    {
    //        Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, initialBullet.bulletSpeed, angle,
    //                            initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan, initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
    //                            initialBullet.hitRadius, initialBullet.bulletSprite, initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);
    //        BulletCommand change1 = bullet.gameObject.AddComponent<BulletCommand>();
    //        change1.update = update;
    //        void update()
    //        {
    //            if (change1.frame == 60)
    //            {
    //                Bullet bullet21 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
    //                            bullet.Direction - 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);
    //                BulletCommand change21 = bullet21.gameObject.AddComponent<BulletCommand>();
    //                change21.update = update21;
    //                void update21()
    //                {
    //                    if (change21.frame == 60)
    //                    {
    //                        Bullet bullet31 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet21.transform.position, initialBullet.bulletSpeed,
    //                                    bullet21.Direction - 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);
    //                        BulletCommand change31 = bullet31.gameObject.AddComponent<BulletCommand>();
    //                        change31.update = update31;
    //                        void update31()
    //                        {
    //                            if (change31.frame == 60)
    //                            {
    //                                Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction - 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change41 = bullet41.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update41;
    //                                void update41()
    //                                {
    //                                    if (change41.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed2.bulletSpeed, bullet41.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet41.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet41.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }


    //                                Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction + 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change42 = bullet42.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update42;
    //                                void update42()
    //                                {
    //                                    if (change42.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet42.transform.position,
    //                                            transformed2.bulletSpeed, bullet42.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet42.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet41.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }


    //                        Bullet bullet32 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet21.transform.position, initialBullet.bulletSpeed,
    //                                    bullet21.Direction + 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                        BulletCommand change32 = bullet32.gameObject.AddComponent<BulletCommand>();
    //                        change32.update = update32;
    //                        void update32()
    //                        {
    //                            if (change32.frame == 60)
    //                            {
    //                                Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction - 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change41 = bullet41.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update41;
    //                                void update41()
    //                                {
    //                                    if (change41.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed2.bulletSpeed, bullet41.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet41.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet41.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }


    //                                Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction + 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change42 = bullet42.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update42;
    //                                void update42()
    //                                {
    //                                    if (change42.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet42.transform.position,
    //                                            transformed2.bulletSpeed, bullet42.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet42.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet41.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }

    //                Bullet bullet22 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
    //                            bullet.Direction + 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                BulletCommand change22 = bullet22.gameObject.AddComponent<BulletCommand>();
    //                change22.update = update22;
    //                void update22()
    //                {
    //                    if (change22.frame == 60)
    //                    {
    //                        Bullet bullet31 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet21.transform.position, initialBullet.bulletSpeed,
    //                                    bullet21.Direction - 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);
    //                        BulletCommand change31 = bullet31.gameObject.AddComponent<BulletCommand>();
    //                        change31.update = update31;
    //                        void update31()
    //                        {
    //                            if (change31.frame == 60)
    //                            {
    //                                Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction - 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change41 = bullet41.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update41;
    //                                void update41()
    //                                {
    //                                    if (change41.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed2.bulletSpeed, bullet41.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet41.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet41.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }


    //                                Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction + 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change42 = bullet42.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update42;
    //                                void update42()
    //                                {
    //                                    if (change42.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet42.transform.position,
    //                                            transformed2.bulletSpeed, bullet42.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet42.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet41.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }


    //                        Bullet bullet32 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet21.transform.position, initialBullet.bulletSpeed,
    //                                    bullet21.Direction + 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                        BulletCommand change32 = bullet32.gameObject.AddComponent<BulletCommand>();
    //                        change32.update = update32;
    //                        void update32()
    //                        {
    //                            if (change32.frame == 60)
    //                            {

    //                                Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction - 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change41 = bullet41.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update41;
    //                                void update41()
    //                                {
    //                                    if (change41.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed2.bulletSpeed, bullet41.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet41.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet41.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet41.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }


    //                                Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
    //                                            bullet31.Direction + 5 * Mathf.PI / 6, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
    //                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
    //                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

    //                                BulletCommand change42 = bullet42.gameObject.AddComponent<BulletCommand>();
    //                                change41.update = update42;
    //                                void update42()
    //                                {
    //                                    if (change42.frame == 60)
    //                                    {
    //                                        Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet42.transform.position,
    //                                            transformed2.bulletSpeed, bullet42.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
    //                                            transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
    //                                            transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

    //                                        float initAngle = bullet42.Direction - Mathf.PI / 4;


    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet42.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }

    //                                        initAngle = (bullet42.Direction + Mathf.PI) - Mathf.PI / 4;

    //                                        for (int k = 0; k < 3; k++)
    //                                        {
    //                                            Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet42.transform.position,
    //                                            transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
    //                                            transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
    //                                            transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

    //                                            initAngle += Mathf.PI / 4;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        angle += Mathf.PI / 4;
    //    }

    //    yield return new WaitForSeconds(6f);

    //    FinishAttack();
    //}

    private IEnumerator BarrageHard()
    {
        Player target = GameObject.FindObjectOfType<Player>();
        float angleToPlayer = Mathf.Atan2(target.transform.position.y - boss.transform.position.y, target.transform.position.x - boss.transform.position.x);

        float oppositeAngle = angleToPlayer + Mathf.PI;

        float angle = angleToPlayer;

        for (int l = 0; l < 2; l++)
        {
            for (int i = 0; i < 3; i++)
            {
                Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, initialBullet.bulletSpeed, angle,
                                    initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan, initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                                    initialBullet.hitRadius, initialBullet.bulletSprite, initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                Action<Bullet> command = new Action<Bullet>(bullet =>
                {
                    Bullet bullet21 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    Action<Bullet> command21 = new Action<Bullet>(bullet =>
                    {
                        Bullet bullet31 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command31 = new Action<Bullet>(bullet =>
                        {
                            Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                            bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command41 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet41.AddBulletCommand(command41, 30);
                            bullet41.SetDisappear(60);


                            Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
                                        bullet31.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command42 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet42.AddBulletCommand(command42, 30);
                            bullet42.SetDisappear(60);
                        }
                        );

                        bullet31.AddBulletCommand(command31, 30);
                        bullet31.SetDisappear(60);


                        Bullet bullet32 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command32 = new Action<Bullet>(bullet =>
                        {
                            Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                            bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command41 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet41.AddBulletCommand(command41, 30);
                            bullet41.SetDisappear(60);


                            Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command42 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet42.AddBulletCommand(command42, 30);
                            bullet42.SetDisappear(60);
                        }
                        );

                        bullet32.AddBulletCommand(command32, 30);
                        bullet32.SetDisappear(60);
                    });
                    bullet21.AddBulletCommand(command21, 30);
                    bullet21.SetDisappear(60);

                    Bullet bullet22 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    Action<Bullet> command22 = new Action<Bullet>(bullet =>
                    {
                        Bullet bullet31 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command31 = new Action<Bullet>(bullet =>
                        {
                            Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                            bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command41 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet41.AddBulletCommand(command41, 30);
                            bullet41.SetDisappear(60);


                            Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command42 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet42.AddBulletCommand(command42, 30);
                            bullet42.SetDisappear(60);
                        }
                        );

                        bullet31.AddBulletCommand(command31, 30);
                        bullet31.SetDisappear(60);


                        Bullet bullet32 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command32 = new Action<Bullet>(bullet =>
                        {
                            Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                            bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command41 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet41.AddBulletCommand(command41, 30);
                            bullet41.SetDisappear(60);


                            Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                            Action<Bullet> command42 = new Action<Bullet>(bullet =>
                            {
                                Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                        transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                        transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                        transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                                float initAngle = bullet.Direction;


                                for (int k = 0; k < 8; k++)
                                {
                                    Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                    transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                    transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                    initAngle += 2 * Mathf.PI / 8;
                                }
                            });

                            bullet42.AddBulletCommand(command42, 30);
                            bullet42.SetDisappear(60);
                        }
                        );

                        bullet32.AddBulletCommand(command32, 30);
                        bullet32.SetDisappear(60);
                    });
                    bullet22.AddBulletCommand(command22, 30);
                    bullet22.SetDisappear(60);
                });
                bullet.AddBulletCommand(command, 30);
                bullet.SetDisappear(60);


                angle += 2 * Mathf.PI / 3;
            }

            angle = oppositeAngle;
        }

        yield return new WaitForSeconds(6f);

        FinishAttack();
    }

    private IEnumerator BarrageLunatic()
    {
        Player target = GameObject.FindObjectOfType<Player>();
        float angleToPlayer = Mathf.Atan2(target.transform.position.y - boss.transform.position.y, target.transform.position.x - boss.transform.position.x);

        for (int i = 0; i < 8; i++)
        {
            Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, initialBullet.bulletSpeed, angleToPlayer,
                                    initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan, initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                                    initialBullet.hitRadius, initialBullet.bulletSprite, initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);
            Action<Bullet> command = new Action<Bullet>(bullet =>
            {
                Bullet bullet21 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                Action<Bullet> command21 = new Action<Bullet>(bullet =>
                {
                    Bullet bullet31 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    Action<Bullet> command31 = new Action<Bullet>(bullet =>
                    {
                        Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command41 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet41.AddBulletCommand(command41, 30);
                        bullet41.SetDisappear(60);


                        Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet31.transform.position, initialBullet.bulletSpeed,
                                    bullet31.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command42 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet42.AddBulletCommand(command42, 30);
                        bullet42.SetDisappear(60);
                    }
                    );

                    bullet31.AddBulletCommand(command31, 30);
                    bullet31.SetDisappear(60);


                    Bullet bullet32 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    Action<Bullet> command32 = new Action<Bullet>(bullet =>
                    {
                        Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command41 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet41.AddBulletCommand(command41, 30);
                        bullet41.SetDisappear(60);


                        Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command42 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet42.AddBulletCommand(command42, 30);
                        bullet42.SetDisappear(60);
                    }
                    );

                    bullet32.AddBulletCommand(command32, 30);
                    bullet32.SetDisappear(60);
                });
                bullet21.AddBulletCommand(command21, 30);
                bullet21.SetDisappear(60);

                Bullet bullet22 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                            bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                            initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                            initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                Action<Bullet> command22 = new Action<Bullet>(bullet =>
                {
                    Bullet bullet31 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    Action<Bullet> command31 = new Action<Bullet>(bullet =>
                    {
                        Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command41 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet41.AddBulletCommand(command41, 30);
                        bullet41.SetDisappear(60);


                        Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command42 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet42.AddBulletCommand(command42, 30);
                        bullet42.SetDisappear(60);
                    }
                    );

                    bullet31.AddBulletCommand(command31, 30);
                    bullet31.SetDisappear(60);


                    Bullet bullet32 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                    Action<Bullet> command32 = new Action<Bullet>(bullet =>
                    {
                        Bullet bullet41 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                        bullet.Direction - 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                        initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                        initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command41 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet41.AddBulletCommand(command41, 30);
                        bullet41.SetDisappear(60);


                        Bullet bullet42 = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position, initialBullet.bulletSpeed,
                                    bullet.Direction + 7 * Mathf.PI / 8, initialBullet.bulletAcceleration, initialBullet.bulletLifeSpan,
                                    initialBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, initialBullet.hitRadius, initialBullet.bulletSprite,
                                    initialBullet.animatorOverrideController, 0, initialBullet.destroyOnInvisible);

                        Action<Bullet> command42 = new Action<Bullet>(bullet =>
                        {
                            Bullet whiteBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                    transformed2.bulletSpeed, bullet.Direction, transformed2.bulletAcceleration, transformed2.bulletLifeSpan,
                                    transformed2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed2.hitRadius,
                                    transformed2.bulletSprite, transformed2.animatorOverrideController, 0, transformed2.destroyOnInvisible);

                            float initAngle = bullet.Direction;


                            for (int k = 0; k < 8; k++)
                            {
                                Bullet iceBullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                                transformed1.bulletSpeed, initAngle, transformed1.bulletAcceleration, transformed1.bulletLifeSpan,
                                transformed1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, transformed1.hitRadius,
                                transformed1.bulletSprite, transformed1.animatorOverrideController, 0, transformed1.destroyOnInvisible);

                                initAngle += 2 * Mathf.PI / 8;
                            }
                        });

                        bullet42.AddBulletCommand(command42, 30);
                        bullet42.SetDisappear(60);
                    }
                    );

                    bullet32.AddBulletCommand(command32, 30);
                    bullet32.SetDisappear(60);
                });
                bullet22.AddBulletCommand(command22, 30);
                bullet22.SetDisappear(60);
            });
            bullet.AddBulletCommand(command, 30);
            bullet.SetDisappear(60);
            angleToPlayer += 2 * Mathf.PI / 8;
        }

        yield return new WaitForSeconds(6f);

        FinishAttack();
    }
}
