using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientWrapper))]
public class StartStopPotion : MonoBehaviour
{
    [Tooltip("If true, this potion will start the game. If false, it will end it.")]
    [SerializeField] private bool startGame;
    [SerializeField] private float triggerQuantity;
    [SerializeField] private string triggerLiquidName;

    private IngredientWrapper ingredientWrapper;

    private void Start()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
    }

    void Update()
    {
        if (ingredientWrapper.Ingredients.Count < 1) return;

        Ingredient ing = ingredientWrapper.Ingredients[0];
        if (ing.Name == triggerLiquidName && ing.Quantity >= triggerQuantity)
        {
            GameManager.Instance.ChangeGameStatus(startGame);
        }
    }
}
