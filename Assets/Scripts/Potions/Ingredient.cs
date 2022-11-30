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
    
    [SerializeField] private Color color;

    [SerializeField, Range(0,1)] private float quality;
    [SerializeField] private List<IngredientState> states;

    //Name of the ingredient can be null.
    public string Name { get => name; set => name = value; }
    public float Quantity { get => quantity; set => quantity = value; }
    public float Quality { get => quality; set => quality = value; }
    public Color Color { get => color; set => color = value; }
    public List<IngredientState> States { get => states; }
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

    public void SetTotalQantity() => quantity = Ingredients.Sum(ing => ing.quantity);

    public float GetTotalQantity()
    {
        if (Ingredients.Count > 0) SetTotalQantity();
        return quantity;
    }

    public void SetAvgQuality() => quality = Ingredients.Average(ing => ing.Quality);

    public void AddState(IngredientState state) => States.Add(state);

    public void RemoveIngredients() => Ingredients.Clear();


    public void AddQuantity(Ingredient ingredient, float value)
    {
        if (name == ingredient.name) quantity += value;
        else
        {
            Ingredient ing = Ingredients.Find(ing => ing.Equals(ingredient));
            if (ing != null) ing.quantity += value;
            else
            {
                ingredient.quantity = value;
                AddIngredient(ingredient);
            }
        }
    }

    /// <summary>
    /// Adds a new Ingredient to the composition. This removes the MIXED state of the ingredient.
    /// </summary>
    private void AddIngredient(Ingredient ingredient)
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