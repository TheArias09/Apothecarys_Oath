using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    [SerializeField] GameObject fantomAudioSourcePrefab;
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

    public void LaunchOnDestroySFXPlay()
    {
        ChooseRandomClip();
        if(fantomAudioSourcePrefab != null)
        {
            GameObject fantomSourceObject = Instantiate(fantomAudioSourcePrefab, transform.position, transform.rotation);
            var fantomAudioSource = fantomSourceObject.GetComponent<FantomAudioSource>();
            fantomAudioSource.SetFantomAudioClip(audioSource.clip);
            fantomAudioSource.PlaySoundAndDestroy();
        }
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
