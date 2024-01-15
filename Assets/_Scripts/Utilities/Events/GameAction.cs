using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvents/GameEvent")]
public class GameAction : ScriptableObject
{
    private event Action gameEvent;

    public void Raise()
    {
        gameEvent?.Invoke();
    }

    public void RegisterHandler(Action handler)
    {
        gameEvent += handler;
    }

    public void UnregisterHandler(Action handler)
    {
        gameEvent -= handler;
    }
}

public class GameAction<T> : ScriptableObject
{
    private event Action<T> gameEvent;

    public void Invoke(T parameter)
    {
        gameEvent?.Invoke(parameter);
    }

    public void RegisterHandler(Action<T> handler)
    {
        gameEvent += handler;
    }

    public void UnregisterHandler(Action<T> handler)
    {
        gameEvent -= handler;
    }
}

public class GameAction<T1, T2> : ScriptableObject
{
    private event Action<T1, T2> gameEvent;

    public void Invoke(T1 parameter1, T2 parameter2)
    {
        gameEvent?.Invoke(parameter1, parameter2);
    }

    public void RegisterHandler(Action<T1, T2> handler)
    {
        gameEvent += handler;
    }

    public void UnregisterHandler(Action<T1, T2> handler)
    {
        gameEvent -= handler;
    }
}
