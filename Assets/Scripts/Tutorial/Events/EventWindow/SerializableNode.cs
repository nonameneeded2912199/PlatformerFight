using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.EventSystem
{
    [System.Serializable]
    public class SerializableNode
    {
        public string name;
        public string type;
        public int id;
        public Rect rect;
        public List<int> children;
    }
}
