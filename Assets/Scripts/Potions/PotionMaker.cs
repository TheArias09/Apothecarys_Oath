using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionMaker : MonoBehaviour
{
    [SerializeField] private List<Ingredient> ingredients;
    [Space(10)]
    [SerializeField] private List<Recipe> recipes;

    private Ingredient potion;
    private Dictionary<string, Recipe> recipeDict;

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
            float quality = Recipe.CheckPotion(recipe, potion);
            if (quality > 0) break;
        }
    }
}
