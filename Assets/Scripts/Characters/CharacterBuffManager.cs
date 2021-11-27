using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterThings
{
    public class CharacterBuffManager : MonoBehaviour
    {
        private List<BaseBuff> outdatedBuff;
        private Dictionary<ScriptableBuff, BaseBuff> buffs;

        [SerializeField]
        private Transform gridTransform;

        private void Awake()
        {
            buffs = new Dictionary<ScriptableBuff, BaseBuff>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void AddBuff(BaseBuff buff)
        {
            if (buffs.ContainsKey(buff.Buff))
            {
                buffs[buff.Buff].Active(false);
                Destroy(buff.BuffIconPrefab);
            }
            else
            {
                buffs.Add(buff.Buff, buff);
                buff.BuffIconPrefab.transform.SetParent(gridTransform);
                buff.Active(true);
            }                
        }

        // Update is called once per frame
        void Update()
        {
            outdatedBuff = new List<BaseBuff>();
            foreach (var buff in buffs)
            {
                var resBuff = buff.Value;
                resBuff.Tick(Time.deltaTime);
                if (resBuff.isFinished)
                {
                    resBuff.ApplyOnEnd();
                    outdatedBuff.Add(resBuff);
                }
            }

            foreach (var buff in outdatedBuff)
            {
                buffs.Remove(buff.Buff);
            }
        }
    }
}