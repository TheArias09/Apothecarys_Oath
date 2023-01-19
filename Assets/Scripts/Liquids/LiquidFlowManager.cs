using System.Collections;
using System.Collections.Generic;
using Recipients;
using UnityEngine;

public class LiquidFlowManager : MonoBehaviour
{
    private Transform flowTransform;
    private Vector3 flowPosition;
    
    private GameObject potion;
    private Flowing potionFlowing;
    private LiquidVisualsManager potionLiquids;
    
    private int liquidCount;
    private int previousLiquidCount;

    private ParticleSystem flowSystem;
    private ParticleSystem.MainModule flowSystemMain;
    private ParticleSystem.TrailModule flowSystemTrails;

    private bool targetPotionExists;
    private GameObject targetPotion;
    private GameObject previousTargetPotion;
    private Vector3 targetPotionPosition;
    private LiquidVisualsManager targetPotionLVM;
    private WobbleManager targetPotionWM;

    private bool isFlowing = true;

    public bool IsFlowing => isFlowing;
    public bool TargetPotionExists => targetPotionExists;
    
    void Start()
    {
        flowTransform = transform;
        flowPosition = flowTransform.position;

        potion = flowTransform.parent.gameObject;
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
        targetPotionExists = (targetPotion != null);
        if (targetPotionExists)
        {
            previousTargetPotion = targetPotion;
            targetPotionPosition = targetPotion.transform.position;
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
        
        targetPotion = potionFlowing.GetTargetPotion();
        
        targetPotionExists = (targetPotion != null);
        if (targetPotion != previousTargetPotion && targetPotionExists)
        {
            targetPotionPosition = targetPotion.transform.position;
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Setting Color");
            SetColor();
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

    public float GetTargetPointY()
    {
        if (targetPotionExists)
        {
            return targetPotion.transform.position.y + 
                   targetPotionLVM.FindTotalDisplayedFill();
        }
        
        Vector3 startPoint = potionFlowing.GetFlowPoint();
        Ray ray = new Ray(startPoint, Vector3.down);
        
        Physics.Raycast(ray, out RaycastHit hit,5,11 );
        return hit.point.y;

    }

    public Vector3 GetTargetPointPosition()
    {

        if (targetPotionExists)
        {
            return new Vector3(targetPotionPosition.x,targetPotionPosition.y + targetPotionLVM.FindTotalDisplayedFill(),targetPotionPosition.z);
        }

        //Vector3 startPoint = potionFlowing.GetFlowPoint();
        //return new Vector3(startPoint.x, 0, startPoint.z);
        
        Vector3 startPoint = potionFlowing.GetFlowPoint();
        Ray ray = new Ray(startPoint, Vector3.down);
        
        Physics.Raycast(ray, out RaycastHit hit,5,11 );
        return hit.point;
    }

    
    public Quaternion GetTargetPointRotation()
    {
        if (targetPotionExists)
        {
            float wobbleAmountX = targetPotionWM.WobbleAmountX;
            float wobbleAmountZ = targetPotionWM.WobbleAmountZ;
        
            //Setting the desired rotation
            float rotationZ = -90f * wobbleAmountZ * Mathf.Deg2Rad * 500;
            float rotationX = -90f * wobbleAmountX * Mathf.Deg2Rad * 500;

            if (targetPotionLVM.LiquidCount == 0)
            {
                rotationZ = 0f;
                rotationX = 0f;
            }

            return Quaternion.Euler(rotationZ,0f,rotationX);        }
        else
        {
            return new Quaternion();
        }
        
    }
    
    
    void SetHeight()
    {
        float desiredHeight;
        desiredHeight = flowPosition.y - GetTargetPointY();

        flowSystemMain.gravityModifierMultiplier = 0.25f * desiredHeight;
    }

    void SetWidth()
    {
        flowSystemTrails.widthOverTrail = (ParticleSystem.MinMaxCurve)(0.05 * potionLiquids.FindViscosityFactor());
    }
}
