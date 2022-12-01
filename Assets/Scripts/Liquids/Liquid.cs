using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float trueFill; //Entre 0 et 1
    public float displayedFill; //Entre -0.1 et 0.1
    public Color liquidColor;
    public Color surfaceColor;
    public float fresnelPower;
    public Color fresnelColor;

    public Liquid(float TrueFill,float DisplayedFill,Color LiquidColor,Color SurfaceColor,float FresnelPower,Color FresnelColor)
    {
        trueFill = TrueFill;
        displayedFill = DisplayedFill;
        liquidColor = LiquidColor;
        surfaceColor = SurfaceColor;
        fresnelPower = FresnelPower;
        fresnelColor = FresnelColor;
    }
}
