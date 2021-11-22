using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Molly Röle
[CreateAssetMenu()]
public class SnailSproutAtkState : State
{
    public float speed = 10;

    private SnailBoss snailBoss;
    private bool foundSpot;
    private Vector3 mouthPos;
    private int sproutRange = 10;
    private int playerLayer = 0; //Default Layer
    //private float timer = 0;
    //private float waitTime = 2.0f;

    protected override void Initialize()
    {
        snailBoss = (SnailBoss)owner;
        Debug.Assert(snailBoss);
    }

    public override void Enter()
    {
        Debug.Log("Sprout state entered!");
        snailBoss.body.SetActive(true);
        snailBoss.isVulnerable = true;
        mouthPos = snailBoss.GetMouthTransform().transform.position;
    }

    public override void RunUpdate()
    {
        if (!snailBoss.awake)
        {
            stateMachine.ChangeState<SnailSleepingState>();
        }

        //First just move to a random location (choosing from chargePoints atm) 
        //Only do this once
        if (!foundSpot)
        {
            Vector3 targetPosition;
            Vector3 newPosition;
            int index = Random.Range(0, 3);
            targetPosition = new Vector3(snailBoss.GetChargePoints()[index].transform.position.x, snailBoss.transform.position.y, snailBoss.GetChargePoints()[index].transform.position.z);
            newPosition = Vector3.MoveTowards(snailBoss.transform.position, targetPosition, speed * Time.deltaTime);
            snailBoss.UpdatePosition(newPosition);

            foundSpot = true;
        }
        //Start Sprouting at chosen location
        SproutAttack();  
    }

    private void SproutAttack()
    {
        float rotationAmount = 20;
        //Stand still while rotating around in a circle
        snailBoss.transform.RotateAround(snailBoss.transform.position, Vector3.up, rotationAmount * Time.deltaTime);
        //Spruta sporer
        //från någon sorts transform som man placerat ut nånstans i snigelns face
            Physics.Linecast
            (
            mouthPos, 
            new Vector3(mouthPos.x + sproutRange, mouthPos.y + sproutRange, mouthPos.z + sproutRange), 
            out RaycastHit hitInfo,
            playerLayer
            );
       
    }


    //private void OldSproutStuff()
    //{
    //    //cache initial rotation
    //    Transform initialTransform = snailBoss.transform;

    //    float rotationAmount = 20;

    //    float rotationLimit = initialTransform.rotation.y + rotationAmount;

    //    //Står still men roterar fram och tillbaka i en cone-shape (alternativt full cirkel)
    //    snailBoss.transform.RotateAround(snailBoss.transform.position, Vector3.up, rotationAmount * Time.deltaTime);

    //    //Kolla om den hunnit rotera hela vägen
    //    if (snailBoss.transform.rotation.y >= rotationLimit)
    //    {
    //        Debug.Log("Reached rotation limit");

    //        //Pausa
    //        while (timer < waitTime)
    //        {
    //            timer += Time.deltaTime;
    //            Debug.Log("Pausing for " + timer + "..");
    //        }
    //        //Rotera åt andra hållet
    //        //snailBoss.transform.RotateAround(snailBoss.transform.position, Vector3.down, 20 * Time.deltaTime);
    //        //timer = 0;
    //    }
    //}
}
