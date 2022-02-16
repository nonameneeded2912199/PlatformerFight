using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Midboss_RandomAttack : RangedAttackState
{
    Stage1Midboss boss;

    Player target;

    Stage1Midboss_Phase1 phase;

    private float timeStartAttack = 0f;

    Coroutine attackingCoroutine;

    public Stage1Midboss_RandomAttack(FiniteStateMachine stateMachine, Stage1Midboss entity, string animBoolName, Transform attackPosition, Stage1Midboss_Phase1 phase, D_RangedAttackState stateData)
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

        timeStartAttack = 0f;
        target = GameObject.FindObjectOfType<Player>();

        TriggerAttack();
    }

    public override void Exit()
    {
        base.Exit();
        boss.StopCoroutine(attackingCoroutine);
        phase.randomShootTime++;
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
                attackingCoroutine = boss.StartCoroutine(RandomAttackEasy());
                break;
            case GameDifficulty.NORMAL:
                attackingCoroutine = boss.StartCoroutine(RandomAttackNormal());
                break;
            case GameDifficulty.HARD:
                attackingCoroutine = boss.StartCoroutine(RandomAttackHard());
                break;
            case GameDifficulty.LUNATIC:
                attackingCoroutine = boss.StartCoroutine(RandomAttackLunatic());
                break;
        }

    }

    private IEnumerator RandomAttackEasy()
    {
        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];


        while (Time.time <= timeStartAttack + stateData.attackTime)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);

            entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack / 2, 0.5f,
                            thisAttackProjectile.bulletSprite, thisAttackProjectile.animatorOverrideController, 0, thisAttackProjectile.destroyOnInvisible);

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        FinishAttack();
    }

    private IEnumerator RandomAttackNormal()
    {
        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];
        BulletDetails thisAttackProjectile2 = stateData.bulletDetails[1];

        while (Time.time <= timeStartAttack + stateData.attackTime)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);

            entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                            thisAttackProjectile.bulletSprite, thisAttackProjectile.animatorOverrideController, 0, thisAttackProjectile.destroyOnInvisible);

            yield return new WaitForSeconds(0.08f);
        }

        for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 8)
        {
            entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile2.bulletSpeed, i,
                            thisAttackProjectile2.bulletAcceleration, thisAttackProjectile2.bulletLifeSpan,
                            thisAttackProjectile2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f, 
                            thisAttackProjectile2.bulletSprite, thisAttackProjectile2.animatorOverrideController, 0, thisAttackProjectile2.destroyOnInvisible);
        }

        yield return new WaitForSeconds(2f);

        FinishAttack();
    }

    private IEnumerator RandomAttackHard()
    {
        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];
        BulletDetails thisAttackProjectile2 = stateData.bulletDetails[1];

        while (Time.time <= timeStartAttack + stateData.attackTime)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);

            entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                            thisAttackProjectile.bulletSprite, thisAttackProjectile.animatorOverrideController, 0, thisAttackProjectile.destroyOnInvisible);

            yield return new WaitForSeconds(0.06f);
        }

        for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 12)
        {
            entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile2.bulletSpeed, i,
                            thisAttackProjectile2.bulletAcceleration, thisAttackProjectile2.bulletLifeSpan,
                            thisAttackProjectile2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                            thisAttackProjectile2.bulletSprite, thisAttackProjectile2.animatorOverrideController, 0, thisAttackProjectile2.destroyOnInvisible);
        }

        yield return new WaitForSeconds(2f);
        FinishAttack();
    }

    private IEnumerator RandomAttackLunatic()
    {
        BulletDetails thisAttackProjectile = stateData.bulletDetails[0];
        BulletDetails thisAttackProjectile2 = stateData.bulletDetails[1];


        while (Time.time <= timeStartAttack + stateData.attackTime)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);

            entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile.bulletSpeed * 1.5f, angle,
                            thisAttackProjectile.bulletAcceleration, thisAttackProjectile.bulletLifeSpan,
                            thisAttackProjectile.damageMultiplier * boss.CharacterStats.CurrentAttack * 2f, 0.5f,
                            thisAttackProjectile.bulletSprite, thisAttackProjectile.animatorOverrideController, 0, thisAttackProjectile.destroyOnInvisible);

            yield return new WaitForSeconds(0.03f);
        }

        for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / 16)
        {
            entity.BulletEventChannel.RaiseBulletEvent(boss.tag, attackPosition.position, thisAttackProjectile2.bulletSpeed, i,
                            thisAttackProjectile2.bulletAcceleration, thisAttackProjectile2.bulletLifeSpan,
                            thisAttackProjectile2.damageMultiplier * boss.CharacterStats.CurrentAttack, 0.5f,
                            thisAttackProjectile2.bulletSprite, thisAttackProjectile2.animatorOverrideController, 0, thisAttackProjectile2.destroyOnInvisible);
        }

        yield return new WaitForSeconds(1f);
        FinishAttack();
    }
}
