using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionMaker : MonoBehaviour
{
    [SerializeField] private float minQuality = 0.1f;
    [SerializeField] private float deviationMultiplier = 1f;
    [SerializeField] private RecipeBook recipeBook;

    public static PotionMaker Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Checks if an IngredientWrapper contains the ingredients corresponding to a recipe.
    /// </summary>
    /// <param name="potion">The IngredientWrapper to check.</param>
    /// <returns>True if a recipe was crafted, false if nothing happened.</returns>
    public bool CheckPotion(IngredientWrapper potion)
    {
        foreach(var recipeData in recipeBook.recipes)
        {
            float quality = CheckPotion(recipeData.recipe, potion);
            if (quality > 0) return true;
        }

        Debug.Log("No recipe found, checking for dubious");

        foreach(var flexRecipe in recipeBook.flexibleRecipes)
        {
            if (CheckFlexible(flexRecipe, potion)) return true;
        }

        Debug.Log("Not dubious");
        return false;
    }


    /// <summary>
    /// Checks if a potion followed a recipe correctly and returns a float accordingly.
    /// </summary>
    /// <returns>The quality of the crafted potion in case of success, 0 in other case.</returns>
    private float CheckPotion(Recipe recipe, IngredientWrapper potion)
    {
        //Wrong ingredients
        if (recipe.Ingredients.Count != potion.Ingredients.Count || !recipe.Ingredients.All(potion.Ingredients.Contains)) return 0;

        //Potion corresponds to recipe
        Debug.Log(recipe.Name + " crafted !");

        Ingredient result = new(recipe.Result, potion.GetTotalQty(), potion.AvgQuality(), recipe.Cures);
        float craftQuality = CheckProportions(recipe, potion);
        result.Quality *= craftQuality;

        potion.Ingredients.Clear();
        potion.Ingredients.Add(result);

        Debug.Log("Final quality: " + result.Quality);
        return result.Quality;
    }

    private float CheckProportions(Recipe recipe, IngredientWrapper potion)
    {
        float[] proportions = new float[potion.Ingredients.Count];

        for (int i = 0; i < potion.Ingredients.Count; i++)
        {
            Ingredient equivalent = recipe.Ingredients.Find(ing => ing.Name.Equals(potion.Ingredients[i].Name));
            proportions[i] = potion.Ingredients[i].Quantity / equivalent.Quantity;
        }

        float minProportion = proportions.Min();
        proportions = proportions.Select(x => x / minProportion).ToArray();

        float avgProportion = proportions.Average();
        float deviation = proportions.Select(val => Math.Abs(val - avgProportion)).Sum();
        deviation *= deviationMultiplier / proportions.Length;

        float quality = deviation > 1 ? Instance.minQuality : 1 - deviation;

        Debug.Log("deviation : " + deviation + " => quality: " + quality);
        return quality;
    }

    private bool CheckFlexible(FlexibleRecipe recipe, IngredientWrapper potion)
    {
        bool success = potion.Ingredients.Count >= recipe.minIngredientCount &&
                      recipe.ingredients.All(potion.Ingredients.Select(i => i.Data).Contains) &&
                      !potion.Ingredients.Select(i => i.Data).Any(recipe.nonIngredients.Contains);

         if (success)
        {
            Ingredient result = new(recipe.result, potion.GetTotalQty(), 0, DiseaseName.NONE);
            potion.Ingredients.Clear();
            potion.Ingredients.Add(result);
            Debug.Log("Dubious potion crafted...");
        }

        return success;
    }
}
