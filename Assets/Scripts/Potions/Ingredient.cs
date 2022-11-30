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
    [SerializeField, Range(0, 1)] private float quality;
    [SerializeField] private Color color;
    [SerializeField] private DiseaseName? cures;

    [SerializeField] private List<IngredientState> states;

    //Name of the ingredient can be null.
    public string Name { get => name; set => name = value; }
    public float Quantity { get => quantity; set => quantity = value; }
    public float Quality { get => quality; set => quality = value; }
    public Color Color { get => color; set => color = value; }
    public List<IngredientState> States { get => states; }

    public DiseaseName? Cures { get => cures; set => cures = value; }

    public Ingredient(string name, float quantity, float quality, Color color, DiseaseName disease)
    {
        this.name = name;
        this.quantity = quantity;
        this.quality = quality;
        this.color = color;
        cures = disease;

        states = new List<IngredientState>();
    }

    public void AddState(IngredientState state) => States.Add(state);

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