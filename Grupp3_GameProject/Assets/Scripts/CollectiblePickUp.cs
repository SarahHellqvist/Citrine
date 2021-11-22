using UnityEngine;

public class CollectiblePickUp : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] collectibleSounds;
    [SerializeField] private ParticleSystem collectibleParticles;

    private int clipIndex;

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
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncreaseCollectible();

            //Get random sound from array (differing pitches)
            clipIndex = Random.Range(1, collectibleSounds.Length);
            AudioClip clip = collectibleSounds[clipIndex];

            //Play Sound
            EventCallbacks.EventHelper.CreateSoundEvent(gameObject, clip);

            //Fire Particles
            EventCallbacks.EventHelper.CreateParticleEvent(gameObject, collectibleParticles);

            isPickedUp = true;

            //De-activate gameobject
            EventCallbacks.EventHelper.CreateDeathEvent(gameObject);

            //source.PlayOneShot(clip);
            //collectibleSounds[clipIndex] = collectibleSounds[0];
            //collectibleSounds[0] = clip;
            //Destroy(gameObject);
            //isPickedUp = true;
            //gameObject.SetActive(false);
        }
    }

    public float GetCollectiblePickUpPosX()
    {
        return posX;
    }
    public float GetCollectiblePickUpPosY()
    {
        return posY;
    }
    public float GetCollectiblePickUpPosZ()
    {
        return posZ;
    }

    public bool CollectibleIsPickedUp()
    {
        return isPickedUp;
    }
}
