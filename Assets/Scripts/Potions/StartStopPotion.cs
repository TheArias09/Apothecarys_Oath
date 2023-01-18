using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientWrapper))]
public class StartStopPotion : MonoBehaviour
{
    [SerializeField] private float triggerQuantity;
    [Space(10)]
    [SerializeField] private IngredientData startIngredient;
    [SerializeField] private IngredientData restartIngredient;
    [SerializeField] private IngredientData quitIngredient;

    private IngredientWrapper ingredientWrapper;

    private void Start()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
    }

    void Update()
    {
        if (ingredientWrapper.Ingredients.Count < 1) return;

        Ingredient ing = ingredientWrapper.Ingredients[0];

        if (ing.Quantity >= triggerQuantity)
        {
            Debug.Log("Game potion triggered");

            if (ing.Data == startIngredient) GameManager.Instance.StartGame();
            else if (ing.Data == restartIngredient) GameManager.Instance.Restart();
            else if (ing.Data == quitIngredient) GameManager.Instance.GameOver();

            ingredientWrapper.Empty();
        }
    }
}
