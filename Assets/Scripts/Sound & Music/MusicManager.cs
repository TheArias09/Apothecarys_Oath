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
    [SerializeField] AudioClip gameOverMusic;

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

    private void Update()
    {
        /*
        if (Input.GetKeyDown("a"))
        {
            SwitchToChill();
            return;
        }

        if (Input.GetKeyDown("z"))
        {
            Debug.Log("ZZZZZ");
            SwitchToDay();
            return;
        }

        if (Input.GetKeyDown("e"))
        {
            SwitchToVictory();
            return;
        }

        if (Input.GetKeyDown("r"))
        {
            SwitchToLastChance();
            return;
        }

        if (Input.GetKeyDown("t"))
        {
            SwitchToGameOver();
            return;
        }*/
    }

    public void SwitchToChill()
    {
        musicPlayer.Stop();
        musicPlayer.clip = chillingMusic;
        musicPlayer.Play();
    }

    public void SwitchToDay()
    {
        musicPlayer.Stop();
        musicPlayer.clip = dayMusic;
        musicPlayer.Play();
    }

    public void SwitchToLastChance()
    {
        musicPlayer.Stop();
        musicPlayer.clip = oneChanceLeftMusic;
        musicPlayer.Play();
    }

    public void SwitchToVictory()
    {
        musicPlayer.Stop();
        musicPlayer.clip = victoryMusic;
        musicPlayer.Play();
    }

    public void SwitchToGameOver()
    {
        musicPlayer.Stop();
        musicPlayer.clip = gameOverMusic;
        musicPlayer.Play();
    }
}
