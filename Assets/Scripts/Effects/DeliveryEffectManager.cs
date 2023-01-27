using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryEffectManager : MonoBehaviour
{
    private ParticleSystem particles;
    private bool startedPlaying;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        startedPlaying = false;
    }

    void Update()
    {
        if (!startedPlaying && particles.isPlaying) //Checks if it starts
        {
            startedPlaying = true;
        }

        if (startedPlaying && !particles.isPlaying) //Has started and ended
        {
            Destroy(gameObject); //We destroy the effect
        }
    }
}
