using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(IngredientWrapper))]
public class StartStopPotion : MonoBehaviour
{
    [SerializeField] GameAction OnPour;

    [SerializeField] private float triggerQuantity;

    private IngredientWrapper ingredientWrapper;

    [SerializeField] Respawner respawner;

    private Vector3 initialPosition;

    [SerializeField] GameObject potionToSpawn;

    private void Awake()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (ingredientWrapper.Ingredients.Count == 0) return;

        Ingredient ing = ingredientWrapper.Ingredients[0];

        if (ing.Quantity <= triggerQuantity)
        {
            if(potionToSpawn != null)
            {
                Instantiate(potionToSpawn, initialPosition, Quaternion.identity, transform.parent);
            }

            Debug.Log("Game potion triggered");

            OnPour.Raise();

            Destroy(gameObject);
        }
    }
}
