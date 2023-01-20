using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientWrapper))]
public class StartStopPotion : MonoBehaviour
{
    [SerializeField] private bool start;
    [SerializeField] private float triggerQuantity;
    [SerializeField] private GameObject uiText;

    private IngredientWrapper ingredientWrapper;

    private void Start()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
    }

    void Update()
    {
        Ingredient ing = ingredientWrapper.Ingredients[0];

        if (ing.Quantity <= triggerQuantity)
        {
            Debug.Log("Game potion triggered");

            if (start) GameManager.Instance.StartGame();
            else GameManager.Instance.QuitGame();

            uiText.SetActive(false);
            Destroy(gameObject);
        }
    }
}
