using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Molly R�le
public class SnailTrigger : MonoBehaviour
{
    public SnailBoss snailBoss;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            snailBoss.awake = true;
            //Debug.Log("Trigger Entered!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            snailBoss.awake = false;
        }
    }
}
