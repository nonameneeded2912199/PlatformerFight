using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerFight.CharacterThings
{
    public class APBar : MonoBehaviour
    {
        [SerializeField]
        private CharacterStats host;

        public Image apImage;
        public Text textAP;
        public Image apEffectImage;

        [SerializeField]
        private float transitionLerp = 0.005f;

        private void Start()
        {
            //transform.parent.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            apImage.fillAmount = (float)host.CurrentAP / (float)host.MaxAP;
            if (textAP != null)
                textAP.text = (int)host.CurrentAP + " / " + host.MaxAP;

            if (apEffectImage.fillAmount > apImage.fillAmount)
            {
                apEffectImage.fillAmount -= transitionLerp;
            }
            else
            {
                apEffectImage.fillAmount = apImage.fillAmount;
            }
        }
    }
}