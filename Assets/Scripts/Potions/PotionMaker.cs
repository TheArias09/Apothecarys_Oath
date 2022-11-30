using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionMaker : MonoBehaviour
{
    [SerializeField] private float minQuality = 0.1f;
    [Space(10)]
    [SerializeField] private List<Recipe> recipes;

    private Ingredient potion;

    public static PotionMaker Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void CheckPotion(IngredientWrapper potion)
    {
        foreach(var recipe in recipes)
        {
            float quality = CheckPotion(recipe, potion);
            if (quality > 0) break;
        }
    }


    /// <summary>
    /// Checks if a potion followed a recipe correctly and returns a bool accordingly.
    /// </summary>
    public float CheckPotion(Recipe recipe, IngredientWrapper potion)
    {
        //Wrong States
        if (recipe.States.Count != potion.States.Count || !recipe.States.All(potion.States.Contains)) return 0;

        //Wrong ingredients
        if (recipe.Ingredients.Count != potion.Ingredients.Count || !recipe.Ingredients.All(potion.Ingredients.Contains)) return 0;

        //Potion corresponds to recipe
        Debug.Log(recipe.Name + " crafted !");

        Ingredient result = new(recipe.Name, potion.GetTotalQty(), potion.AvgQuality(), recipe.Color, recipe.Cures);
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

        float avgProportion = proportions.Average();
        double squarDiff = proportions.Select(val => Math.Pow(val - avgProportion, 2)).Sum();
        float std_dev = (float)Math.Sqrt(squarDiff / proportions.Length);

        float quality = std_dev > 1 ? Instance.minQuality : 1 - std_dev;

        Debug.Log("std_dev: " + std_dev + " => quality: " + quality);
        return quality;
    }
}
