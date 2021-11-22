using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{    
    [Header("LayerMask")]
    [SerializeField] private LayerMask collisionMask;

    [Header("Timers")]
    [SerializeField, Min(0.01f)] private float crumbleTimer;
    [SerializeField, Min(0.01f)] private float respawnTimer;

    [Header("Effects")]
    [SerializeField] private AudioClip sound;
    [SerializeField] private ParticleSystem particles;

    [Header("References")]
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private MeshRenderer meshRenderer;

    //Control field
    private bool isCrumbling = false;

    void Start()
    {
        if(triggerCollider == null)
        {
            triggerCollider = GetComponent<BoxCollider>();
        }
        if(meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isCrumbling)
        {
            StartCoroutine(Crumble());
            //Sound effect
            EventCallbacks.EventHelper.CreateSoundEvent(gameObject, sound);
            //VFX
            //EventCallbacks.EventHelper.CreateParticleEvent(gameObject, particles);
        }
    }

    //Despawning the platform
    private IEnumerator Crumble()
    {
        isCrumbling = true; 
        yield return new WaitForSeconds(crumbleTimer);
        triggerCollider.enabled = false;
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        StartCoroutine(Respawn());
    }

    //Respawning the platform
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTimer);
        triggerCollider.enabled = true;
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
        isCrumbling = false;
    }
}
