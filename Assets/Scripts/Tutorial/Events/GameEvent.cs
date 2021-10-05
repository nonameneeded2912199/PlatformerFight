using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventSystem
{
    public class GameEvent : GameBaseEvent
    {
        public delegate void ResponseHandle(params object[] args);
        protected ResponseHandle response;

        public delegate bool CheckHandle(out object[] args);
        protected CheckHandle checkHandle;

        protected object[] m_Args;

        public GameEvent(string eventName)
        {
            EventName = eventName;
        }

        public override void Update()
        {
            if (!Enabled)
                return;

            if (checkHandle != null && checkHandle(out m_Args))
            {
                if (response != null)
                    response(m_Args);
            }    
        }

        public void AddCheckHandle(CheckHandle checkHandle)
        {
            this.checkHandle += checkHandle;
        }

        public void RemoveCheckHandle(CheckHandle checkHandle)
        {
            this.checkHandle -= checkHandle;
        }

        public void AddResponse(ResponseHandle response)
        {
            this.response += response;
        }

        public void RemoveResponse(ResponseHandle response)
        {
            this.response -= response;
        }
    }

}
