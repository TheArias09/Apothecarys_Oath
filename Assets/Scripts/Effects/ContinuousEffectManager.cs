using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to add on Effects GameObjects
public class ContinuousEffectManager : MonoBehaviour
{
    private GameObject potion;
    private LiquidVisualsManager liquidVisualsManager;
    private WobbleManager wobbleManager;
    
    private ParticleSystem particles;

    private float wobbleAmountX;
    private float wobbleAmountZ;
    private float rotationZ;
    private float rotationX;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();

        potion = transform.parent.gameObject;
        liquidVisualsManager = potion.GetComponent<LiquidVisualsManager>();
        wobbleManager = potion.GetComponent<WobbleManager>();
        SetScale();
    }

    void Update()
    {
        SetTransform();
    }

    void SetTransform()
    {
        transform.position = potion.transform.position + new Vector3(0,liquidVisualsManager.FindTotalDisplayedFill() * potion.transform.lossyScale.y / liquidVisualsManager.BaseScale,0);
        
        wobbleAmountX = wobbleManager.WobbleAmountX;
        wobbleAmountZ = wobbleManager.WobbleAmountZ;
        
        //Setting the desired rotation
        rotationZ = -90f * wobbleAmountZ * Mathf.Deg2Rad * 500;
        rotationX = -90f * wobbleAmountX * Mathf.Deg2Rad * 500;

        transform.rotation = Quaternion.Euler(rotationZ,0f,rotationX);        
    }

    void SetScale()
    {
        transform.localScale = liquidVisualsManager.FindInsideScale() * Vector3.one;
    }
}
