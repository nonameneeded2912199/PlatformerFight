using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventSystem
{
    public class GameEventGroup : GameBaseEvent
    {
        protected List<GameBaseEvent> events;

        public GameEventGroup(string eventName)
        {
            EventName = eventName;
        }

        /// <summary>
        /// Add event to list
        /// </summary>
        /// <param name="gameEvent">Event</param>
        public void AddEvent(GameBaseEvent gameEvent)
        {
            if (events == null)
            {
                events = new List<GameBaseEvent>();
            }

            if (events.Find(e => e.EventName == gameEvent.EventName) != null)
            {
                return;
            }

            events.Add(gameEvent);
        }

        public GameBaseEvent GetEvent(string eventName)
        {
            Queue<GameBaseEvent> q = new Queue<GameBaseEvent>();

            q.Enqueue(this);
            while(q.Count > 0)
            {
                GameEventGroup temp = q.Dequeue() as GameEventGroup;
                if (temp != null && temp.events != null && temp.events.Count > 0)
                {
                    var children = temp.events;
                    foreach (GameBaseEvent eventItem in children)
                    {
                        if (eventItem.EventName == eventName)
                        {
                            return eventItem;
                        }
                        q.Enqueue(eventItem);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Enable all events from group and its children
        /// </summary>
        /// <param name="enable">Enable</param>
        public void EnableAllEvents(bool enable)
        {
            Enabled = enable;
            if (events == null) return;

            foreach (GameBaseEvent eventItem in events)
            {
                if (eventItem is GameEventGroup)
                {
                    (eventItem as GameEventGroup).EnableAllEvents(enable);
                }
                else
                {
                    eventItem.Enabled = enable;
                }
            }
        }

        /// <summary>
        /// Remove event from group and its children
        /// </summary>
        /// <param name="eventName">Event's name</param>
        public void RemoveEvent(string eventName)
        {
            if (events == null)
                return;

            foreach (GameBaseEvent eventItem in events)
            {
                if (eventItem.EventName == eventName)
                {
                    events.Remove(eventItem);
                    return;
                }
                else
                {
                    if (eventItem is GameEventGroup)
                    {
                        (eventItem as GameEventGroup).RemoveEvent(eventName);
                    }
                }
            }    
        }


        /// <summary>
        /// Update events
        /// </summary>
        public override void Update()
        {
            if (!Enabled || events == null)
                return;

            foreach (GameBaseEvent eventItem in events)
            {
                if (eventItem != null)
                    eventItem.Update();
            }
        }
    }
}
