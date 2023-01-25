using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventRegister : MonoBehaviour
{
    [SerializeField] List<GameActionRegistration> gameActionRegistrations = new List<GameActionRegistration>();

    [System.Serializable]
    struct GameActionRegistration
    {
        public GameAction gameAction;
        public UnityEvent unityEvent;
    }

    private void OnEnable()
    {
        foreach (var registration in gameActionRegistrations)
        {
            registration.gameAction.RegisterHandler(registration.unityEvent.Invoke);
        }
    }

    private void OnDisable()
    {
        foreach (var registration in gameActionRegistrations)
        {
            registration.gameAction.UnregisterHandler(registration.unityEvent.Invoke);
        }
    }
}
