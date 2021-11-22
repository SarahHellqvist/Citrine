using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu()]
public class AI_BaseState : State
{
    protected PatrollingEnemy enemy;
    protected Transform playerPos;
    protected NavMeshAgent navAgent;
    protected LayerMask collisionLayer;
    protected override void Initialize()
    {
        enemy = (PatrollingEnemy)owner;
        Debug.Assert(enemy);
        navAgent = enemy.GetNavAgent();
        playerPos = enemy.GetPlayerTransform();
        collisionLayer = enemy.GetCollisionLayer();
    }

    public override void RunUpdate()
    {

    }
}
