using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    [CreateAssetMenu(fileName = "Test_Skill1Data", menuName = "Skill/TestCharacter/Skill1")]
    public class Test_Skill1Data : ScriptableSkill
    {
        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_Skill1(this, executor, attackPoint);
        }
    }
}