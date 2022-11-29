using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public float fill;
    public Color liquidColor;
    public Color surfaceColor;
    public float fresnelPower;
    public Color fresnelColor;

    public Liquid(float Fill,Color LiquidColor,Color SurfaceColor,float FresnelPower,Color FresnelColor)
    {
        fill = Fill;
        liquidColor = LiquidColor;
        surfaceColor = SurfaceColor;
        fresnelPower = FresnelPower;
        fresnelColor = FresnelColor;
    }
}
