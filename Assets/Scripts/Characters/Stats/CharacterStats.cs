using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public class CharacterStats : MonoBehaviour
    {
        public CharacterData_SO characterData;

        public float BaseHP
        {
            get
            {
                if (characterData != null)
                    return characterData.baseHP;
                return 0;
            }
            //set => characterData.baseHP = value;
        }

        public float BaseAP
        {
            get
            {
                if (characterData != null)
                    return characterData.baseAP;
                return 0;
            }
            //set => characterData.baseAP = value;
        }

        public float BaseATK
        {
            get
            {
                if (characterData != null)
                    return characterData.baseATK;
                return 0;
            }
            //set => characterData.baseATK = value;
        }


        public float BaseDEF
        {
            get
            {
                if (characterData != null)
                    return characterData.baseDEF;
                return 0;
            }
            //set => characterData.baseDEF = value;
        }

        public float BaseAPRecoveryRate
        {
            get
            {
                if (characterData != null)
                    return characterData.apRecoveryRate;
                return 0;
            }
            //set => characterData.apRecoveryRate = value;
        }

        private float maxHP;
        public float MaxHP { get => maxHP; set => maxHP = value; }

        private float currentHP;
        public float CurrentHP { get => currentHP; set => currentHP = Mathf.Clamp(value, 0, MaxHP); }

        private float maxAP;
        public float MaxAP { get => maxAP; set => maxAP = value; }

        private float currentAP;
        public float CurrentAP { get => currentAP; set => currentAP = Mathf.Clamp(value, 0, maxAP); }


        private float bonusAttack;

        public float BonusAttack { get => bonusAttack; set => bonusAttack = value; }

        public float CurrentAttack { get => BaseATK + bonusAttack; }

        private float bonusDefense;

        public float BonusDefense { get => bonusDefense; set => bonusDefense = value; }

        public float CurrentDefense { get => BaseDEF + bonusDefense; }


        private float defense;
        public float Defense { get => defense; }

        private float apRecoveryRate;

        public float APRecoveryRate { get => apRecoveryRate; set => apRecoveryRate = value; }

        private bool rechargeEnergy = true;

        void Awake()
        {
            if (characterData != null)
            {
                maxHP = BaseHP;
                maxAP = BaseAP;
                currentHP = maxHP;
                currentAP = maxAP;

                bonusAttack = 0;
                bonusDefense = 0;

                apRecoveryRate = BaseAPRecoveryRate;
            }


        }

        private void Update()
        {
            if (rechargeEnergy)
            {
                currentAP = Mathf.MoveTowards(currentAP, maxAP, Time.deltaTime * apRecoveryRate);
            }
            else
            {
                currentAP += 0f;
            }

            if (currentAP > maxAP)
            {
                currentAP = maxAP;
            }
            else if (currentAP < 0f)
            {
                currentAP = 0f;
            }
        }

        public void ConsumeAP(float ap)
        {
            currentAP -= ap;
            if (currentAP < 0)
                currentAP = 0;
        }

        public void SetAPRecovery(bool recover)
        {
            rechargeEnergy = recover;
        }
    }
}
