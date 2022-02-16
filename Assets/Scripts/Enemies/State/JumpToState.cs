using PlatformerFight.CharacterThings;
using UnityEngine;
using DG.Tweening;

public class JumpToState : State
{
    protected D_JumpToState stateData;

    protected bool wallCheck;

    protected bool jumpDone = false;

    private Vector3 targetJump = default;

    public JumpToState(FiniteStateMachine stateMachine, BaseEnemy entity, string animBoolName, D_JumpToState stateData)
        : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        wallCheck = entity.onWall;
    }

    public override void Enter()
    {
        base.Enter();
        jumpDone = false;

        entity.transform.DOJump(targetJump, stateData.jumpHeight, stateData.numJumps, stateData.duration, stateData.snapping).OnComplete(() =>
        {
            jumpDone = true;
        });
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetTargetJump(Vector3 target)
    {
        targetJump = target;
    }
}
