using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField]
    private int requiredShards = 1;
    [Header("Effects")]
    [SerializeField]
    private AudioClip enoughShards;
    [SerializeField]
    private AudioClip spawnedPlatform;
    [SerializeField]
    private ParticleSystem spawnPlatformParticles;
    [Header("Object References")]
    [SerializeField]
    private GameObject gate;
    [SerializeField]
    private GameObject shardText;
    [SerializeField]
    private GameObject platform;

    private bool playedShardSound;

    private void Awake()
    {
        playedShardSound = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameController.GameControllerInstance.ShardCount >= requiredShards)
            {
                gate.SetActive(false);
            }
            else
            {
                shardText.SetActive(true);
                if(platform != null)
                {
                    platform.SetActive(true);
                    EventCallbacks.EventHelper.CreateSoundEvent(platform, spawnedPlatform);
                    EventCallbacks.EventHelper.CreateParticleEvent(platform, spawnPlatformParticles);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && shardText.activeSelf)
        {
            shardText.SetActive(false);
        }
    }

    private void Update()
    {

        if (!playedShardSound && (GameController.GameControllerInstance.ShardCount >= requiredShards))
        {
            EventCallbacks.EventHelper.CreateSoundEvent(GameController.GameControllerInstance.gameObject, enoughShards);
            playedShardSound = true;
        }
    }
}