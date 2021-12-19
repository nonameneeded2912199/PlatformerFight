using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public interface BossPhase
    {
        public abstract void StartPhase();

        public abstract void Update(float deltaTime);

        public abstract void FixedUpdate(float deltaTime);

        public abstract void LateUpdate(float deltaTime);

        public abstract void TakeDamage(AttackDetails attackDetails);

        public abstract void EndPhase();
    }
}