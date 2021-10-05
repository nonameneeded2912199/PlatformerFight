using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings
{
    public class CharacterStats : MonoBehaviour
    {
        public CharacterData_SO characterData;

        public int MaxHP 
        {
            get
            {
                if (characterData != null)
                    return characterData.maxHP;
                return 0;
            }
            set => characterData.maxHP = value;
        }

        public int CurrentHP
        {
            get
            {
                if (characterData != null)
                    return characterData.currentHP;
                return 0;
            }
            set => characterData.currentHP = value;
        }

        public int BaseDEF
        {
            get
            {
                if (characterData != null)
                    return characterData.baseDEF;
                return 0;
            }
            set => characterData.baseDEF = value;
        }

        public int CurrentDEF
        {
            get
            {
                if (characterData != null)
                    return characterData.currentDEF;
                return 0;
            }
            set => characterData.currentDEF = value;
        }
    }
}
