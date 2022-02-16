using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : Boss
{
    #region Phase1

    public Stage1Boss_Phase1 phase1 { get; private set; }

    [Header("Phase 1")]
    [SerializeField]
    private Stage1Boss_Phase1Data phase1_Data;

    #endregion

    #region Phase2

    
    public Stage1Boss_Phase2 phase2 { get; private set; }

    [Header("Phase 2")]
    [SerializeField]
    private Stage1Boss_Phase2Data phase2_Data;

    #endregion

    #region Phase3

    public Stage1Boss_Phase3 phase3 { get; private set; }

    [SerializeField]
    private Stage1Boss_Phase3Data phase3_Data;

    #endregion

    #region TransitionPhases

    public Stage1Boss_PhaseTransition1 phaseTransition1 { get; private set; }
    public Stage1Boss_PhaseTransition2 phaseTransition2 { get; private set; }

    [SerializeField]
    private D_MoveState phaseTransition1Data;

    #endregion

    #region OtherPhases

    public Stage1Boss_Dead deadState { get; private set; }

    [SerializeField]
    private D_DeadState deadStateData;

    #endregion

    public Transform meleeATKPosition;

    public Transform leftMost;

    public Transform leftCenter;

    public Transform rightCenter;

    public Transform middleCenter;

    public Transform rightMost;

    public Transform highCenter;

    [SerializeField]
    private VoidEventChannelSO afterDefeatEvent = default;

    public AIPathfinder aiPathfinder { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        aiPathfinder = GetComponent<AIPathfinder>();
    }

    protected override void Start()
    {
        base.Start();

        phase1 = new Stage1Boss_Phase1(this, phase1_Data, _onBossTimerUpdate, _onCompletedPhase);

        phase2 = new Stage1Boss_Phase2(this, phase2_Data, _onBossTimerUpdate, _onCompletedPhase);

        phase3 = new Stage1Boss_Phase3(this, phase3_Data, _onBossTimerUpdate, _onCompletedPhase);

        phaseTransition1 = new Stage1Boss_PhaseTransition1(stateMachine, this, "Boss1_Move", phaseTransition1Data, leftCenter, rightCenter);
        phaseTransition2 = new Stage1Boss_PhaseTransition2(stateMachine, this, "Boss1_Move", phaseTransition1Data, middleCenter);

        deadState = new Stage1Boss_Dead(stateMachine, this, "Boss1_Dead", deadStateData);

        stateMachine.Initialize(new State(stateMachine, this, ""));

        gameObject.SetActive(false);
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
        gameObject.SetActive(true);

        foreach (GameObject door in doorsToLock)
            door.SetActive(true);

        phase1.StartPhase(); 
    }

    public override void Kill()
    {
        OnDefeat();
        stateMachine.ChangeState(deadState);
    }

    public override void OnDefeat()
    {
        afterDefeatEvent.RaiseEvent();
    }
}
