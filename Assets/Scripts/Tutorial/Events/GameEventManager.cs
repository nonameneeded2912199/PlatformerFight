using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventSystem
{
    public static class GameEventManager
    {
        private static GameEventGroup root;

        public static GameEventGroup Root
        {
            get => root;
            private set => root = value;
        }

        public struct NodeEvent
        {
            public SerializableNode node;
            public GameBaseEvent gameEvent;
        }

        static GameEventManager()
        {
            //Init();

            ImportDataFromGraph();

            Root.Enabled = true;
            EnableAllEvents("Group1", true);
        }

        private static void Init()
        {
            Root = new GameEventGroup("Root");

            GameEventGroup group1 = new GameEventGroup("Group1");
            GameEvent event1 = new GameEvent("Event1");
            GameEvent event2 = new GameEvent("Event2");
            group1.AddEvent(event1);
            group1.AddEvent(event2);

            Root.AddEvent(group1);
        }

        private static void ImportDataFromGraph()
        {
            var graph = UnityEditor.AssetDatabase.LoadAssetAtPath<EventGraph>("Assets/Resources/Data/Event Data/Core/EventCore.asset");
            var root = graph.nodes.Find(n => n.name == "Root");

            if (root == null)
            {
                Debug.LogError("Can not find the root node");
                return;
            }

            Root = new GameEventGroup("Root");
            Queue<NodeEvent> nodeQueue = new Queue<NodeEvent>();
            nodeQueue.Enqueue(new NodeEvent() { node = root, gameEvent = Root });

            while (nodeQueue.Count > 0)
            {
                var node = nodeQueue.Dequeue();
                if (node.node != null && node.node.type == "Group" && node.node.children != null && node.node.children.Count > 0)
                {
                    for (int i = 0; i < node.node.children.Count; i++)
                    {
                        var child = graph.nodes.Find(n => n.id == node.node.children[i]);
                        if (child != null)
                        {
                            if (child.type == "Single")
                            {
                                var newEvent = new GameEvent(child.name);
                                ((GameEventGroup)node.gameEvent).AddEvent(newEvent);
                            }
                            else if (child.type == "Group")
                            {
                                var newEvent = new GameEventGroup(child.name);
                                ((GameEventGroup)node.gameEvent).AddEvent(newEvent);
                                nodeQueue.Enqueue(new NodeEvent() { node = child, gameEvent = newEvent });
                            }
                        }
                    }
                }
            }
        }

        public static void RegisterEvent(string eventName, GameEvent.CheckHandle check)
        {
            var target = Root?.GetEvent(eventName);
            if (target != null && target is GameEvent temp)
            {
                temp.AddCheckHandle(check);
            }
        }

        public static void SubscribeEvent(string eventName, GameEvent.ResponseHandle response)
        {
            var target = Root?.GetEvent(eventName);
            if (target != null && target is GameEvent temp)
            {
                temp.AddResponse(response);
            }
        }

        public static void EnableEvent(string eventName, bool enable)
        {
            var target = Root?.GetEvent(eventName);
            if (target != null)
            {
                target.Enabled = enable;
            }
        }

        /// <summary>
        /// Enable all events from a group
        /// </summary>
        /// <param name="eventName">Event's name</param>
        /// <param name="enable">Is enable or not?</param>
        public static void EnableAllEvents(string eventName, bool enable)
        {
            var target = Root?.GetEvent(eventName) as GameEventGroup;
            if (target != null)
            {
                target.EnableAllEvents(enable);
            }
        }

        public static void Update()
        {
            if (Root == null)
                return;

            Root.Update();
        }
    }
}
