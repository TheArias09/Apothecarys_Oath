using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//LOOK AT THAT https://www.ronja-tutorials.com/post/021-plane-clipping/

public class LiquidSurfaceManager : MonoBehaviour
{
    private LiquidVisualsManager liquidVisualsManager;

    private void Awake()
    {
        liquidVisualsManager = transform.parent.GetComponent<LiquidVisualsManager>();
    }

    private void FixedUpdate()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        
    }
}
