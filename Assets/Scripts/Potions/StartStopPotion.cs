using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(IngredientWrapper))]
public class StartStopPotion : MonoBehaviour
{
    [SerializeField] GameAction OnPour;
    [SerializeField] private float triggerQuantity;
    [SerializeField] Respawner respawner;
    //[SerializeField] GameObject potionToSpawn;

    private IngredientWrapper ingredientWrapper;
    private Vector3 initialPosition;

    private void Awake()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (ingredientWrapper.Ingredients.Count == 0) return;

        Ingredient ing = ingredientWrapper.Ingredients[0];

        if (ing.Quantity <= triggerQuantity) Trigger();
    }

    public void Trigger()
    {
        OnPour.Raise();
    }

    public void ReplaceWith(GameObject potionToSpawn)
    {
        if (potionToSpawn != null)
        {
            Instantiate(potionToSpawn, initialPosition, Quaternion.identity, transform.parent);
        }

        DestroyImmediate(gameObject);
    }
}
