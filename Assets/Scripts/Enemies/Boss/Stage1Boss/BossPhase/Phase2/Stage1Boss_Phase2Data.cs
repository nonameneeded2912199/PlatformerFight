using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Phase Data/Stage 1 Boss/Phase 2")]
public class Stage1Boss_Phase2Data : BossPhaseDataSO
{
    public D_IdleState initialState;

    public D_JumpToState movingState;

    public BulletDetails movingStateBullet;

    public BulletDetails movingStateBullet2;

    public BulletDetails movingStateBullet3;

    public D_RangedAttackState normalAttack;
}
