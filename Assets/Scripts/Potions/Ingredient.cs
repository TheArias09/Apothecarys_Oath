using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Ingredient of a potion. Can be itself composed of ingredients, and have multiple states.
/// An ingredient does not have any state when created.
/// </summary>
[Serializable]
public class Ingredient
{
    [SerializeField] private string name;
    [SerializeField] private float quantity;
    [SerializeField, Range(0,1)] private float quality;
    [SerializeField] private List<IngredientState> states;

    //Name of the ingredient can be null.
    public string Name { get { return name; } }
    public float Quantity { get { return quantity; } }
    public float Quality { get { return quality; } }
    public List<IngredientState> States { get { return states; } }
    public List<Ingredient> Ingredients { get; }

    public DiseaseName? Cures { get; set; }

    public Ingredient(string name, float quantity)
    {
        this.name = name;
        this.quantity = quantity;
        quality = 1;
        states = new List<IngredientState>();
        Ingredients = new List<Ingredient>();
    }

    /// <summary>
    /// This constructor should only be used to list an ingredient as part of a recipe.
    /// </summary>
    public Ingredient(string name, float quantity, List<IngredientState> states) : this(name, quantity)
    {
        this.states = states;
    }

    public Ingredient(string name, float quantity, List<Ingredient> ingredients) : this(name, quantity)
    {
        Ingredients = ingredients;
    }

    public Ingredient(float quantity, List<Ingredient> ingredients) : this(null, quantity, ingredients) { }


    public void SetName(string name) => this.name = name;

    public void SetQuantity(float value) => quantity = value;

    public void SetTotalQantity() => quantity = Ingredients.Sum(ing => ing.quantity);

    public void SetQuality(float value) => quality = value;

    public void SetAvgQuality() => quality = Ingredients.Average(ing => ing.Quality);

    public void AddState(IngredientState state) => States.Add(state);

    public void RemoveState(IngredientState state) => States.Remove(state);

    public void RemoveIngredients() => Ingredients.Clear();


    /// <summary>
    /// Adds a new Ingredient to the composition. This removes the MIXED state of the ingredient.
    /// </summary>
    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
        States.Remove(IngredientState.MIXED);
    }

    public override string ToString()
    {
        StringBuilder result = new (Name + ", Qty= " + Quantity + ", Qual= " + Quality + ", States: ");
        if (States.Count > 0) result.Append(string.Join(", ", States));
        else result.Append("NONE");
        return result.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(Ingredient)) return false;

        Ingredient other = obj as Ingredient;
        if (other.Name != Name) return false;
        if (other.states.Count != States.Count || !other.states.All(States.Contains)) return false;

        return true;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

[Serializable]
public enum IngredientState
{
    MIXED, CUT, CRUSHED
}