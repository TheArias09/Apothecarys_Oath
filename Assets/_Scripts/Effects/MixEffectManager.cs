using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixEffectManager : MonoBehaviour
{
    private GameObject potion;
    private LiquidVisualsManager liquidVisualsManager;
    private WobbleManager wobbleManager;

    private ParticleSystem particles;
    private ParticleSystem.MainModule particlesMain;

    private bool startedPlaying;
    
    public bool takesColor;
    public float effectBaseScale;
    private float splashBaseScale = 0.003f;

    private float wobbleAmountX;
    private float wobbleAmountZ;
    private float rotationZ;
    private float rotationX;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particlesMain = particles.main;

        startedPlaying = false;

        potion = transform.parent.gameObject;
        liquidVisualsManager = potion.GetComponent<LiquidVisualsManager>();
        wobbleManager = potion.GetComponent<WobbleManager>();
        
        SetScale();

        if (takesColor)
        {
            SetColor();
        }
        
    }

    void Update()
    {
        if (!startedPlaying && particles.isPlaying) //Checks if it starts
        {
            startedPlaying = true;
        }

        if (startedPlaying && !particles.isPlaying) //Has started and ended
        {
            Destroy(gameObject); //We destroy the effect
        }
        
        SetTransform();
    }

    void SetTransform()
    {
        transform.position = potion.transform.position; //+ new Vector3(0,liquidVisualsManager.FindTotalDisplayedFill() * potion.transform.lossyScale.y / liquidVisualsManager.BaseScale,0)
        
        wobbleAmountX = wobbleManager.WobbleAmountX;
        wobbleAmountZ = wobbleManager.WobbleAmountZ;
        
        //Setting the desired rotation
        rotationZ = -90f * wobbleAmountZ * Mathf.Deg2Rad * 500;
        rotationX = -90f * wobbleAmountX * Mathf.Deg2Rad * 500;

        transform.rotation = Quaternion.Euler(rotationZ,0f,rotationX);        
    }

    void SetScale()
    {
        //transform.localScale = liquidVisualsManager.FindInsideScale() * Vector3.one;
        transform.localScale = Vector3.one * (liquidVisualsManager.FindInsideScale() * effectBaseScale) / splashBaseScale;
    }
    
    void SetColor()
    {
        Color liquidColor = liquidVisualsManager.Liquids[^1].liquidColor;
        Color effectColor = new Color(
            Mathf.Min(1f, liquidColor.r - 0.20f),
            Mathf.Min(1f, liquidColor.g - 0.20f),
            Mathf.Min(1f, liquidColor.b - 0.20f)
        );
        particlesMain.startColor = new ParticleSystem.MinMaxGradient( effectColor );
    }
}
