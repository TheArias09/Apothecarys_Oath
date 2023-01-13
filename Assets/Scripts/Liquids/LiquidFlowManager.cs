using System.Collections;
using System.Collections.Generic;
using Recipients;
using UnityEngine;

public class LiquidFlowManager : MonoBehaviour
{
    private GameObject potion;
    private Flowing potionFlowing;
    private LiquidVisualsManager potionLiquids;
    
    private int liquidCount;
    private int previousLiquidCount;

    private ParticleSystem flowSystem;
    private ParticleSystem.MainModule flowSystemMain;
    private ParticleSystem.TrailModule flowSystemTrails;

    private bool isFlowing = true;

    void Start()
    {
        potion = transform.parent.gameObject;
        potionFlowing = potion.GetComponent<Flowing>();
        potionLiquids = potion.GetComponent<LiquidVisualsManager>();
        
        liquidCount = potionLiquids.LiquidCount;
        previousLiquidCount = liquidCount;

        flowSystem = gameObject.GetComponent<ParticleSystem>();
        flowSystemMain = flowSystem.main;
        flowSystemTrails = flowSystem.trails;

        SetPosition();
        SetColor();
    }

    void FixedUpdate()
    {
        SetPosition();
        
        liquidCount = potionLiquids.LiquidCount;
        if (liquidCount != previousLiquidCount)
        {
            SetColor();
            previousLiquidCount = liquidCount;
        }

        if(Input.GetKeyDown(KeyCode.O))
        { 
            isFlowing = !isFlowing;
        }
     
        if(isFlowing)
        {
            if(!flowSystem.isPlaying)
            {
                flowSystem.Play();
            }
        }
        else{ 
            if(flowSystem.isPlaying) 
            { 
                flowSystem.Stop(); 
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Setting Color");
            SetColor();
        }
    }

    void SetPosition()
    {
        transform.position = potionFlowing.GetFlowPoint() + new Vector3(0,0,0.01f);
        transform.rotation = Quaternion.identity;
    }
    
    void SetColor()
    {
        if (liquidCount >= 1)
        {
            flowSystemMain.startColor = new ParticleSystem.MinMaxGradient( potionLiquids.Liquids[liquidCount-1].liquidColor );
        }
        else
        {
            flowSystemMain.startColor = new ParticleSystem.MinMaxGradient( Color.white );;
        }
    }

    void SetHeight(float desiredHeight)
    {
        flowSystemMain.gravityModifierMultiplier = 0.25f * desiredHeight;
    }
}
