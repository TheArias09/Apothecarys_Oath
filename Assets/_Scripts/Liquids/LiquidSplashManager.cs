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
    private IngredientWrapper potionLiquids;
    //private LiquidVisualsManager potionLiquids;

    private Vector3 targetPosition;
    private bool splashingOnPotion;
    
    public bool isSplashing = true;
    void Start()
    {
        liquidFlow = transform.parent.GetComponent<LiquidFlowManager>();
        liquidFlowMain = liquidFlow.gameObject.GetComponent<ParticleSystem>().main;

        splashSystem = gameObject.GetComponent<ParticleSystem>();
        splashSystemMain = splashSystem.main;
        splashSystemTrails = splashSystem.trails;
        
        potion = transform.parent.transform.parent.gameObject;
        potionLiquids = potion.GetComponent<IngredientWrapper>();
        //potionLiquids = potion.GetComponent<LiquidVisualsManager>();

        isSplashing = liquidFlow.IsFlowing;
    }

    void FixedUpdate()
    {
        SetTransform();
        SetColor();
        SetScale();

        isSplashing = liquidFlow.IsFlowing;
        
        if(isSplashing && !splashingOnPotion)
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
        targetPosition = liquidFlow.GetTargetPointPosition();
        splashingOnPotion = targetPosition == new Vector3(42000, 42000, 42000);
        transform.position = targetPosition;
        transform.rotation = liquidFlow.GetTargetPointRotation();
    }
    
    private void SetColor()
    {
        /*
        if (potionLiquids.LiquidCount >= 1)
        {
            splashSystemMain.startColor = new ParticleSystem.MinMaxGradient( potionLiquids.Liquids[potionLiquids.LiquidCount-1].liquidColor );
        }
        else
        {
            splashSystemMain.startColor = new ParticleSystem.MinMaxGradient( Color.white );;
        }
        */
        if (potionLiquids.Ingredients.Count >= 1)
        {
            splashSystemMain.startColor = new ParticleSystem.MinMaxGradient( potionLiquids.Ingredients[^1].Data.color );
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
