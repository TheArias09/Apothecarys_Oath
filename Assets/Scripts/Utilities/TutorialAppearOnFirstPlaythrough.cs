using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialAppearOnFirstPlaythrough : MonoBehaviour
{
    [SerializeField] UnityEvent OnFirstPlaythrough;

    private string playerPrefsKey = "First Playthrough";

    [SerializeField] bool deleteInStart = false;

    private void Start()
    {
        if (deleteInStart) Delete();

        if(!PlayerPrefs.HasKey(playerPrefsKey))
        {
            OnFirstPlaythrough?.Invoke();
            ConfirmHavingDoneAPlaythrough();
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
