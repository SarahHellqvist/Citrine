using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingPlatformTrigger : MonoBehaviour
{
    [SerializeField] private MovingPlatform movingPlatform;
   private void OnTriggerEnter(Collider other)
   {
       if(other.gameObject.tag == "Player")
       {
           movingPlatform.SetAutomatic(true);
       }
   }
}
