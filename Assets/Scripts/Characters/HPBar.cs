using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerFight.CharacterThings
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField]
        private CharacterStats host;

        public Image hpImage;
        public Text textHP;
        public Image hpEffectImage;

        [SerializeField]
        private float transitionLerp = 0.005f;

        // Update is called once per frame
        void Update()
        {
            hpImage.fillAmount = (float)host.CurrentHP / (float)host.MaxHP;
            if (textHP != null)
                textHP.text = host.CurrentHP + " / " + host.MaxHP;

            if (hpEffectImage.fillAmount > hpImage.fillAmount)
            {
                hpEffectImage.fillAmount -= transitionLerp;
            }
            else
            {
                hpEffectImage.fillAmount = hpImage.fillAmount;
            }
        }
    }
}