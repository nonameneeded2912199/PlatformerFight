using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public abstract class Boss : BaseEnemy
    {
        [Header("Event channel")]
        [SerializeField]
        private BossPhaseEventChannelSO _onBossStatusStart;
        public BossPhaseEventChannelSO _OnBossStatusStart => _onBossStatusStart;

        [SerializeField]
        private VoidEventChannelSO _onBossStatusEnd;
        public VoidEventChannelSO _OnBossStatusEnd => _onBossStatusEnd;

        public BossPhase CurrentBossPhase { get; set; }

        public BossPhase NextBossPhase { get; set; }

        [SerializeField]
        protected string currentState;

        [SerializeField]
        protected string currentPhase;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            CurrentBossPhase?.Update(Time.deltaTime);
        }

        protected override void FixedUpdate()
        {
            HandleCollision();
            CheckTouchDamage();
            CurrentBossPhase?.FixedUpdate(Time.deltaTime);
        }

        public override void LateUpdate()
        {
            CurrentBossPhase?.LateUpdate(Time.deltaTime);
        }

        protected override void TakeDamage(AttackDetails attackDetails)
        {
            if (IsInvincible)
                return;

            CurrentBossPhase?.TakeDamage(attackDetails);
        }

        public abstract void Activate();

        public virtual void OnDefeat()
        {

        }
    }
}