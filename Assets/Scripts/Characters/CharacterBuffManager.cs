using PlatformerFight.Buffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public class CharacterBuffManager : MonoBehaviour
    {
        [SerializeField]
        private BuffEventChannelSO _onBuffDisplay;

        private List<BaseBuff> outdatedBuff;
        private Dictionary<ScriptableBuff, BaseBuff> buffs;

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
                if (_onBuffDisplay != null)
                    //buff.BuffIconPrefab.transform.SetParent(gridTransform);
                    _onBuffDisplay.RaiseEvent(buff);
                else
                    Destroy(buff.BuffIconPrefab);
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