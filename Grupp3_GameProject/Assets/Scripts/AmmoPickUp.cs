using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField]
    private AudioClip ammoPickupSound;

    [SerializeField]
    private ParticleSystem ammoParticleSystem;

    private float posX;
    private float posY;
    private float posZ;
    private bool isPickedUp = false;

    private void Update()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncreaseAmmo())
            {
                //Destroy(gameObject);
                isPickedUp = true;
                EventCallbacks.EventHelper.CreateSoundEvent(gameObject, ammoPickupSound);
                EventCallbacks.EventHelper.CreateParticleEvent(gameObject, ammoParticleSystem);
                EventCallbacks.EventHelper.CreateDeathEvent(gameObject);
                
                //gameObject.SetActive(false);
            } 
        }
    }

    public float GetAmmoPickUpPosX()
    {
        return posX;
    }
    public float GetAmmoPickUpPosY()
    {
        return posY;
    }
    public float GetAmmoPickUpPosZ()
    {
        return posZ;
    }

    public bool AmmoIsPickedUp()
    {
        return isPickedUp;
    }
}