using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Molly R�le

[CreateAssetMenu()]
public class SnailHeadbuttAtkState : State
{
    public float damage;
    public float attackDistance;
    public float speed = 3;
    SnailBoss snailBoss;
    protected override void Initialize()
    {
        snailBoss = (SnailBoss)owner;
        Debug.Assert(snailBoss);
    }

    public override void Enter()
    {
        snailBoss.body.SetActive(true);
        snailBoss.isVulnerable = true;
        Debug.Log("Headbutt state entered!");
    }

    public override void RunUpdate()
    {
        if (!snailBoss.awake)
        {
            stateMachine.ChangeState<SnailSleepingState>();
        }

        //H�mta spelarens position
        Transform playerPos = snailBoss.GetPlayerTransform();

        //R�r dig mot spelaren (i x och z-led, inte y eftersom snigeln ska ej kunna befinna sig ovanf�r marken)
        Vector3 targetPosition = new Vector3(playerPos.position.x, snailBoss.transform.position.y, playerPos.position.z);
        Vector3 newPosition = Vector3.MoveTowards(snailBoss.transform.position, targetPosition, speed * Time.deltaTime);

        snailBoss.UpdatePosition(newPosition);
        //without rigidbody: snailboss.transform.position += newPosition; ???

        //Om/n�r tillr�ckligt n�ra, attackera spelaren
        if (Vector3.Distance(playerPos.position, snailBoss.transform.position) <= attackDistance)
        {
            HeadbuttAttack();
        }
        
        //Villkor f�r att byta state skulle ocks� kunna vara tex en timer ist�llet f�r en HP-gr�ns, kolla med designers
        if(snailBoss.GetHealth() <= 400)
        {
            stateMachine.ChangeState<SnailSproutAtkState>();
            //stateMachine.ChangeState<SnailRollAtkState>();
            //snailBoss.awake = false; //f�r att testa om den byter state r�tt (till sleeping), senare ska den byta till n�got annat state
        }
    }

    private void HeadbuttAttack()
    {
        //G�r skada p� spelaren
        snailBoss.GetPlayerTransform().gameObject.GetComponent<Health>().DecreaseHealth(damage);
    }
}
