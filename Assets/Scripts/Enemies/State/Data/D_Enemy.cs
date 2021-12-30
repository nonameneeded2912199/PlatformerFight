using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Enemy : ScriptableObject
{
    [Header("Stats")]
    public float knockbackResistance;
    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;
    public float closeRangeActionDistance = 1f;

    [Header("Physics Parameter")]
    public float ledgeRadius = 0.4f;

    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;

    [Header("Layers")]
    public LayerMask playerLayer;

    [Header("Score")]
    public long scoreYield;
}
