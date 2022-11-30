using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientWrapper : MonoBehaviour
{
    [SerializeField] private Ingredient ingredient;

    public Ingredient Ingredient { get => ingredient; set => ingredient = value; }


}
