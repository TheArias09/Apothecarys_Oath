using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    [SerializeField] private string name;
    [SerializeField] private DiseaseName cures;
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private List<IngredientState> states;

    public string Name { get { return name; } }
    public DiseaseName Cures { get { return cures; } }
    public List<Ingredient> Ingredients { get { return ingredients; } }
    public List<IngredientState> States { get { return states; } }

    public Recipe(string name, List<Ingredient> ingredients, List<IngredientState> states)
    {
        this.name = name;
        this.ingredients = ingredients;
        this.states = states;
    }
}
