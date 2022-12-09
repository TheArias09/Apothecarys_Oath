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
    
    public float MaxWobble = 2.5f;
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

    private void Awake()
    {
        liquidVisualsManager = GetComponent<LiquidVisualsManager>();
        liquidCount = liquidVisualsManager.LiquidCount;
        previousLiquidCount = liquidCount;
        viscocityFactor = FindViscosityFactor();
        balancedMaxWobble = viscocityFactor * MaxWobble;
        balancedWobbleSpeed = viscocityFactor * balancedWobbleSpeed;
        balancedRecovery = viscocityFactor * Recovery;
    }

    private void Update()
    {
        liquidCount = liquidVisualsManager.LiquidCount;
        if (liquidCount != previousLiquidCount)
        {
            previousLiquidCount = liquidCount;
            viscocityFactor = FindViscosityFactor();
            balancedMaxWobble = viscocityFactor * MaxWobble;
            balancedWobbleSpeed = viscocityFactor * balancedWobbleSpeed;
            balancedRecovery = viscocityFactor * Recovery;
        }
        
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
                rend = GetComponent<Renderer>();
                
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
        float totalTrueFill = 0f;
        List<LiquidVisuals> liquids = liquidVisualsManager.Liquids;
       
        for (int i = 0; i < liquids.Count; i++)
        {
            returnFactor += liquids[i].viscosity * liquids[i].trueFill;
            totalTrueFill += liquids[i].trueFill;
        }
        
        returnFactor /= totalTrueFill;
        return returnFactor;
    }

}