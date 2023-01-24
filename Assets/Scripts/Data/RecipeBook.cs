using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/RecipeBook", fileName = "RecipeBook")]
public class RecipeBook : ScriptableObject
{
    public List<RecipeData> recipes = new();
    public List<FlexibleRecipe> flexibleRecipes = new();
}
