using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientWrapper : MonoBehaviour
{
    [SerializeField] private List<Ingredient> ingredients;
    [SerializeField] private List<IngredientState> states;

    private float recipientVolume = 0;
    private float quantity = 0;

    public List<Ingredient> Ingredients { get => ingredients; }
    public List<IngredientState> States { get => states; }

    public event Action OnQuantityUpdated;

    public void SetRecipient(LiquidManager liquidManager) => recipientVolume = liquidManager.RecipientQuantity;

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
    public bool FillWith(IngredientWrapper wrap, float deltaQty)
    {
        if (quantity >= recipientVolume) return false;
        else
        {
            bool no_overflow = true;

            if (quantity + deltaQty >= recipientVolume)
            {
                deltaQty = recipientVolume - quantity;
                quantity = recipientVolume;
                no_overflow = false;
            }
            else quantity += deltaQty;

            foreach(var ingredient in wrap.Ingredients) AddQuantity(ingredient, deltaQty);
            
            SetTotalQty();
            return no_overflow;
        }

        OnQuantityUpdated?.Invoke();
    }

    /// <summary>
    /// Called by the IngredientWrapper pouring its content.
    /// </summary>
    /// <returns></returns>
    public float Pour(IngredientWrapper wrap, float deltaQty)
    {
        SetTotalQty();
        float totalPoured = 0;

        foreach (var ingredient in wrap.Ingredients)
        {
            float removedQty = Mathf.Max(ingredient.Quantity, deltaQty * ingredient.Quantity / quantity);
            ingredient.Quantity -= removedQty;
            totalPoured += removedQty;
        }

        return totalPoured;
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
}
