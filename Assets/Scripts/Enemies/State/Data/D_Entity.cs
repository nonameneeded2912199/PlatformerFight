using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    [Header("Stats")]
    public float maxHP = 30f;
    public float damageHopSpeed = 3f;
    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;
    public float closeRangeActionDistance = 1f;

    [Header("Physics Parameter")]
    public float wallRadius = 0.2f;
    public float ledgeRadius = 0.4f;
    public float groundRadius = 0.2f;

    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("Respawnability")]
    public bool allowRespawn = true;
    public float respawnTime = 2f;
}
