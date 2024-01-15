using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantomAudioSource : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetFantomAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void PlaySoundAndDestroy()
    {
        StartCoroutine(PlaySoundAndDestroyCoroutine());
    }

    private IEnumerator PlaySoundAndDestroyCoroutine()
    {
        audioSource.Stop(); //?
        audioSource.loop = false; //just in case
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);

    }

}
