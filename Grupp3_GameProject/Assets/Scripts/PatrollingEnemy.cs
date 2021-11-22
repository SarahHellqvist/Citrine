using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Molly R�le
public class PatrollingEnemy : MonoBehaviour, IKillable
{
    [SerializeField]
    private AudioClip dyingSound;

    [SerializeField]
    private ParticleSystem dyingParticles;

    [SerializeField]
    private NavMeshAgent navAgent;

    [SerializeField]
    private List<Transform> patrolPoints;

    [SerializeField]
    private Transform player;
   
    [SerializeField]
    private LayerMask collisionLayer;

    [SerializeField]
    private State[] states;

    private StateMachine stateMachine;
    private float posX;
    private float posY;
    private float posZ;
    private bool isDead = false;

    private void Awake()
    {
        stateMachine = new StateMachine(this, states);
        //navAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        stateMachine.RunUpdate();
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
    }

    public LayerMask GetCollisionLayer()
    {
        return collisionLayer;
    }
    public NavMeshAgent GetNavAgent()
    {
        return navAgent;
    }

    public Transform GetPatrolPoint()
    {
        return patrolPoints[Random.Range(0, patrolPoints.Count)];
    }
    public Transform GetPlayerTransform()
    {
        return player;
    }
    public float GetPatrollingEnemyPosX()
    {
        return posX;
    }
    public float GetPatrollingEnemyPosY()
    {
        return posY;
    }
    public float GetPatrollingEnemyPosZ()
    {
        return posZ;
    }

    public bool PatrollingEnemyIsDead()
    {
        return isDead;
    }

    public void Die()
    {
        EventCallbacks.EventHelper.CreateSoundEvent(gameObject, dyingSound);
        EventCallbacks.EventHelper.CreateParticleEvent(gameObject, dyingParticles);

        isDead = true;
        EventCallbacks.EventHelper.CreateDeathEvent(gameObject);

        //gameObject.SetActive(false);
    }
}
