using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidVisuals : MonoBehaviour
{
    public float trueFill; //Entre 0 et 1
    public float displayedFill; //Entre -0.1 et 0.1
    public Color liquidColor;
    public Color surfaceColor;
    public float fresnelPower;
    public Color fresnelColor;
    public float viscosity; //Water = 1, Honey < 1, Gaz > 1
    public bool isTextured;
    public float noisePower;
    public Color noiseColor;

    public LiquidVisuals(float TrueFill,float DisplayedFill,Color LiquidColor,Color SurfaceColor, 
        float FresnelPower,Color FresnelColor, float Viscosity,
        bool IsTextured, float NoisePower, Color NoiseColor)
    {
        trueFill = TrueFill;
        displayedFill = DisplayedFill;
        liquidColor = LiquidColor;
        surfaceColor = SurfaceColor;
        fresnelPower = FresnelPower;
        fresnelColor = FresnelColor;
        viscosity = Viscosity;
        isTextured = IsTextured;
        noisePower = NoisePower;
        noiseColor = NoiseColor;
    }
}
