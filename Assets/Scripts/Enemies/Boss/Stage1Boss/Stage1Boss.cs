using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Boss : Boss
{
    public Stage1Boss_InitialState initialState { get; private set; }

    public Stage1Boss_MeleeAttack meleeAttack { get; private set; }

    public Stage1Boss_RangedAttack rangedAttack { get; private set; }

    public Stage1Boss_SpecialAttack1 specialAttack1 { get; private set; }

    public Stage1Boss_SpecialAttack2 specialAttack2 { get; private set; }

    public Stage1Boss_SpecialAttack3 specialAttack3 { get; private set; }

    public Stage1Boss_SpecialAttackFatal specialAttackFatal { get; private set; }

    public Stage1Boss_MoveToCenter moveToCenter { get; private set; }

    public Stage1Boss_Dead deadState { get; private set; }

    public State incomingSpecialAttack { get; set; }

    [SerializeField]
    private D_IdleState initialStateData;

    [SerializeField]
    private D_MeleeAttackState meleeStateData;

    [SerializeField]
    private D_RangedAttackState rangedStateData;

    [SerializeField]
    private D_RangedAttackState specialAttack1Data;

    [SerializeField]
    private D_RangedAttackState specialAttack2Data;

    [SerializeField]
    private D_MoveState specialAttack3DataMove;

    [SerializeField]
    private D_RangedAttackState specialAttack3DataRanged;

    [SerializeField]
    private D_RangedAttackState specialAttackFatalData;

    [SerializeField]
    private D_MoveState moveToCenterData;

    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    [SerializeField]
    private Transform rangedAttackPosition;

    public Transform centerPosition;
    public Transform specialAttack1LeftPoint;
    public Transform specialAttack1RightPoint;

    public Transform specialAttack2LeftPoint;
    public Transform specialAttack2RightPoint;

    public GameObject doors;

    public GameObject HPBarsOBJ;

    public Image imageHP;

    public Text textHP;

    public Text stageCompleteText;

    protected override void Start()
    {
        base.Start();

        HPBarsOBJ.SetActive(false);

        initialState = new Stage1Boss_InitialState(stateMachine, this, "Idle", initialStateData);

        specialAttack1 = new Stage1Boss_SpecialAttack1(stateMachine, this, "RangedAttack", this.transform, specialAttack1Data);
        specialAttack2 = new Stage1Boss_SpecialAttack2(stateMachine, this, "RangedAttack", this.transform, specialAttack2Data);
        specialAttack3 = new Stage1Boss_SpecialAttack3(stateMachine, this, "Move", specialAttack3DataMove, specialAttack3DataRanged);

        meleeAttack = new Stage1Boss_MeleeAttack(stateMachine, this, "MeleeAttack", meleeAttackPosition, meleeStateData);
        rangedAttack = new Stage1Boss_RangedAttack(stateMachine, this, "RangedAttack", rangedAttackPosition, rangedStateData);

        moveToCenter = new Stage1Boss_MoveToCenter(stateMachine, this, "Move", moveToCenterData, centerPosition);

        specialAttackFatal = new Stage1Boss_SpecialAttackFatal(stateMachine, this, "Fatal", centerPosition.transform, specialAttackFatalData);

        deadState = new Stage1Boss_Dead(stateMachine, this, "Dead", deadStateData);
    }

    protected override void Update()
    {
        base.Update();
        HandleHPBar();
    }

    protected override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if (isDead)
        {
            OnDefeat();
            stateMachine.ChangeState(deadState);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeStateData.attackRadius);
    }

    public override void Activate()
    {
        base.Activate();

        doors.SetActive(true);

        HPBarsOBJ.SetActive(true);

        stateMachine.Initialize(initialState);
    }

    private void HandleHPBar()
    {
        imageHP.fillAmount = CharacterStats.CurrentHP / CharacterStats.MaxHP;
        textHP.text = CharacterStats.CurrentHP + " / " + CharacterStats.MaxHP;
    }
    public override void OnDefeat()
    {
        Invoke("Fanfare", 5f);
        Invoke("ToNextStage", 10f);
    }

    private void Fanfare()
    {
        stageCompleteText.gameObject.SetActive(true);
    }

    private void ToNextStage()
    {
        SaveManager.SceneName = "Stage2-1";
    }
}
