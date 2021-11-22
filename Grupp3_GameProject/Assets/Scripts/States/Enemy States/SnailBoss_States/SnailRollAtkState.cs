using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Molly R�le

[CreateAssetMenu()]
public class SnailRollAtkState : State
{
    //TODO: 
    //Better choosing of chargepoint, more randomness
    //Deal damage when on top of player, not just when close

    public float damage = 10;
    public float attackDistance = 1;
    public float speed = 50;
    public float chargeTimer = 10;

    private SnailBoss snailBoss;
    private int index = 0;
    private float timer = 0;
    private float waitTime = 2.0f;

    //static InnerMonoBehaviour _innerMonoBehaviour; //Stuff needed to be able to use monobehavior things, template

    protected override void Initialize()
    {
        //InnerMonobehaviourDesignPattern(); //Stuff needed to be able to use monobehavior things, template
        snailBoss = (SnailBoss)owner;
        Debug.Assert(snailBoss);
    }

    public override void Enter()
    {
        snailBoss.body.SetActive(false);
        snailBoss.isVulnerable = false;
        Debug.Log("Roll state entered!");
    }

    public override void RunUpdate()
    {
        if (!snailBoss.awake)
        {
            stateMachine.ChangeState<SnailSleepingState>();
        }

        //Villkor f�r att byta state skulle ocks� kunna vara tex en timer ist�llet f�r en HP-gr�ns, kolla med designers
        //if (snailBoss.GetHealth() <= 400)
        //{
        //    stateMachine.ChangeState<SnailHeadbuttAtkState>();
        //}

        Charge();
      
    }
    private void Charge()
    {

        Vector3 targetPosition;
        Vector3 newPosition;

        //Find position to move to
        targetPosition = new Vector3(snailBoss.GetChargePoints()[index].transform.position.x, snailBoss.transform.position.y, snailBoss.GetChargePoints()[index].transform.position.z);
        newPosition = Vector3.MoveTowards(snailBoss.transform.position, targetPosition, speed * Time.deltaTime);
        snailBoss.UpdatePosition(newPosition);

        //Get player position
        Transform playerPos = snailBoss.GetPlayerTransform();

        //If/Whenever close enough to player, attack
        //(Har lagt detta h�r inne eftersom den ska kolla under tiden den r�r sig ifall den �r n�ra spelaren,
        //den ska inte r�ra sig klart innan den kan attackera spelaren
        if (Vector3.Distance(playerPos.position, snailBoss.transform.position) <= attackDistance)
        {
            Attack();
        }

        //Make sure timer doesn't start until snail has reached it's destination
        if (Vector3.Distance(snailBoss.transform.position, targetPosition) < 1.0f)
        {
            //Make a small pause when destination is reached, before moving on to the next position
            if (timer < waitTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                index++;

                //Make sure snail never tries to move to a non-existing position
                if (index > snailBoss.GetChargePoints().Length - 1)
                {
                    index = 0;
                }
                timer = 0;
            }
        }
    }

    private void Attack()
    {
        snailBoss.GetPlayerTransform().gameObject.GetComponent<Health>().DecreaseHealth(damage);
        Debug.Log("Attacking player for " + damage + " damage");
    }


    //Stuff needed to be able to use monobehavior things, template

    //static void InnerMonobehaviourDesignPattern()
    //{
    //    // Creating a GameObject that will hold our "secret" component
    //    var gameObject = new GameObject();
    //    // Properly hiding it from other colleagues that shall not modify it
    //    gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
    //    // Adding the component
    //    _innerMonoBehaviour = gameObject.AddComponent<InnerMonoBehaviour>();
    //}
    //class InnerMonoBehaviour : MonoBehaviour
    //{
    //    void Awake()
    //    {
    //        hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
    //    }
    //}
}
