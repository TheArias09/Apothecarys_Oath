using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    [SerializeField] private static float minQuality = 0.1f;

    [SerializeField] private string name;
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private List<IngredientState> states;

    public string Name { get { return name; } }
    public List<Ingredient> Ingredients { get { return ingredients; } }
    public List<IngredientState> States { get { return states; } }

    public Recipe(string name, List<Ingredient> ingredients, List<IngredientState> states)
    {
        this.name = name;
        this.ingredients = ingredients;
        this.states = states;
    }

    /// <summary>
    /// Checks if a potion followed a recipe correctly and returns a bool accordingly.
    /// </summary>
    public static float CheckPotion(Recipe recipe, Ingredient potion)
    {
        //Wrong States
        if (recipe.states.Count != potion.States.Count || !recipe.states.All(potion.States.Contains)) return 0;

        //Wrong ingredients
        if (recipe.Ingredients.Count != potion.Ingredients.Count || !recipe.Ingredients.All(potion.Ingredients.Contains)) return 0;
        
        //Potion corresponds to recipe
        Debug.Log(recipe.name + " crafted !");
        potion.SetName(recipe.Name);
        potion.SetTotalQantity();
        potion.SetAvgQuality();

        CheckProportions(recipe, potion);
        return potion.Quality;
    }

    private static void CheckProportions(Recipe recipe, Ingredient potion)
    {
        float[] proportions = new float[potion.Ingredients.Count];

        for (int i = 0; i < potion.Ingredients.Count; i++)
        {
            Ingredient equivalent = recipe.Ingredients.Find(ing => ing.Name.Equals(potion.Ingredients[i].Name));
            proportions[i] = potion.Ingredients[i].Quantity / equivalent.Quantity;
        }

        float avgProportion = proportions.Average();
        double squarDiff = proportions.Select(val => Math.Pow(val - avgProportion, 2)).Sum();
        float std_dev = (float) Math.Sqrt(squarDiff / proportions.Length);

        float quality = std_dev > 1 ? minQuality : 1 - std_dev;
        potion.SetQuality(potion.Quality * quality);

        Debug.Log("std_dev: " + std_dev + " => quality: " + quality);
        Debug.Log("Final quality: " + potion.Quality);
    }
}
