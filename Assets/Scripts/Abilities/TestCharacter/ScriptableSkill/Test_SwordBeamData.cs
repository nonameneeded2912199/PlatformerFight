using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Abilities
{
    [CreateAssetMenu(fileName = "Test_SwordBeamData", menuName = "Skill/TestCharacter/SwordBeam")]
    public class Test_SwordBeamData : ScriptableSkill
    {
        public BulletDetails swordBeamDetail;

        [SerializeField]
        private BulletEventChannelSO bulletEventChannel = default;

        public override Skill InitializeSkill(BaseCharacter executor, Transform attackPoint)
        {
            return new Test_SwordBeam(this, executor, attackPoint, swordBeamDetail, bulletEventChannel);
        }
    }
}