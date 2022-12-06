using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientWrapper : MonoBehaviour
{
    [SerializeField] private float recipientQuantity = 0;
    [Space(10)]
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private List<IngredientState> states;

    private float quantity = 0;

    public float RecipientQuantity { get => recipientQuantity; set => recipientQuantity = value; }
    public List<Ingredient> Ingredients { get => ingredients; }
    public List<IngredientState> States { get => states; }

    public event Action OnQuantityUpdated;

    public void CallOnQuantityUpdated() => OnQuantityUpdated?.Invoke();

    public void SetTotalQty() => quantity = ingredients.Sum(ing => ing.Quantity);

    public float GetTotalQty()
    {
        SetTotalQty();
        return quantity;
    }

    public float AvgQuality() => Ingredients.Average(ing => ing.Quality);

    /// <summary>
    /// Called by the IngredientWrapper receiving another liquid.
    /// </summary>
    /// <returns>true if liquid has been added, false if there is overflow</returns>
    public bool FillWith(List<Ingredient> ingredients, float deltaQty)
    {
        if (quantity >= recipientQuantity) return false;
        else
        {
            bool no_overflow = true;

            if (quantity + deltaQty >= recipientQuantity)
            {
                quantity = recipientQuantity;
                no_overflow = false;
            }
            else quantity += deltaQty;

            foreach(var ing in ingredients) AddQuantity(ing, ing.Quantity);
            
            SetTotalQty();
            OnQuantityUpdated?.Invoke();
            return no_overflow;
        }
    }

    private void AddQuantity(Ingredient ingredient, float value)
    {
        Ingredient sameIngr = Ingredients.Find(ing => ing.Equals(ingredient));

        if (sameIngr != null) sameIngr.Quantity += value;
        else
        {
            ingredient.Quantity = value;
            Ingredients.Add(ingredient);
        }
    }

    /// <summary>
    /// Called by the IngredientWrapper pouring its content.
    /// </summary>
    /// <returns></returns>
    public List<Ingredient> Pour(float deltaQty)
    {
        SetTotalQty();
        if (deltaQty > quantity) deltaQty = quantity;
        if (quantity <= 0) return null;

        List<Ingredient> pouredIngredients = new();

        foreach (var ing in ingredients)
        {
            float removedQty = Mathf.Min(ing.Quantity, deltaQty * ing.Quantity / quantity);
            ing.Quantity -= removedQty;

            Ingredient pouredIngredient = new(ing.Name, removedQty, ing.Quality, ing.Color, ing.Cures);
            pouredIngredients.Add(pouredIngredient);
        }

        CleanEmptyIngredients();

        OnQuantityUpdated?.Invoke();
        return pouredIngredients;
    }

    private void CleanEmptyIngredients()
    {
        for(int i = Ingredients.Count - 1; i >= 0; i--)
        {
            if(ingredients[i].Quantity == 0) ingredients.RemoveAt(i);
        }
    }
}
