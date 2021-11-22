using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings
{
    public class CharacterBuffManager : MonoBehaviour
    {
        private List<string> outdatedBuff;
        private Dictionary<string, BaseBuff> buffs;

        private void Awake()
        {
            buffs = new Dictionary<string, BaseBuff>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void AddBuff(string buffName, BaseBuff buff)
        {
            buffs.Add(buffName, buff);
        }

        // Update is called once per frame
        void Update()
        {
            outdatedBuff = new List<string>();
            foreach (var buff in buffs)
            {
                var resBuff = buff.Value.Tick(Time.deltaTime);
                if (!resBuff.Item2)
                {
                    outdatedBuff.Add(buff.Key);
                }
            }

            foreach (var buff in outdatedBuff)
            {
                buffs.Remove(buff);
            }
        }
    }
}