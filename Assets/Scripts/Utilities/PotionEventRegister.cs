using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotionEventRegister : MonoBehaviour
{
    [SerializeField] GameAction OnStart;
    [SerializeField] GameAction OnStop;
    [SerializeField] GameAction OnHelp;
    [SerializeField] GameAction OnCloseHelp;

    [SerializeField] UnityEvent OnStartUnity;
    [SerializeField] UnityEvent OnStopUnity;
    [SerializeField] UnityEvent OnHelpUnity;
    [SerializeField] UnityEvent OnCloseHelpUnity;

    private void OnEnable()
    {
        OnStart.RegisterHandler(OnStartUnity.Invoke);
        OnStop.RegisterHandler(OnStopUnity.Invoke);
        OnHelp.RegisterHandler(OnHelpUnity.Invoke);
        OnCloseHelp.RegisterHandler(OnCloseHelpUnity.Invoke);
    }

    private void OnDisable()
    {
        OnStart.UnregisterHandler(OnStartUnity.Invoke);
        OnStop.UnregisterHandler(OnStopUnity.Invoke);
        OnHelp.UnregisterHandler(OnHelpUnity.Invoke);
        OnCloseHelp.UnregisterHandler(OnCloseHelpUnity.Invoke);
    }
}
