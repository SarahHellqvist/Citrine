using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Molly Röle

[CreateAssetMenu()]
public class AIPatrolState : AI_BaseState
{
    [SerializeField]
    private float sightDistance;

    [SerializeField]
    private float speed;
    //PatrollingEnemy enemy;

    private Transform currentPatrol;


    //protected override void Initialize()
    //{
    //    enemy = (PatrollingEnemy)owner;
    //    Debug.Assert(enemy);
    //}

    public override void Enter()
    {
        currentPatrol = playerPos;
        navAgent.SetDestination(currentPatrol.position);
        navAgent.speed = speed;
    }
    public override void RunUpdate()
    {
        float dist = Vector3.Distance(enemy.transform.position, playerPos.position);
        if (!Physics.Linecast(enemy.transform.position, playerPos.position, collisionLayer) && dist <= sightDistance)//kolla även någon sight-distance
        {
            stateMachine.ChangeState<AIChasePlayerState>();
        }
        if (navAgent.remainingDistance < 2.0f)
        {
            currentPatrol = enemy.GetPatrolPoint();
            navAgent.SetDestination(currentPatrol.position);
        }
    }
}