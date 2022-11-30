using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionMaker : MonoBehaviour
{
    [SerializeField] private float minQuality = 0.1f;
    [Space(10)]
    [SerializeField] private List<Ingredient> ingredients;
    [Space(10)]
    [SerializeField] private List<Recipe> recipes;

    private Ingredient potion;
    private Dictionary<string, Recipe> recipeDict;

    public static PotionMaker Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        recipeDict = new Dictionary<string, Recipe>();
        foreach (var recipe in recipes) recipeDict.Add(recipe.Name, recipe);
    }

    void UpdateIngredients()
    {
        potion = new(1, ingredients.ToList());
        potion.AddState(IngredientState.MIXED);
    }

    public void UpdatePotion()
    {
        UpdateIngredients();
        CheckPotion(potion);
    }

    public void CheckPotion(Ingredient potion)
    {
        UpdateIngredients();
        foreach(var recipe in recipes)
        {
            float quality = CheckPotion(recipe, potion);
            if (quality > 0) break;
        }
    }


    /// <summary>
    /// Checks if a potion followed a recipe correctly and returns a bool accordingly.
    /// </summary>
    public float CheckPotion(Recipe recipe, Ingredient potion)
    {
        //Wrong States
        if (recipe.States.Count != potion.States.Count || !recipe.States.All(potion.States.Contains)) return 0;

        //Wrong ingredients
        if (recipe.Ingredients.Count != potion.Ingredients.Count || !recipe.Ingredients.All(potion.Ingredients.Contains)) return 0;

        //Potion corresponds to recipe
        Debug.Log(recipe.Name + " crafted !");
        potion.Name = recipe.Name;
        potion.SetTotalQantity();
        potion.SetAvgQuality();
        potion.Color = recipe.Color;
        potion.Cures = recipe.Cures;
        potion.RemoveIngredients();

        CheckProportions(recipe, potion);
        return potion.Quality;
    }

    private void CheckProportions(Recipe recipe, Ingredient potion)
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
        potion.Quality *= quality;

        Debug.Log("std_dev: " + std_dev + " => quality: " + quality);
        Debug.Log("Final quality: " + potion.Quality);
    }
}
