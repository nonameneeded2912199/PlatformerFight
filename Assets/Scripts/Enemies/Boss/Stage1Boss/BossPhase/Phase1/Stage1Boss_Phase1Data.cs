using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Phase Data/Stage 1 Boss/Phase 1")]
public class Stage1Boss_Phase1Data : BossPhaseDataSO
{
    public D_IdleState initialState;

    public D_MoveState pursueData;

    public D_MeleeAttackState meleeData;

    public float meleeCD = 4;
}
