using System.Collections;
using UnityEngine;
//Author: Filip Hallberg
//Secondary Author: Molly R�le

public class Shooter : MonoBehaviour
{
    //L�gga i player ist�llet? Oklart
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private ParticleSystem shootingParticles;

    public void Fire(GameObject bullet, Vector3 firingPosition, Vector3 firingRotation)
    {
        Object.Instantiate(bullet, firingPosition, Quaternion.Euler(firingRotation));
        
        //Fire Sound
        EventCallbacks.EventHelper.CreateSoundEvent(gameObject, shootingSound);

        //Fire Particles
        EventCallbacks.EventHelper.CreateParticleEvent(bullet, shootingParticles); //Maybe do this one only in bullet instead, we'll see

        //GameController.SetAmmoCount(GameController.GetAmmoCount() - 1);
        GameController.GameControllerInstance.AmmoCount = (GameController.GameControllerInstance.AmmoCount - 1);
        if (GameController.GameControllerInstance.AmmoCount < 0)
        {
            //GameController.SetAmmoCount(0);
            GameController.GameControllerInstance.AmmoCount = 0;
        }
    }
}
