using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryAIPassiveState : State
{
    StationaryEnemy enemy;


    protected override void Initialize()
    {
        enemy = (StationaryEnemy)owner;
        Debug.Assert(enemy);
    }

    public override void RunUpdate()
    {
      
    }
    
}
