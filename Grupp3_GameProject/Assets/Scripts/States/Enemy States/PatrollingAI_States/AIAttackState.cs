using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Molly R�le

[CreateAssetMenu()]
public class AIAttackState : AI_BaseState
{
    [SerializeField]
    private float chaseDistance;

    [SerializeField]
    private float damage;
    //PatrollingEnemy enemy;

    [SerializeField]
    private float timerValue = 0.5f;

    private float timer;

    //protected override void Initialize()
    //{
    //    enemy = (PatrollingEnemy)owner;
    //    Debug.Assert(enemy);
    //}

    public override void Enter()
    {
        timer = 0;
    }
    public override void RunUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Attack();
            timer = timerValue;
        }

        //if (Vector3.Distance(agent.transform.position, agent.GetPlayerPosition()) <= attackDistance)
        //{
        //    stateMachine.ChangeState<AIPatrolState>();
        //}

        if (Vector3.Distance(enemy.transform.position, playerPos.position) > chaseDistance)
        {
            stateMachine.ChangeState<AIChasePlayerState>();
        }

    }

    private void Attack()
    {
        playerPos.gameObject.GetComponent<Health>().DecreaseHealth(damage);
    }

    //public void ResetState()
    //{
    //    stateMachine.ChangeState<AIPatrolState>();
    //}
}
