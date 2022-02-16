using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss_Phase2NormalShoot : RangedAttackState
{
    Stage1Boss boss;

    Player target;

    Stage1Boss_Phase2 phase;

    private float timeStartAttack = 0f;

    Coroutine attackingCoroutine;

    public Stage1Boss_Phase2NormalShoot(FiniteStateMachine stateMachine, Stage1Boss entity, string animBoolName, Transform attackPosition, 
        Stage1Boss_Phase2 phase, D_RangedAttackState stateData)
        : base(stateMachine, entity, animBoolName, attackPosition, stateData)
    {
        this.boss = entity;
        this.phase = phase;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        target = GameObject.FindObjectOfType<Player>();

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

        Debug.Log("Done");

        Vector3 target = new Vector3(Random.Range(boss.leftMost.position.x, boss.rightMost.position.x),
                boss.middleCenter.position.y, 0);

        phase.MovingState.SetTargetJump(target);

        boss.stateMachine.ChangeState(phase.MovingState);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        timeStartAttack = Time.time;
        switch (boss.thisDifficulty)
        {
            case GameDifficulty.EASY:
                attackingCoroutine = boss.StartCoroutine(SpiralAttackEasy());
                break;
            case GameDifficulty.NORMAL:
                attackingCoroutine = boss.StartCoroutine(SpiralAttackNormal());
                break;
            case GameDifficulty.HARD:
                attackingCoroutine = boss.StartCoroutine(SpiralAttackHard());
                break;
            case GameDifficulty.LUNATIC:
                attackingCoroutine = boss.StartCoroutine(SpiralAttackLunatic());
                break;
        }

    }

    private IEnumerator SpiralAttackEasy()
    {
        float angle = Mathf.PI / 2;

        bool shootDirection = Random.Range(0, 2) == 0;

        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];

        while (Time.time <= timeStartAttack + stateData.attackTime)
        {          
            Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack / 2, 0.5f, thisAttackProjectile.bulletSprite, 
                            thisAttackProjectile.animatorOverrideController, 0, false, true, true);

            System.Action<Bullet> bulletCommand = new System.Action<Bullet>(bullet =>
            {
                bullet.Acceleration = 0.4f;
            });

            bullet.AddBulletCommand(bulletCommand, 90);

            if (shootDirection)
                angle -= Mathf.PI / 4;
            else
                angle += Mathf.PI / 4;

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(5f);
        FinishAttack();
    }

    private IEnumerator SpiralAttackNormal()
    {
        float angle = Mathf.PI / 2;

        bool shootDirection = Random.Range(0, 2) == 0;

        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];

        while (Time.time <= timeStartAttack + stateData.attackTime)
        {
            Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, thisAttackProjectile.bulletSprite,
                            thisAttackProjectile.animatorOverrideController, 0, false, true, true);

            System.Action<Bullet> bulletCommand = new System.Action<Bullet>(bullet =>
            {
                bullet.Acceleration = 0.5f;
            });

            bullet.AddBulletCommand(bulletCommand, 90);

            if (shootDirection)
                angle -= Mathf.PI / 6;
            else
                angle += Mathf.PI / 6;

            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(5f);
        FinishAttack();
    }

    private IEnumerator SpiralAttackHard()
    {
        float angle = Mathf.PI / 2;

        bool shootDirection = Random.Range(0, 2) == 0;

        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];

        while (Time.time <= timeStartAttack + stateData.attackTime)
        {
            Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, thisAttackProjectile.bulletSprite,
                            thisAttackProjectile.animatorOverrideController, 0, false, true, true);

            System.Action<Bullet> bulletCommand = new System.Action<Bullet>(bullet =>
            {
                bullet.Acceleration = 0.6f;
            });

            bullet.AddBulletCommand(bulletCommand, 90);

            if (shootDirection)
                angle -= Mathf.PI / 8;
            else
                angle += Mathf.PI / 8;

            yield return new WaitForSeconds(0.06f);
        }
        yield return new WaitForSeconds(5f);
        FinishAttack();
    }

    private IEnumerator SpiralAttackLunatic()
    {
        float angle = Mathf.PI / 2;

        bool shootDirection = Random.Range(0, 2) == 0;

        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];

        while (Time.time <= timeStartAttack + stateData.attackTime)
        {
            Bullet bullet = entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed * 1.5f, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack * 2f, 0.5f, thisAttackProjectile.bulletSprite,
                            thisAttackProjectile.animatorOverrideController, 0, false, true, true);

            System.Action<Bullet> bulletCommand = new System.Action<Bullet>(bullet =>
            {
                bullet.Acceleration = 0.8f;
            });

            bullet.AddBulletCommand(bulletCommand, 90);
            if (shootDirection)
                angle -= Mathf.PI / 12;
            else
                angle += Mathf.PI / 12;

            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(5f);
        FinishAttack();
    }

}
