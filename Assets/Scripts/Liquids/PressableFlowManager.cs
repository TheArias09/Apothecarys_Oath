using System.Collections;
using System.Collections.Generic;
using Recipients;
using UnityEngine;

public class PressableFlowManager : MonoBehaviour
{
    private Transform flowTransform;
    //private Vector3 flowPosition;
    
    private GameObject potion;
    private Flowing potionFlowing;
    private LiquidVisualsManager potionLiquids;
    
    private int liquidCount;
    private int previousLiquidCount;

    private ParticleSystem flowSystem;
    private ParticleSystem.MainModule flowSystemMain;
    private ParticleSystem.TrailModule flowSystemTrails;
    private ParticleSystem.EmissionModule flowEmission;

    private bool targetPotionExists;
    private GameObject targetPotion;
    private GameObject previousTargetPotion;
    //private Vector3 targetPotionPosition;
    private LiquidVisualsManager targetPotionLVM;
    private WobbleManager targetPotionWM;

    private bool isFlowing = true;

    public bool IsFlowing => isFlowing;
    public bool TargetPotionExists => targetPotionExists;
    
    void Start()
    {
        flowTransform = transform;
        //flowPosition = flowTransform.position;

        potion = flowTransform.parent.gameObject;
        potionFlowing = potion.GetComponent<Flowing>();
        potionLiquids = potion.GetComponent<LiquidVisualsManager>();
        
        liquidCount = potionLiquids.LiquidCount;
        previousLiquidCount = liquidCount;

        flowSystem = gameObject.GetComponent<ParticleSystem>();
        flowSystemMain = flowSystem.main;
        flowSystemTrails = flowSystem.trails;
        flowEmission = flowSystem.emission;

        isFlowing = potionFlowing.IsFlowing;
        flowSystem.Play();
        
        targetPotion = potionFlowing.GetTargetPotion();
        targetPotionExists = (targetPotion != null);
        if (targetPotionExists)
        {
            previousTargetPotion = targetPotion;
            //targetPotionPosition = targetPotion.transform.position;
            targetPotionLVM = targetPotion.GetComponent<LiquidVisualsManager>();
            targetPotionWM = targetPotion.GetComponent<WobbleManager>();
        }
        SetPosition();
        SetHeight();
        SetColor();
        SetWidth();
    }

    void FixedUpdate()
    {
        SetPosition();
        SetHeight();

        isFlowing = potionFlowing.IsFlowing;
        
        //Avec le système
        if(isFlowing)
        {
            if(!flowSystem.isPlaying)
            {
                flowSystem.Play();
            }
        }else{
            if(flowSystem.isPlaying)
            {
                
                //flowSystem.Clear();
                flowSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        //Avec les émissions
        /*
        if(isFlowing)
        {
            if(!flowEmission.enabled)
            {
                flowEmission.enabled = true;
            }
        }else{
            if(flowEmission.enabled)
            {
                
                flowEmission.enabled = false;
            }
        }
        */
        
        targetPotion = potionFlowing.GetTargetPotion();
        
        targetPotionExists = (targetPotion != null);
        if (targetPotion != previousTargetPotion && targetPotionExists)
        {
            //targetPotionPosition = targetPotion.transform.position;
            targetPotionLVM = targetPotion.GetComponent<LiquidVisualsManager>();
            targetPotionWM = targetPotion.GetComponent<WobbleManager>();
            previousTargetPotion = targetPotion;
        }

        liquidCount = potionLiquids.LiquidCount;
        if (liquidCount != previousLiquidCount)
        {
            SetColor();
            SetWidth();
            previousLiquidCount = liquidCount;
        }
    }

    void SetPosition()
    {
        flowTransform.position = potionFlowing.GetFlowPoint() + new Vector3(0,0.01f,0);
        flowTransform.rotation = Quaternion.identity;
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
    
    void SetHeight()
    {
        float desiredHeight;
        desiredHeight = flowTransform.position.y - GetTargetPointY();

        flowSystemMain.gravityModifierMultiplier = 0.25f * desiredHeight; //Lifetime de 0.95
        //flowSystemMain.gravityModifierMultiplier = 0.45f * desiredHeight; //Lifetime de 0.5
    }

    void SetWidth()
    {
        flowSystemTrails.widthOverTrail = (ParticleSystem.MinMaxCurve)(0.05 * potionLiquids.FindViscosityFactor());
    }

    public float GetTargetPointY()
    {
        if (targetPotionExists)
        {
            return targetPotion.transform.position.y + 
                   targetPotionLVM.FindTotalDisplayedFill() * targetPotion.transform.lossyScale.y / targetPotionLVM.BaseScale;
        }
        
        Vector3 startPoint = potionFlowing.GetFlowPoint();
        Ray ray = new Ray(startPoint, Vector3.down);
        
        bool res = Physics.Raycast(ray, out RaycastHit hit,5 );

        if (!res)
        {
            return 0;
        }
        
        return hit.point.y;

    }

    public Vector3 GetTargetPointPosition()
    {
        if (targetPotionExists)
        {
            return new Vector3(targetPotion.transform.position.x,targetPotion.transform.position.y + targetPotionLVM.FindTotalDisplayedFill()* targetPotion.transform.lossyScale.y / targetPotionLVM.BaseScale,targetPotion.transform.position.z);
        }

        //Vector3 startPoint = potionFlowing.GetFlowPoint();
        //return new Vector3(startPoint.x, 0, startPoint.z);
        
        Vector3 startPoint = potionFlowing.GetFlowPoint();
        Ray ray = new Ray(startPoint, Vector3.down);
        
        bool res = Physics.Raycast(ray, out RaycastHit hit,5 );
        
        if (hit.transform.gameObject.layer == 11)
        {
            return new Vector3(42000, 42000, 42000);
        }

        if (!res)
        {
            return new Vector3(startPoint.x, 0, startPoint.z);
        }
        
        return hit.point;
    }

    
    public Quaternion GetTargetPointRotation()
    {
        if (targetPotionExists)
        {
            float wobbleAmountX = targetPotionWM.WobbleAmountX;
            float wobbleAmountZ = targetPotionWM.WobbleAmountZ;
        
            //Setting the desired rotation
            float rotationZ = -90f * wobbleAmountZ * Mathf.Deg2Rad * 90;
            float rotationX = -90f * wobbleAmountX * Mathf.Deg2Rad * 90;

            if (targetPotionLVM.LiquidCount == 0)
            {
                rotationZ = 0f;
                rotationX = 0f;
            }

            return Quaternion.Euler(rotationZ,0f,rotationX);        
        }
        return new Quaternion();
    }

    public float GetSplashScale()
    {
        if (targetPotionExists)
        {
            return targetPotionLVM.FindInsideScale();
        }
        return 0.004f;
    }
}
