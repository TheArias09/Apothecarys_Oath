using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEngine;

public class IngredientWrapper : MonoBehaviour
{
    [SerializeField] private float recipientQuantity = 1;
    [SerializeField] private float minQuantity = 0.01f;
    [SerializeField] private bool infiniteSource;
    [Space(10)]
    [SerializeField] private List<Ingredient> ingredients;

    [SerializeField] private Respawner respawner;
    public Respawner Respawner => respawner;

    public bool Mixed { get; set; } = true;

    private float quantity = 0;

    public float RecipientQuantity { get => recipientQuantity; set => recipientQuantity = value; }
    public List<Ingredient> Ingredients { get => ingredients; }

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
        if (infiniteSource) return false;
        Mixed = false;

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
    /// <returns>The list of poured ingredients.</returns>
    public List<Ingredient> Pour(float deltaQty)
    {
        SetTotalQty();
        if (quantity <= 0) return null;

        List<Ingredient> pouredIngredients = new();
        float removedQty = 0;

        if (deltaQty > quantity) deltaQty = quantity;
        
        //Pour ingredients one by one until removedQty corresponds to poured quantity
        for (int i= ingredients.Count -1; i >= 0 && removedQty < deltaQty; i--)
        {
            Ingredient ing = ingredients[i];
            removedQty += Mathf.Min(ing.Quantity, deltaQty);

            var quantityActuallyRemoved = removedQty * ing.Data.viscosity;
            if (!infiniteSource) ing.Quantity -= quantityActuallyRemoved;
            pouredIngredients.Add(new(ing.Data, quantityActuallyRemoved, ing.Quality, ing.Cures));
        }

        ingredients.RemoveAll(ing => ing.Quantity <= minQuantity);

        OnQuantityUpdated?.Invoke();
        return pouredIngredients;
    }
    
    //Calcule le facteur de viscosité dans le IngredientWrapper
    public float FindViscosityFactor()
    {
        float returnFactor = 0f;
        float totalTrueFill = 0f;

        for (int i = 0; i < Ingredients.Count; i++)
        {
            returnFactor += Ingredients[i].Data.viscosity * Ingredients[i].Quantity;
            totalTrueFill += Ingredients[i].Quantity;
        }
        
        returnFactor /= totalTrueFill;
        return returnFactor;
    }

    public void Empty()
    {
        Ingredients.Clear();
        quantity = 0;
        OnQuantityUpdated?.Invoke();
    }
}
