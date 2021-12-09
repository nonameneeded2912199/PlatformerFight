using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Pool
{
    /// <summary>
    /// Object pool
    /// </summary>
    /// <typeparam name="T">Type of elements in pool</typeparam>
    public interface IPool<T>
    {
        void Prewarm(int num);

        T Request();

        void Return(T member);
    }
}