//  By Tommi T
//
//  Used to release steam and play sound

using UnityEngine;
using System.Collections;

public class Steam : MonoBehaviour
{

    private float timer;
    private ParticleSystem steamParticles;
    private AudioSource steamAudioSource;


    void Start()
    {
        steamParticles = GetComponent<ParticleSystem>();
        steamAudioSource = GetComponent<AudioSource>();
        timer = 8f;
    }

    // Every 8 seconds steam will be released and sound is played
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 8f;
            steamParticles.Play();
            steamAudioSource.Play();
        }
    }
}
