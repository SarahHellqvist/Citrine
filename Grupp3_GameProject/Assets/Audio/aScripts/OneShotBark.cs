using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotBark : MonoBehaviour
{

    public AudioSource bark;
    private bool alreadyPlayed;

    // Start is called before the first frame update
    void Start()
    {
        alreadyPlayed = false;
        bark = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !alreadyPlayed) 
        {
            bark.Play();
            alreadyPlayed = true;
        }
        else
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
