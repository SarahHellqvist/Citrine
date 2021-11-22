using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Molly R�le
public class SnailBoss : MonoBehaviour, IKillable
{
    public State[] states;
    public BoxCollider awakeTrigger;
    public GameObject shell;
    public GameObject body;
    public bool awake = false;
    public bool isVulnerable = false;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform mouth;

    [SerializeField]
    private Transform [] chargePoints;

    [SerializeField]
    private GameObject rotationTarget;

    private Health health;
    private Rigidbody rb;
    private StateMachine stateMachine;
    private bool isDead = false;
    private float posX;
    private float posY;
    private float posZ;

    private void Awake()
    {
        stateMachine = new StateMachine(this, states);
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        stateMachine.RunUpdate();

        //F�r att dra ner o upp HP:t och kunna testa om den byter state vid r�tt HP
        if(Input.GetKeyDown(KeyCode.V))
        {
            DecreaseHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            health.InitializeHealth();
            //Debug.Log("Health reset to 500");
        }

    }
    public bool BossIsDead()
    {
        return isDead;
    }
    public float GetBossPosX()
    {
        return posX;
    }
    public float GetBossPosY()
    {
        return posY;
    }
    public float GetBossPosZ()
    {
        return posZ;
    }
    public Transform GetPlayerTransform()
    {
        return player;
    }

    public Transform GetMouthTransform()
    {
        return mouth;
    }

    public Transform[] GetChargePoints()
    {
        return chargePoints;
    }

    public GameObject GetRotationTarget()
    {
        return rotationTarget;
    }

    public void UpdatePosition(Vector3 pos)
    {
        rb.MovePosition(pos);
    }

    public float GetHealth()
    {
        return health.GetCurrentHealth();
    }

    public void DecreaseHealth(float damage)
    {
        //Boss in invulnerable in SleepingState and RollAtkState
        if (isVulnerable)
        {
            health.DecreaseHealth(damage);
            //Debug.Log("Boss taking damage, new HP is: " + GetHealth());
        }
        
    }

    public void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }
}
