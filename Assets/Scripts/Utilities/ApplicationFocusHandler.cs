using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ApplicationFocusHandler : MonoBehaviour
{
    [SerializeField] UnityEvent OnPause;
    [SerializeField] UnityEvent OnResume;

    private float baseTimeScale = 1f;
    private float pauseTimeScale = 0f;

    private void OnApplicationFocus(bool focus)
    {
        if (Application.platform != RuntimePlatform.Android) return;

        if(focus)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Resume()
    {
        Time.timeScale = baseTimeScale;
        OnResume?.Invoke();
    }

    private void Pause()
    {
        Time.timeScale = pauseTimeScale;
        OnPause?.Invoke();
    }
}
