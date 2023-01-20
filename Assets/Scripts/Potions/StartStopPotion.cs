using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientWrapper))]
public class StartStopPotion : MonoBehaviour
{
    [SerializeField] private bool start;
    [SerializeField] private bool stop;
    [SerializeField] private bool restart;
    [Space(20)]
    [SerializeField] private float triggerQuantity;

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
            else if (restart) GameManager.Instance.Restart();
            else if (stop) GameManager.Instance.QuitGame();

            Destroy(gameObject);
        }
    }
}
