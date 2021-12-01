using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperJoe_DeadState : DeadState
{
    private SniperJoe enemy;

    public SniperJoe_DeadState(FiniteStateMachine stateMachine, SniperJoe enemy, string animBoolName, D_DeadState stateData)
        : base(stateMachine, enemy, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
