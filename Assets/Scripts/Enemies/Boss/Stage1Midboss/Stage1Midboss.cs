using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Midboss : Boss
{
    #region Phase1

    public Stage1Midboss_Phase1 phase1 { get; private set; }

    [SerializeField]
    private Stage1Midboss_Phase1Data phase1_Data;

    #endregion

    #region Phase2

    public Stage1Midboss_Phase2 phase2 { get; private set; }

    [SerializeField]
    private Stage1Midboss_Phase2Data phase2_Data;

    #endregion

    public Stage1Midboss_PhaseTransition phaseTransition { get; private set; }

    [SerializeField]
    private VoidEventChannelSO OnStageCompleted;

    public Stage1Midboss_Dead deadState { get; private set; }

    [SerializeField]
    private D_DeadState deadStateData;

    public Transform flightLevel1;
    public Transform flightLevel2;

    public Transform centerPosition;

    //public Text stageCompleteText;

    protected override void Start()
    {
        base.Start();

        phase1 = new Stage1Midboss_Phase1(this, phase1_Data, _onBossTimerUpdate, _onCompletedPhase);

        phase2 = new Stage1Midboss_Phase2(this, phase2_Data, _onBossTimerUpdate, _onCompletedPhase);

        phaseTransition = new Stage1Midboss_PhaseTransition(stateMachine, this, "Midboss_Move", null);

        deadState = new Stage1Midboss_Dead(stateMachine, this, "Midboss_Dead", deadStateData);


        //HPBarsOBJ.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        currentPhase = "" + CurrentBossPhase;
        currentState = "" + stateMachine.CurrentState;
    }

    protected override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void Activate()
    {
        phase1.StartPhase();
    }

    public override void Kill()
    {
        OnDefeat();
        stateMachine.ChangeState(deadState);
    }

    public override void OnDefeat()
    {
        OnStageCompleted.RaiseEvent();
    }
}
