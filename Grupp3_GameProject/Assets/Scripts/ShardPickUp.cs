using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardPickUp : MonoBehaviour
{
    [SerializeField]
    private AudioClip shardPickUpAudioClip;

    [SerializeField]
    private ParticleSystem shardPickUpParticles;

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
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncreaseShard();

            EventCallbacks.EventHelper.CreateSoundEvent(gameObject, shardPickUpAudioClip);

            EventCallbacks.EventHelper.CreateParticleEvent(gameObject, shardPickUpParticles);

            isPickedUp = true;
            EventCallbacks.EventHelper.CreateDeathEvent(gameObject);

            //Destroy(gameObject);
            //isPickedUp = true;
            //gameObject.SetActive(false);
        }
    }

    public float GetShardPickUpPosX()
    {
        return posX;
    }
    public float GetShardPickUpPosY()
    {
        return posY;
    }
    public float GetShardPickUpPosZ()
    {
        return posZ;
    }

    public bool ShardIsPickedUp()
    {
        return isPickedUp;
    }
}
