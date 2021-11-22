using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public float healthIncreased = 20;

    [SerializeField]
    private AudioClip healthPickupAudioClip;

    [SerializeField]
    private ParticleSystem healthPickupParticles;
    [SerializeField] 
    private GameObject healthText;

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
            if (other.GetComponent<Health>().GetCurrentHealth() < 100)
            {
                other.GetComponent<Health>().IncreaseHealth(healthIncreased);

                EventCallbacks.EventHelper.CreateSoundEvent(gameObject, healthPickupAudioClip);

                EventCallbacks.EventHelper.CreateParticleEvent(gameObject, healthPickupParticles);

                isPickedUp = true;
                EventCallbacks.EventHelper.CreateDeathEvent(gameObject);
                
                //Destroy(gameObject);
                //isPickedUp = true;
                //gameObject.SetActive(false);
            }
            else {
                healthText.SetActive(true);
            }
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && healthText.activeSelf)
        {
            healthText.SetActive(false);
        }
    }
    public float GetHealthPickUpPosX()
    {
        return posX;
    }
    public float GetHealthPickUpPosY()
    {
        return posY;
    }
    public float GetHealthPickUpPosZ()
    {
        return posZ;
    }

    public bool HealthIsPickedUp()
    {
        return isPickedUp;
    }
}
