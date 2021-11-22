using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ammoGameObject;
    [SerializeField] private float spawnTimer;

    private Transform ammoSpawnerTransform;

    private void Start()
    {
        ammoSpawnerTransform = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(SpawnPickUp());
        }
    }

    private IEnumerator SpawnPickUp()
    {
        yield return new WaitForSeconds(spawnTimer);
        Instantiate(ammoGameObject, ammoSpawnerTransform);
    }
}
