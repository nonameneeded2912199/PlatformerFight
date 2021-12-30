using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    [CreateAssetMenu(fileName = "Test_WallSlideATKData", menuName = "Skill/TestCharacter/WallSlideATK")]
    public class Test_WallSlideATKData : ScriptableSkill
    {
        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_WallSlideATK(this, executor, attackPoint);
        }
    }
}