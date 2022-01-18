using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Boss : Boss
{
    #region Phase1

    public Stage1Boss_Phase1 phase1 { get; private set; }

    [SerializeField]
    private Stage1Boss_Phase1Data phase1_Data;

    #endregion

    #region Phase2

    #endregion

    public Stage1Midboss_PhaseTransition phaseTransition { get; private set; }

    public Stage1Boss_Dead deadState { get; private set; }

    [SerializeField]
    private D_DeadState deadStateData;

    public Transform flightLevel1;
    public Transform flightLevel2;

    public Transform centerPosition;

    public AIPathfinder aiPathfinder { get; protected set; }

    [SerializeField]
    private IntEventChannelSO openDoorOnDefeat;

    [SerializeField]
    private int doorNumber;

    public List<Vector3Int> mPath = new List<Vector3Int>();

    protected override void Awake()
    {
        base.Awake();
        aiPathfinder = GetComponent<AIPathfinder>();
    }

    protected override void Start()
    {
        base.Start();

        phase1 = new Stage1Boss_Phase1(this, phase1_Data, _onBossTimerUpdate, _onCompletedPhase);

        //phase2 = new Stage1Midboss_Phase2(this, phase2_Data, _onBossTimerUpdate, _onCompletedPhase);


        deadState = new Stage1Boss_Dead(stateMachine, this, "Boss1_Dead", deadStateData);

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
        Player player = FindObjectOfType<Player>();  
    }

    public override void Kill()
    {
        OnDefeat();
        stateMachine.ChangeState(deadState);
    }

    public override void OnDefeat()
    {

    }
}
