using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventSystem
{
    public class SingleEventNode : BaseEventNode
    {
        public SingleEventNode()
        {
            nodeTitle = "Event";
            textColor = Color.cyan;
        }

        public override void CopyNode(List<SerializableNode> containers)
        {
            if (!legal)
                return;

            SerializableNode snode = new SerializableNode();
            snode.type = "Single";
            snode.name = eventName;
            snode.id = id;
            snode.rect = nodeRect;
            containers.Add(snode);
        }
    }
}
