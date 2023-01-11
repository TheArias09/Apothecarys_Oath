using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(LiquidVisualsManager))]

public class WobbleManager : MonoBehaviour
{
    private Renderer rend;
    private Vector3 lastPos;
    private Vector3 velocity;
    private Vector3 lastRot;  
    private Vector3 angularVelocity;
    
    public float MaxWobble = 1f;
    private float balancedMaxWobble;
    public float WobbleSpeed = 1f;
    private float balancedWobbleSpeed;
    public float Recovery = 1f;
    private float balancedRecovery;
    private float viscocityFactor;
    
    private float wobbleAmountX;
    private float wobbleAmountZ;
    private float wobbleAmountToAddX;
    private float wobbleAmountToAddZ;
    private float pulse;
    private float time = 0.5f;

    private LiquidVisualsManager liquidVisualsManager;
    private int liquidCount;
    private int previousLiquidCount;
    private float totalTrueFill;
    private float previousTotalTrueFill;

    public float WobbleAmountX => wobbleAmountX;
    public float WobbleAmountZ => wobbleAmountZ;

    private void Awake()
    {
        liquidVisualsManager = GetComponent<LiquidVisualsManager>();
        liquidCount = liquidVisualsManager.LiquidCount;
        previousLiquidCount = liquidCount;
        totalTrueFill = liquidVisualsManager.FindTotalTrueFill();
        previousTotalTrueFill = totalTrueFill;
        viscocityFactor = FindViscosityFactor();
        balancedMaxWobble = viscocityFactor * MaxWobble;
        balancedWobbleSpeed = viscocityFactor * WobbleSpeed;
        balancedRecovery = viscocityFactor * Recovery;
    }

    private void Update()
    {
        liquidCount = liquidVisualsManager.LiquidCount;
        totalTrueFill = liquidVisualsManager.FindTotalTrueFill();
        if (liquidCount != previousLiquidCount || Math.Abs(totalTrueFill - previousTotalTrueFill) > 0.005f)
        {
            previousLiquidCount = liquidCount;
            viscocityFactor = FindViscosityFactor();
            balancedMaxWobble = viscocityFactor * MaxWobble;
            balancedWobbleSpeed = viscocityFactor * WobbleSpeed;
            balancedRecovery = viscocityFactor * Recovery;
        }

        //balancedMaxWobble = MaxWobble;
        //balancedWobbleSpeed = WobbleSpeed;
        balancedRecovery = Recovery;
        
        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (balancedRecovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (balancedRecovery));

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * balancedWobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);
        
        foreach(Transform child in transform)
        {
            if (child.gameObject.CompareTag("LiquidVolume"))
            {
                //TODO optimiser la mémoire en ayant une liste de renderer qu'on update
                rend = child.GetComponent<Renderer>();
                
                // send it to the shader
                rend.material.SetFloat("_WobbleX", wobbleAmountX);
                rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

                // velocity
                velocity = (lastPos - transform.position) / Time.deltaTime;
                angularVelocity = transform.rotation.eulerAngles - lastRot;


                // add clamped velocity to wobble
                wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * balancedMaxWobble, -balancedMaxWobble, balancedMaxWobble);
                wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * balancedMaxWobble, -balancedMaxWobble, balancedMaxWobble);

                // keep last position
                lastPos = transform.position;
                lastRot = transform.rotation.eulerAngles;

            }
        }
    }
    
    float FindViscosityFactor()
    {
        float returnFactor = 0f; 
        List<LiquidVisuals> liquids = liquidVisualsManager.Liquids;
       
        for (int i = 0; i < liquids.Count; i++)
        {
            returnFactor += liquids[i].viscosity * liquids[i].trueFill;
        }
        
        returnFactor /= liquidVisualsManager.FindTotalTrueFill();
        return returnFactor;
    }

}


/* Base Wobble Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wobble : MonoBehaviour
{
    Renderer rend;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;  
    Vector3 angularVelocity;
    public float MaxWobble = 3f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;
    
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));
        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);
        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);
        // velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;
        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }
}

*/