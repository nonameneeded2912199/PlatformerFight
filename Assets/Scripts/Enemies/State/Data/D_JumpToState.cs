using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewJumpToStateData", menuName = "Data/State Data/Jump To State")]
public class D_JumpToState : ScriptableObject
{
    public float jumpHeight = 5f;
    public int numJumps = 1;
    public float duration = 1f;
    public bool snapping = false;
}
