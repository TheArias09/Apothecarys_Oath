using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource musicPlayer;

    [SerializeField] AudioClip chillingMusic;
    [SerializeField] AudioClip dayMusic;
    [SerializeField] AudioClip oneChanceLeftMusic;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] AudioClip gameOverJingle;

    private void Awake()
    {
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = chillingMusic;
        musicPlayer.spatialize = false;
    }

    private void Start()
    {
        musicPlayer.Play();
    }

    /* For debug purposes
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) SwitchToChill();
        else if (Input.GetKeyDown(KeyCode.Z)) SwitchToDay();
        else if (Input.GetKeyDown(KeyCode.E)) SwitchToVictory();
        else if (Input.GetKeyDown(KeyCode.R)) SwitchToLastChance();
        else if (Input.GetKeyDown(KeyCode.T)) SwitchToGameOver();
    }
    */

    public void PauseMusic() => musicPlayer.Pause();
    public void UnpauseMusic() => musicPlayer.UnPause();

    public void SwitchToChill()
    {
        musicPlayer.Stop();
        musicPlayer.loop = true;
        musicPlayer.clip = chillingMusic;
        musicPlayer.Play();
    }

    public void SwitchToDay()
    {
        musicPlayer.Stop();
        musicPlayer.loop = true;
        musicPlayer.clip = dayMusic;
        musicPlayer.Play();
    }

    public void SwitchToLastChance()
    {
        musicPlayer.Stop();
        musicPlayer.loop = true;
        musicPlayer.clip = oneChanceLeftMusic;
        musicPlayer.Play();
    }

    public void SwitchToVictory()
    {
        musicPlayer.Stop();
        musicPlayer.loop = true;
        musicPlayer.clip = victoryMusic;
        musicPlayer.Play();
    }

    public void SwitchToGameOver()
    {
        StartCoroutine(GameOverMusicTransition());
    }

    private IEnumerator GameOverMusicTransition()
    {
        musicPlayer.Stop();
        musicPlayer.loop = false;
        musicPlayer.clip = gameOverJingle;
        musicPlayer.Play();

        while (musicPlayer.isPlaying)
        {
            yield return null;
        }

        musicPlayer.Stop(); //?
        musicPlayer.clip = chillingMusic;
        musicPlayer.Play();
        
    }
}
