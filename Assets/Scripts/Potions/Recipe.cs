using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    [SerializeField] private string name;
    [SerializeField] private Color color;
    [SerializeField] private DiseaseName cures;
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private List<IngredientState> states;

    public string Name { get => name; }
    public Color Color { get => color; }
    public DiseaseName Cures { get => cures; }
    public List<Ingredient> Ingredients { get => ingredients; }
    public List<IngredientState> States { get => states; }

    public Recipe(string name, List<Ingredient> ingredients, List<IngredientState> states)
    {
        this.name = name;
        this.ingredients = ingredients;
        this.states = states;
    }
}
