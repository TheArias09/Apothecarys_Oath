using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    [SerializeField] private IngredientData result;
    [SerializeField] private DiseaseName cures;
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private List<IngredientState> states;
    [SerializeField] private List<String> recipeInstructions;

    public IngredientData Result { get => result; }
    public string Name { get => result.ingredientName; }
    public Color Color { get => result.color; }
    public DiseaseName Cures { get => cures; }
    public List<Ingredient> Ingredients { get => ingredients; }
    public List<IngredientState> States { get => states; }
    public List<String> RecipeInstructions { get => recipeInstructions; }
}
