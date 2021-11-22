using UnityEngine;

public class RespawnPointUpdater : MonoBehaviour
{

    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.SetRespawnPoint(respawnPoint.position);
        }
    }
}
