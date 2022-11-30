using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientWrapper : MonoBehaviour
{
    [SerializeField] private Ingredient ingredient;

    public float RecipientVolume { get; set; }
    public Ingredient Ingredient { get => ingredient; set => ingredient = value; }

    public bool FillWith(IngredientWrapper wrap, float deltaQty)
    {
        float ingrQty = ingredient.GetTotalQantity();
        if (ingrQty >= RecipientVolume) return false;
        else
        {
            ingredient.AddQuantity(wrap.Ingredient, deltaQty);
            return true;
        }
    }
}
