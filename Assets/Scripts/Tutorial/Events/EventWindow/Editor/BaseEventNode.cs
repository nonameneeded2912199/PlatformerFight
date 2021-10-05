using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Core.EventSystem
{
    /// <summary>
    /// Base event node
    /// </summary>
    public abstract class BaseEventNode
    {
        public Rect nodeRect = new Rect(10, 10, 250, 100);
        public string nodeTitle;
        public string eventName;
        public int id;
        public bool legal;
        protected Color textColor;

        public EventGroupNode parent;

        public virtual void Draw()
        {
            GUIStyle style = new GUIStyle(GUI.skin.window);
            style.normal.textColor = textColor;
            nodeRect = GUI.Window(id, nodeRect, NodeCallback, nodeTitle, style);
        }

        protected virtual void NodeCallback(int id)
        {
            GUI.Label(new Rect(15, 25, 80, 20), "Name");
            eventName = GUI.TextField(new Rect(70, 25, 160, 20), eventName);

            legal = false;
            if (string.IsNullOrEmpty(eventName))
            {
                GUI.Label(new Rect(15, 55, 200, 20), "Please input event name");
            }
            else if (EventGraphView.IsEventNameRepeated(eventName, id))
            {
                GUI.Label(new Rect(15, 55, 200, 20), "Event name is repeated");
            }    
            else
            {
                legal = true;
            }
            GUI.DragWindow();
        }

        public virtual bool ClickOn(Vector2 pos, out BaseEventNode node)
        {
            if (nodeRect.Contains(pos))
            {
                node = this;
                return true;
            }

            node = null;
            return false;
        }

        public virtual void DeleteNode()
        {
            if (parent != null)
            {
                parent.DeleteNode(this);
            }
        }

        public virtual bool IsNameRepeated(string name, int id)
        {
            if (eventName == name && id != this.id)
            {
                return true;
            }
            return false;
        }

        public virtual void MoveNode(Vector2 delta)
        {
            nodeRect.position += delta;
        }

        public abstract void CopyNode(List<SerializableNode> containers);

        public virtual void LoadNode(List<SerializableNode> containers, int id)
        {
            var data = containers.Find(n => n.id == id);
            if (data != null)
            {
                eventName = data.name;
                this.id = data.id;
                nodeRect = data.rect;
            }
        }
    }
}
