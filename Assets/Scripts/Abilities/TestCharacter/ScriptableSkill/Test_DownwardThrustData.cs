using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    [CreateAssetMenu(fileName = "Test_DownwardThrustData", menuName = "Skill/TestCharacter/DownwardThrust")]
    public class Test_DownwardThrustData : ScriptableSkill
    {
        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_DownwardThrust(this, executor, attackPoint);
        }
    }
}