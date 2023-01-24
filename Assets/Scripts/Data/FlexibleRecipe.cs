using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/FlexibleRecipe", fileName = "FlexibleRecipe")]
public class FlexibleRecipe : ScriptableObject
{
    public IngredientData result;
    public int minIngredientCount;
    [Space(10)]
    public List<IngredientData> ingredients = new();
    public List<IngredientData> nonIngredients = new();

    // A flexible recipe doesn't need exact quantities, and the potion can contain other ingredients
    // as long as they are not part of the nonIngredients list to be validated.
}
