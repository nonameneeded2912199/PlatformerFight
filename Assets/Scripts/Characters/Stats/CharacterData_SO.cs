using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings
{
    [CreateAssetMenu(fileName = "NewCharacterDataSO", menuName = "Characters/Character Stats/Data")]
    public class CharacterData_SO : ScriptableObject
    {
        [Header("Stats Info")]
        public int maxHP;

        public int currentHP;

        public int baseDEF;

        public int currentDEF;
    }
}
