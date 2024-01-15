using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeColliderManager : MonoBehaviour
{
    enum GameModes
    {
        Demo, Infinite
    }

    [SerializeField] private GameModes gameMode;
    
    void OnTriggerEnter(Collider objectName)
    {
        Debug.Log("Entered collision on mode " + gameMode);
        switch (gameMode)
        {
            case GameModes.Demo :
                GameManager.Instance.SetDemoMode();
                break;
            case GameModes.Infinite :
                GameManager.Instance.SetInfiniteMode();
                break;
        }
    }
}
