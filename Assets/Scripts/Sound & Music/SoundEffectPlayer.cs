using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ChooseRandomClip();
    }

    public void LaunchSFXPlay()
    {
        ChooseRandomClip();
        audioSource.Play();
    }

    public void StopSFXPlay()
    {
        audioSource.Stop();
    }

    private void ChooseRandomClip() {
        audioSource.Stop();
        var index = Random.Range(0, audioClips.Count);
        audioSource.clip = audioClips[index];
    }
}
