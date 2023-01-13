using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/IngredientData", fileName = "IngredientData")]
public class IngredientData : ScriptableObject
{
    public string name;
    public Color color;
    public float fresnelPower;
    public float viscosity;
    [Space(10)]
    public Sprite symbol;
}
