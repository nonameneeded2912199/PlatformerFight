using PlatformerFight.CharacterThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerFight.Buffs
{
    public abstract class ScriptableBuff : ScriptableObject
    {
        public bool Timed;

        [ConditionalField("Timed", true)]
        public float Duration;

        public bool IsDurationStacked;

        public bool IsEffectStacked;

        public int maxStacks = 1;

        public Sprite icon;

        public GameObject buffIconPrefab;

        public abstract BaseBuff InitializeBuff(BaseCharacter host);
    }
}