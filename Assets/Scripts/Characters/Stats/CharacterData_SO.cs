using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    [CreateAssetMenu(fileName = "NewCharacterDataSO", menuName = "Characters/Character Stats/Data")]
    public class CharacterData_SO : ScriptableObject
    {
        [Header("Stats Info")]
        public float baseHP;

        public float baseAP;

        public float baseATK;

        public float baseDEF;

        public float originalGravityScale;

        public float apRecoveryRate;
    }
}
