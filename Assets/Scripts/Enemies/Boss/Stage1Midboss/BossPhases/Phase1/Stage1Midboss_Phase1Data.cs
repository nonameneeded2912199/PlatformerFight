using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Phase Data/Stage 1 Midboss/Phase 1")]
public class Stage1Midboss_Phase1Data : BossPhaseDataSO
{
    public D_IdleState initialStateData;

    public D_TeleportInStateData teleportInStateData;

    public D_RangedAttackState spiralAttackStateData;

    public D_RangedAttackState chasingAttackStateData;
}
