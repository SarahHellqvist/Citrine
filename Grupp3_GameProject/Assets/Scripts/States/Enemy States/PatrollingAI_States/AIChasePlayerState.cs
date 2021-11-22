using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Molly Röle

[CreateAssetMenu()]
public class AIChasePlayerState : AI_BaseState
{
    [SerializeField]
    private AudioClip detectionSound;

    [SerializeField]
    private float sightDistance;

    [SerializeField]
    private float attackDistance;

    [SerializeField]
    private float speed;

    public override void Enter()
    {
        navAgent.speed = speed;
        EventCallbacks.EventHelper.CreateSoundEvent(enemy.gameObject, detectionSound);
    }
    public override void RunUpdate()
    {
        navAgent.SetDestination(playerPos.position);
        float dist = Vector3.Distance(enemy.transform.position, playerPos.position);
        if (Physics.Linecast(enemy.transform.position, playerPos.position, collisionLayer) && dist >= sightDistance)
        {
            stateMachine.ChangeState<AIPatrolState>(); //should probably go to some kind of investigation state
        }
        if (Vector3.Distance(enemy.transform.position, playerPos.position) <= attackDistance)
        {
            stateMachine.ChangeState<AIAttackState>();
        }

    }

}