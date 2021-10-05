using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventSystem
{
    [CreateAssetMenu(fileName = "NewEventGraph", menuName = "EventSystem/EventGraph")]
    public class EventGraph : ScriptableObject
    {
        public List<SerializableNode> nodes = new List<SerializableNode>();
        public int currentID;
    }
}
