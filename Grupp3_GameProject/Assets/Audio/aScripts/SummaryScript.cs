using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerScript : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] jumpSounds;

    public int clipIndex;

    public float volLow = 0.1f;
    public float volHigh = 1.0f;
    public float pitchLow = 0.5f;
    public float pitchHigh = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            source.volume = Random.Range(volLow, volHigh);
            source.pitch = Random.Range(pitchLow, pitchHigh);

            clipIndex = Random.Range(1, jumpSounds.Length);
            AudioClip clip = jumpSounds[clipIndex];
            source.PlayOneShot(clip);
            jumpSounds[clipIndex] = jumpSounds[0];
            jumpSounds[0] = clip;
        }

    }
}
