using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    [CreateAssetMenu(fileName = "Test_NormalAirATKData", menuName = "Skill/TestCharacter/NormalAirATK")]

    public class Test_NormalAirATKData : ScriptableSkill
    {
        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_NormalAirATK(this, executor, attackPoint);
        }
    }
}