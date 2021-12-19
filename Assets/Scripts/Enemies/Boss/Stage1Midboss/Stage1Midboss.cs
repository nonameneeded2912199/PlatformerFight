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

    public Stage1Midboss_Phase2 phase2 { get; private set; }

    [SerializeField]
    private Stage1Midboss_Phase2Data phase2_Data;

    public Stage1Midboss_PhaseTransition phaseTransition { get; private set; }

    public Stage1Midboss_Dead deadState { get; private set; }

    [SerializeField]
    private D_DeadState deadStateData;

    #endregion

    #region Phase2

    #endregion

    public Transform flightLevel1;
    public Transform flightLevel2;

    public Transform centerPosition;

    //public GameObject doors;

    //public GameObject HPBarsOBJ;

    //public Image imageHP;

    //public Text textHP;

    //public Text stageCompleteText;

    protected override void Start()
    {
        base.Start();

        phase1 = new Stage1Midboss_Phase1(this, phase1_Data);

        phase2 = new Stage1Midboss_Phase2(this, phase2_Data);

        phaseTransition = new Stage1Midboss_PhaseTransition(stateMachine, this, "Midboss_Move", null);

        deadState = new Stage1Midboss_Dead(stateMachine, this, "Midboss_Dead", deadStateData);


        //HPBarsOBJ.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        currentPhase = "" + CurrentBossPhase;
        currentState = "" + stateMachine.CurrentState;
        HandleHPBar();
    }

    protected override void TakeDamage(AttackDetails attackDetails)
    {
        base.TakeDamage(attackDetails);

        if (isDead)
        {
            OnDefeat();
            //stateMachine.ChangeState(deadState);
            Destroy(gameObject);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void Activate()
    {
        //base.Activate();

        //doors.SetActive(true);

        //HPBarsOBJ.SetActive(true);

        //stateMachine.Initialize(initialState);

        phase1.StartPhase();
    }

    private void HandleHPBar()
    {
        //imageHP.fillAmount = CharacterStats.CurrentHP / CharacterStats.MaxHP;
        //textHP.text = CharacterStats.CurrentHP + " / " + CharacterStats.MaxHP;
    }
    public override void OnDefeat()
    {
        Invoke("Fanfare", 5f);
        Invoke("ToNextStage", 10f);
    }

    private void Fanfare()
    {
        //stageCompleteText.gameObject.SetActive(true);
    }

    private void ToNextStage()
    {
        //SaveManager.SceneName = "Stage2-1";
    }
}
