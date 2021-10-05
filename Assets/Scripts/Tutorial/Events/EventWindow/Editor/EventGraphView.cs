using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Core.EventSystem
{
    public class EventGraphView
    {
        private EventGraphWindow m_Window;

        private static EventGroupNode root;

        private int m_CurrentID;

        private EventGraph m_Graph;

        public EventGraphView(EventGraphWindow window, EventGraph graph)
        {
            m_Window = window;
            m_Graph = graph;

            LoadData(graph);
        }

        private void LoadData(EventGraph graph)
        {
            if (graph.nodes == null || graph.nodes.Count == 0)
            {
                root = new EventGroupNode() { id = m_CurrentID++, eventName = "Root" };
            }
            else
            {
                root = new EventGroupNode() { id = m_CurrentID++, eventName = "Root" };
                root.LoadNode(graph.nodes, 0);
                m_CurrentID = m_Graph.currentID;
            }
        }

        public void SaveData()
        {
            if (m_Graph.nodes != null)
                m_Graph.nodes.Clear();

            root.CopyNode(m_Graph.nodes);
            m_Graph.currentID = m_CurrentID;

            EditorUtility.SetDirty(m_Graph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Draw()
        {
            EditorGUI.DrawRect(new Rect(0, 0, m_Window.position.width, m_Window.position.height),
                new Color(0.1f, 0.1f, 0.1f, 0.8f));

            root?.Draw();
        }

        public bool ClickOnNode(Vector2 pos, out BaseEventNode node)
        {
            if (root != null)
            {
                return root.ClickOn(pos, out node);
            }
            node = null;
            return false;
        }

        public void AddNode<T>(object obj) where T: BaseEventNode, new()
        {
            EventGroupNode node = (EventGroupNode)obj;
            T newNode = new T() { id = m_CurrentID++ };
            newNode.nodeRect.x = node.nodeRect.x + 300;
            newNode.nodeRect.y = node.nodeRect.y;
            newNode.parent = node;
            node.AddNode(newNode);
        }

        public void DeleteNode(object obj)
        {
            BaseEventNode node = (BaseEventNode)obj;
            node.DeleteNode();
        }

        public static bool IsEventNameRepeated(string name, int id)
        {
            return root.IsNameRepeated(name, id);
        }

        public void Move (Vector2 delta)
        {
            root.MoveNode(delta);
            m_Window.Repaint();
        }
    }
}
