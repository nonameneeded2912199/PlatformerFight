using CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings.Abilities
{
    [CreateAssetMenu(fileName = "Test_NormalATKData", menuName = "Skill/TestCharacter/NormalATK")]
    public class Test_NormalATKData : ScriptableSkill
    {
        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_NormalATK(this, executor, attackPoint);
        }
    }
}