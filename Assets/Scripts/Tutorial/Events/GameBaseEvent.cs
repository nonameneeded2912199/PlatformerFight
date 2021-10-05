using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventSystem
{
    public abstract class GameBaseEvent
    {
        private string eventName;

        public string EventName
        {
            get => eventName;
            protected set => eventName = value;
        }

        private bool enabled;

        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public virtual void Update()
        {

        }
    }
}
