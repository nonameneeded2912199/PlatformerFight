using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings.Abilities
{
    [CreateAssetMenu(fileName = "Test_SwordBeamData", menuName = "Skill/TestCharacter/SwordBeam")]
    public class Test_SwordBeamData : ScriptableSkill
    {
        public float bulletMultiplier;

        public GameObject swordbeamOBJ;    

        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_SwordBeam(this, executor, attackPoint, bulletMultiplier, swordbeamOBJ);
        }
    }
}