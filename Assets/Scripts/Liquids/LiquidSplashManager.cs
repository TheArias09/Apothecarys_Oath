using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSplashManager : MonoBehaviour
{
    private ParticleSystem splashSystem;
    private ParticleSystem.MainModule splashSystemMain;
    private ParticleSystem.TrailModule splashSystemTrails;

    private LiquidFlowManager liquidFlow;
    private ParticleSystem.MainModule liquidFlowMain;
    
    private GameObject potion;
    private LiquidVisualsManager potionLiquids;
    
    public bool isSplashing = true;
    void Start()
    {
        liquidFlow = transform.parent.GetComponent<LiquidFlowManager>();
        liquidFlowMain = liquidFlow.gameObject.GetComponent<ParticleSystem>().main;

        splashSystem = gameObject.GetComponent<ParticleSystem>();
        splashSystemMain = splashSystem.main;
        splashSystemTrails = splashSystem.trails;
        
        potion = transform.parent.transform.parent.gameObject;
        potionLiquids = potion.GetComponent<LiquidVisualsManager>();

        isSplashing = liquidFlow.IsFlowing;
    }

    void FixedUpdate()
    {
        SetTransform();
        SetColor();
        SetScale();

        isSplashing = liquidFlow.IsFlowing;
        
        if(isSplashing)
        {
            if(!splashSystem.isPlaying)
            {
                splashSystem.Play();
            }
        }else{
            if(splashSystem.isPlaying)
            {
                splashSystem.Stop();
            }
        }
    }

    private void SetTransform()
    {
        transform.position = liquidFlow.GetTargetPointPosition();
        transform.rotation = liquidFlow.GetTargetPointRotation();
    }
    
    private void SetColor()
    {
        if (potionLiquids.LiquidCount >= 1)
        {
            splashSystemMain.startColor = new ParticleSystem.MinMaxGradient( potionLiquids.Liquids[potionLiquids.LiquidCount-1].liquidColor );
        }
        else
        {
            splashSystemMain.startColor = new ParticleSystem.MinMaxGradient( Color.white );;
        }
    }

    private void SetScale()
    {
        transform.localScale = Vector3.one * liquidFlow.GetSplashScale();
    }
}
