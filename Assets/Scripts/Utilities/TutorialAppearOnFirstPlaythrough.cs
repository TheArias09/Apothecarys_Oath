using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialAppearOnFirstPlaythrough : MonoBehaviour
{
    [SerializeField] UnityEvent OnFirstPlaythrough;

    private string playerPrefsKey = "First Playthrough";

    private void Start()
    {
        if(!PlayerPrefs.HasKey(playerPrefsKey))
        {
            OnFirstPlaythrough?.Invoke();
        }
    }

    public void ConfirmHavingDoneAPlaythrough()
    {
        if (!PlayerPrefs.HasKey(playerPrefsKey))
        {
            PlayerPrefs.SetInt(playerPrefsKey, 0);
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
    }
}
