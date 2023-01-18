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

    private GameObject targetPotion;

    public bool isFlowing = true;

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

        isFlowing = potionFlowing.IsFlowing;
        //flowSystem.Play();
        
        targetPotion = potionFlowing.GetTargetPotion();
        SetPosition();
        SetHeight();
        SetColor();
        SetWidth();
    }

    void FixedUpdate()
    {
        targetPotion = potionFlowing.GetTargetPotion();
        SetPosition();
        SetHeight();
        isFlowing = potionFlowing.IsFlowing;
        liquidCount = potionLiquids.LiquidCount;
        
        if (liquidCount != previousLiquidCount)
        {
            SetColor();
            SetWidth();
            previousLiquidCount = liquidCount;
        }

        if(isFlowing)
        {
            if(!flowSystem.isPlaying)
            {
                flowSystem.Play();
            }
        }else{
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
        transform.position = potionFlowing.GetFlowPoint() + new Vector3(0,0.01f,0);
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

    public float GetTargetPointY()
    {
        return targetPotion.transform.position.y + 
               targetPotion.GetComponent<LiquidVisualsManager>().FindTotalDisplayedFill();
    }

    /*
    public Quaternion getTargetPointRotation()
    {
        targetPotion = potionFlowing.GetTargetPotion();

        float wobbleAmountX = targetPotion.GetComponent<WobbleManager>().WobbleAmountX;
        float wobbleAmountZ = targetPotion.GetComponent<WobbleManager>().WobbleAmountZ;
        
        //Setting the desired rotation
        float rotationZ = -90f * wobbleAmountZ * Mathf.Deg2Rad * 500;
        float rotationX = -90f * wobbleAmountX * Mathf.Deg2Rad * 500;

        if (targetPotion.GetComponent<LiquidVisualsManager>().LiquidCount == 0)
        {
            rotationZ = 0f;
            rotationX = 0f;
        }

        return Quaternion.Euler(rotationZ,0f,rotationX);
    }
    */
    
    void SetHeight()
    {
        float desiredHeight;
        if (targetPotion != null)
        {
            desiredHeight = transform.position.y - GetTargetPointY();
        }
        else
        {
            desiredHeight = transform.position.y;
        }
        
        flowSystemMain.gravityModifierMultiplier = 0.25f * desiredHeight;
    }

    void SetWidth()
    {
        flowSystemTrails.widthOverTrail = (ParticleSystem.MinMaxCurve)(0.05 * potionLiquids.FindViscosityFactor());
    }
}
