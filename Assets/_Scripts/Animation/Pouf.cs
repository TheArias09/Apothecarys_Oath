using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouf : MonoBehaviour
{
    private ParticleSystem particles;
    private AudioSource audioSource;

    private void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource= GetComponent<AudioSource>();
    }

    public void MakePouf()
    {
        particles.Clear();
        particles.Play();
        audioSource.Play();
    }
}
