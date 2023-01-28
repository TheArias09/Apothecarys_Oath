using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
}
