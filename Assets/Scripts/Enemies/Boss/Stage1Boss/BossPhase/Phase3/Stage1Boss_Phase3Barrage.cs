using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase3Barrage : RangedAttackState
{
    private Stage1Boss boss;

    private Stage1Boss_Phase3 phase;

    private float mainRadius = 2f;

    private Coroutine attackingCoroutine;

    private Coroutine attackingSubState;

    private Coroutine attackingSubState2;

    private BulletDetails mainBullet;

    private BulletDetails subBullet1;

    private BulletDetails subBullet2;

    private bool sub1Activated = false;

    private bool sub2Activated = false;

    public Stage1Boss_Phase3Barrage(FiniteStateMachine stateMachine, Stage1Boss entity, string animBoolName, Transform attackPosition,
        D_RangedAttackState stateData, Stage1Boss_Phase3 phase)
        : base(stateMachine, entity, animBoolName, attackPosition, stateData)
    {
        this.boss = entity;
        this.phase = phase;

        mainBullet = stateData.bulletDetails[0];
        subBullet1 = stateData.bulletDetails[1];
        subBullet2 = stateData.bulletDetails[2];
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
        boss.StopCoroutine(attackingSubState);
        boss.StopCoroutine(attackingSubState2);
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + 10f && !sub1Activated)
        {
            sub1Activated = true;
            switch (boss.thisDifficulty)
            {
                case GameDifficulty.EASY:
                case GameDifficulty.NORMAL:
                case GameDifficulty.HARD:
                    attackingSubState = boss.StartCoroutine(SubAttack1Hard());
                    break;
                case GameDifficulty.LUNATIC:
                    attackingSubState = boss.StartCoroutine(SubAttack1Lunatic());
                    break;
            }
        }

        if (Time.time >= startTime + 30f && !sub2Activated)
        {
            sub2Activated = true;
            switch (boss.thisDifficulty)
            {
                case GameDifficulty.EASY:
                case GameDifficulty.NORMAL:
                case GameDifficulty.HARD:
                    attackingSubState2 = boss.StartCoroutine(SubAttack2Hard());
                    break;
                case GameDifficulty.LUNATIC:
                    attackingSubState2 = boss.StartCoroutine(SubAttack2Lunatic());
                    break;
            }
        }
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
            case GameDifficulty.NORMAL:
            case GameDifficulty.HARD:
                attackingCoroutine = boss.StartCoroutine(MainAttackHard());
                break;
            case GameDifficulty.LUNATIC:
                attackingCoroutine = boss.StartCoroutine(MainAttackLunatic());
                break;
        }    
    }

    private IEnumerator MainAttackHard()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 centerPos = boss.transform.position;

        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 disPos = new Vector3(centerPos.x + mainRadius * Mathf.Cos(angle * i), centerPos.y + mainRadius * Mathf.Sin(angle * i), 0);
                Bullet disBullet = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, disPos, mainBullet.bulletSpeed, angle * i, mainBullet.bulletAcceleration,
                    mainBullet.bulletLifeSpan, mainBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, mainBullet.bulletSprite, mainBullet.animatorOverrideController,
                    0f, true, true, true);
            }

            angle += 2 * Mathf.PI / 24;

            yield return new WaitForSeconds(0.3f);
        }    
    }

    private IEnumerator MainAttackLunatic()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 centerPos = boss.transform.position;

        while (true)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector3 disPos = new Vector3(centerPos.x + mainRadius * Mathf.Cos(angle * i), centerPos.y + mainRadius * Mathf.Sin(angle * i), 0);
                Bullet disBullet = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, disPos, mainBullet.bulletSpeed, angle * i, mainBullet.bulletAcceleration,
                    mainBullet.bulletLifeSpan, mainBullet.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, mainBullet.bulletSprite, mainBullet.animatorOverrideController,
                    0f, true, true, true);
            }

            angle += 2 * Mathf.PI / 36;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SubAttack1Hard()
    {
        while (true)
        {
            for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 24)
            {
                Bullet subBullet = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, boss.transform.position, subBullet1.bulletSpeed, 
                    i, subBullet1.bulletAcceleration, subBullet1.bulletLifeSpan, subBullet1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                    subBullet1.bulletSprite, subBullet1.animatorOverrideController, 0f, true, true, true);

                System.Action<Bullet> bulletCommand = new System.Action<Bullet>((bullet) =>
                {
                    bullet.Direction += Mathf.PI;
                    bullet.Acceleration = 0.5f;
                });

                subBullet.AddBulletCommand(bulletCommand, 60);
            }
            yield return new WaitForSeconds(4f);
        }        
    }

    private IEnumerator SubAttack1Lunatic()
    {
        while (true)
        {
            for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 36)
            {
                Bullet subBullet = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, boss.transform.position, subBullet1.bulletSpeed,
                    i, subBullet1.bulletAcceleration, subBullet1.bulletLifeSpan, subBullet1.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                    subBullet1.bulletSprite, subBullet1.animatorOverrideController, 0f, true, true, true);

                System.Action<Bullet> bulletCommand = new System.Action<Bullet>((bullet) =>
                {
                    bullet.Direction += Mathf.PI;
                    bullet.Acceleration = 0.5f;
                });

                subBullet.AddBulletCommand(bulletCommand, 60);
            }
            yield return new WaitForSeconds(4f);
        }
    }

    private IEnumerator SubAttack2Hard()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                float angle = Random.Range(0, 2 * Mathf.PI);

                Bullet subBullet = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, boss.transform.position, subBullet2.bulletSpeed,
                    angle, subBullet2.bulletAcceleration, subBullet2.bulletLifeSpan, subBullet2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                    subBullet2.bulletSprite, subBullet2.animatorOverrideController, 0f, true, true, true);

                System.Action<Bullet> bulletCommand = new System.Action<Bullet>((bullet) =>
                {
                    Player player = GameObject.FindObjectOfType<Player>();
                    float angleToPlayer = Mathf.Atan2(player.transform.position.y - bullet.transform.position.y,
                        player.transform.position.x - bullet.transform.position.x);

                    bullet.Direction = angleToPlayer;
                    bullet.Acceleration = 1;
                });

                subBullet.AddBulletCommand(bulletCommand, 75);
            }

            yield return new WaitForSeconds(7f);
        }
    }

    private IEnumerator SubAttack2Lunatic()
    {
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                float angle = Random.Range(0, 2 * Mathf.PI);

                Bullet subBullet = boss.BulletEventChannel.RaiseBulletEvent(boss.tag, boss.transform.position, subBullet2.bulletSpeed,
                    angle, subBullet2.bulletAcceleration, subBullet2.bulletLifeSpan, subBullet2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                    subBullet2.bulletSprite, subBullet2.animatorOverrideController, 0f, true, true, true);

                System.Action<Bullet> bulletCommand = new System.Action<Bullet>((bullet) =>
                {
                    Player player = GameObject.FindObjectOfType<Player>();
                    float angleToPlayer = Mathf.Atan2(player.transform.position.y - bullet.transform.position.y,
                        player.transform.position.x - bullet.transform.position.x);

                    bullet.Direction = angleToPlayer;
                    bullet.Acceleration = 1;
                });

                subBullet.AddBulletCommand(bulletCommand, 75);
            }

            yield return new WaitForSeconds(7f);
        }
    }
}
