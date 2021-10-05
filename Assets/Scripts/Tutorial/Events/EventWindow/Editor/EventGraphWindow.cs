using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Core.EventSystem
{
    public class EventGraphWindow : EditorWindow
    {
        private static EventGraphView m_View;

        public static void OpenWindow(EventGraph graph)
        {
            EventGraphWindow window = GetWindow<EventGraphWindow>("Event Graph");
            window.minSize = new Vector2(800, 600);
            window.Show();

            m_View = new EventGraphView(window, graph);
        }

        private void OnGUI()
        {
            ProcessEvent();

            BeginWindows();
            m_View.Draw();
            EndWindows();
        }

        private void ProcessEvent()
        {
            Event e = Event.current;
            Vector2 mousePosition = e.mousePosition;

            if (e.button == 1 && e.type == EventType.MouseDown)
            {
                if (m_View.ClickOnNode(mousePosition, out BaseEventNode node))
                {
                    GenericMenu menu = new GenericMenu();
                    if (node is EventGroupNode)
                    {
                        menu.AddItem(new GUIContent("Add Event"), false, m_View.AddNode<SingleEventNode>, node);
                        menu.AddItem(new GUIContent("Add Event Group"), false, m_View.AddNode<EventGroupNode>, node);
                        if (node.eventName == "Root")
                        {
                            menu.AddDisabledItem(new GUIContent("Delete"));
                        }
                        else
                        {
                            menu.AddItem(new GUIContent("Delete"), false, m_View.DeleteNode, node);
                        }
                    }
                    else if (node is SingleEventNode)
                    {
                        menu.AddItem(new GUIContent("Delete"), false, m_View.DeleteNode, node);
                    }
                    menu.ShowAsContext();
                    e.Use();
                }
            }
            else if (e.button == 0 && e.type == EventType.MouseDrag)
            {
                if (m_View.ClickOnNode(mousePosition, out BaseEventNode node))
                {
                    m_View.Move(e.delta);
                }
            }
        }

        private void OnDisable()
        {
            m_View.SaveData();
            m_View = null;
        }
    }
}
