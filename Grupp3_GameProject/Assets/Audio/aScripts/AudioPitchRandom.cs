using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPitchRandom : MonoBehaviour
{

    private AudioSource source;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.time = Random.Range(0, clip.length);
        source.pitch = Random.Range(0.8f, 1.1f);
        source.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
