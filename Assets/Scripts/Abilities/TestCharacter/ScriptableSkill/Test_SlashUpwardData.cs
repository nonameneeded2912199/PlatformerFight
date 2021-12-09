using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    [CreateAssetMenu(fileName = "Test_SlashUpwardData", menuName = "Skill/TestCharacter/SlashUpward")]
    public class Test_SlashUpwardData : ScriptableSkill
    {
        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_SlashUpward(this, executor, attackPoint);
        }
    }
}