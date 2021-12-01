using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterThings
{
    [CreateAssetMenu(menuName = "Buffs/AttackPercentBuff")]

    public class ScriptableAttackPercentBuff : ScriptableBuff
    {
        public bool flatOrPercentage; // true: flat, false: percentage

        public float attackIncreasePercentage;

        public override BaseBuff InitializeBuff(BaseCharacter host)
        {
            return new AttackPercentBuff(this, host);
        }
    }
}