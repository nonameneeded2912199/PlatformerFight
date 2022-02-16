using PlatformerFight.CharacterThings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase2Moving : JumpToState
{
    private Stage1Boss boss;

    private Stage1Boss_Phase2 thisPhase;

    private BulletDetails bullet;

    private BulletDetails bullet2;

    private BulletDetails bullet3;

    private bool hasShot;

    private Player target;

    private float timeToRest = 2f;

    private float timeTouchGround;

    private float flowerRadius = 0.2f;

    private Coroutine flowerTrails;

    public Stage1Boss_Phase2Moving(FiniteStateMachine stateMachine, Stage1Boss boss, string animBoolName, D_JumpToState stateData,
        BulletDetails bullet, BulletDetails bullet2, BulletDetails bullet3, Stage1Boss_Phase2 phase)
        : base(stateMachine, boss, animBoolName, stateData)
    {
        this.boss = boss;
        this.thisPhase = phase;
        this.bullet = bullet;
        this.bullet2 = bullet2;
        this.bullet3 = bullet3;
        hasShot = false;
    }

    public override void Enter()
    {
        base.Enter();
        hasShot = false;
        target = GameObject.FindObjectOfType<Player>();
        timeTouchGround = 0f;
        switch (boss.thisDifficulty)
        {
            case GameDifficulty.EASY:
                flowerTrails = boss.StartCoroutine(FlowerTrailsEasy());
                break;
            case GameDifficulty.NORMAL:
                flowerTrails = boss.StartCoroutine(FlowerTrailsNormal());
                break;
            case GameDifficulty.HARD:
                flowerTrails = boss.StartCoroutine(FlowerTrailsHard());
                break;
            case GameDifficulty.LUNATIC:
                flowerTrails = boss.StartCoroutine(FlowerTrailsLunatic());
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.StopCoroutine(flowerTrails);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float xGoal = target.transform.position.x;
        int facingDirection = boss.facingRight ? 1 : -1;

        if ((xGoal > boss.transform.position.x && facingDirection == -1) || (xGoal < boss.transform.position.x && facingDirection == 1))
        {
            boss.Flip();
        }

        if (boss.IsGrounded)
        {
            boss.CharacterAnimation.PlayAnim("Boss1_Idle");
        }
        else
        {
            boss.CharacterAnimation.PlayAnim("Boss1_Roll");
        }

        if (Time.time >= startTime + stateData.duration / 2 && !hasShot)
        {
            Shoot();
            hasShot = true;
        }

        if (jumpDone && boss.IsGrounded)
        {
            timeTouchGround += Time.deltaTime;
            if (timeTouchGround >= timeToRest)
                boss.stateMachine.ChangeState(thisPhase.NormalShoot);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private IEnumerator FlowerTrailsEasy()
    {
        while (!jumpDone && !boss.IsGrounded)
        {
            Vector3 centerPos = boss.transform.position;
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

            for (int i = 0; i < 4; i++)
            {
                Vector3 petalPos = new Vector3(centerPos.x + flowerRadius * Mathf.Cos(angle), centerPos.y + flowerRadius * Mathf.Sin(angle), 0);
                Bullet petal = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, petalPos, bullet3.bulletSpeed, angle, bullet3.bulletAcceleration,
                    bullet3.bulletLifeSpan, bullet3.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, bullet3.bulletSprite, bullet3.animatorOverrideController,
                    0f, true, true, true);

                Action<Bullet> bulletCommand = new Action<Bullet>(bullet =>
                {
                    bullet.Acceleration = 0.01f;
                });

                petal.AddBulletCommand(bulletCommand, 160);
                petal.SetDisappear(240);

                angle += 2 * Mathf.PI / 4;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator FlowerTrailsNormal()
    {
        while (!jumpDone && !boss.IsGrounded)
        {
            Vector3 centerPos = boss.transform.position;
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

            for (int i = 0; i < 8; i++)
            {
                Vector3 petalPos = new Vector3(centerPos.x + flowerRadius * Mathf.Cos(angle), centerPos.y + flowerRadius * Mathf.Sin(angle), 0);
                Bullet petal = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, petalPos, bullet3.bulletSpeed, angle, bullet3.bulletAcceleration,
                    bullet3.bulletLifeSpan, bullet3.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, bullet3.bulletSprite, bullet3.animatorOverrideController,
                    0f, true, true, true);

                Action<Bullet> bulletCommand = new Action<Bullet>(bullet =>
                {
                    bullet.Acceleration = 0.01f;
                });

                petal.AddBulletCommand(bulletCommand, 160);
                petal.SetDisappear(240);

                angle += 2 * Mathf.PI / 8;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator FlowerTrailsHard()
    {
        while (!jumpDone && !boss.IsGrounded)
        {
            Vector3 centerPos = boss.transform.position;
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

            for (int i = 0; i < 12; i++)
            {
                Vector3 petalPos = new Vector3(centerPos.x + flowerRadius * Mathf.Cos(angle), centerPos.y + flowerRadius * Mathf.Sin(angle), 0);
                Bullet petal = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, petalPos, bullet3.bulletSpeed, angle, bullet3.bulletAcceleration,
                    bullet3.bulletLifeSpan, bullet3.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, bullet3.bulletSprite, bullet3.animatorOverrideController,
                    0f, true, true, true);

                Action<Bullet> bulletCommand = new Action<Bullet>(bullet =>
                {
                    bullet.Acceleration = 0.01f;
                });

                petal.AddBulletCommand(bulletCommand, 160);
                petal.SetDisappear(240);

                angle += 2 * Mathf.PI / 12;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator FlowerTrailsLunatic()
    {
        while (!jumpDone)
        {
            Vector3 centerPos = boss.transform.position;
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

            for (int i = 0; i < 16; i++)
            {
                Vector3 petalPos = new Vector3(centerPos.x + flowerRadius * Mathf.Cos(angle), centerPos.y + flowerRadius * Mathf.Sin(angle), 0);
                Bullet petal = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, petalPos, bullet3.bulletSpeed, angle, bullet3.bulletAcceleration,
                    bullet3.bulletLifeSpan, bullet3.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, bullet3.bulletSprite, bullet3.animatorOverrideController,
                    0f, true, true, true);

                Action<Bullet> bulletCommand = new Action<Bullet>(bullet =>
                {
                    bullet.Acceleration = 0.01f;
                });

                petal.AddBulletCommand(bulletCommand, 160);
                petal.SetDisappear(240);

                angle += 2 * Mathf.PI / 16;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }


    private void Shoot()
    {
        Player target = GameObject.FindObjectOfType<Player>();
        float angleToPlayer = Mathf.Atan2(target.transform.position.y - boss.transform.position.y, target.transform.position.x - boss.transform.position.x);

        Bullet bigBall = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, boss.transform.position, bullet.bulletSpeed, angleToPlayer, bullet.bulletAcceleration,
            bullet.bulletLifeSpan, bullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 1f, bullet.bulletSprite, bullet.animatorOverrideController,
            0f, true, true, true);

        Action<Bullet> bulletCommand = default;
        switch (boss.thisDifficulty)
        {
            case GameDifficulty.EASY:
                bulletCommand = new Action<Bullet>(bullet =>
                {
                    float initAngle = bullet.Direction;

                    for (int k = 0; k < 6; k++)
                    {
                        Bullet small = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                        bullet2.bulletSpeed, initAngle, bullet2.bulletAcceleration, bullet2.bulletLifeSpan,
                        bullet2.damageMultiplier * boss.CharacterStats.CurrentAttack, 1f,
                        bullet2.bulletSprite, bullet2.animatorOverrideController, 0, false, true, true);

                        initAngle += 2 * Mathf.PI / 6;
                    }
                });
                break;
            case GameDifficulty.NORMAL:
                bulletCommand = new Action<Bullet>(bullet =>
                {
                    float initAngle = bullet.Direction;

                    for (int k = 0; k < 10; k++)
                    {
                        Bullet small = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                        bullet2.bulletSpeed, initAngle, bullet2.bulletAcceleration, bullet2.bulletLifeSpan,
                        bullet2.damageMultiplier * boss.CharacterStats.CurrentAttack, 1f,
                        bullet2.bulletSprite, bullet2.animatorOverrideController, 0, false, true, true);

                        initAngle += 2 * Mathf.PI / 10;
                    }
                });
                break;
            case GameDifficulty.HARD:
                bulletCommand = new Action<Bullet>(bullet =>
                {
                    float initAngle = bullet.Direction;

                    for (int k = 0; k < 16; k++)
                    {
                        Bullet small = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                        bullet2.bulletSpeed, initAngle, bullet2.bulletAcceleration, bullet2.bulletLifeSpan,
                        bullet2.damageMultiplier * boss.CharacterStats.CurrentAttack, 1f,
                        bullet2.bulletSprite, bullet2.animatorOverrideController, 0, false, true, true);

                        initAngle += 2 * Mathf.PI / 16;
                    }
                });
                break;
            case GameDifficulty.LUNATIC:
                bulletCommand = new Action<Bullet>(bullet =>
                {
                    float initAngle = bullet.Direction;

                    for (int k = 0; k < 22; k++)
                    {
                        Bullet small = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, bullet.transform.position,
                        bullet2.bulletSpeed * 2, initAngle, bullet2.bulletAcceleration, bullet2.bulletLifeSpan,
                        bullet2.damageMultiplier * boss.CharacterStats.CurrentAttack, 1f,
                        bullet2.bulletSprite, bullet2.animatorOverrideController, 0, false, true, true);

                        initAngle += 2 * Mathf.PI / 22;
                    }
                });
                break;
        }

        int burstFrame = UnityEngine.Random.Range(60, 101);

        bigBall.AddBulletCommand(bulletCommand, burstFrame);
        bigBall.SetDisappear(burstFrame + 5);
    }
}
