using Recipients;
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
    [SerializeField] Flowing serializedPotionToSpawn;

    private IngredientWrapper ingredientWrapper;

    [SerializeField] List<Ingredient> ingredients;

    private Vector3 initialPosition;

    private bool hasAlreadyTriggered = false;

    private bool isReadyToTrigger = false;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        isReadyToTrigger = true;
    }

    private void Awake()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (hasAlreadyTriggered) return;
        if (!isReadyToTrigger) return;

        if (ingredientWrapper.Ingredients.Count == 0) return;

        Ingredient ing = ingredientWrapper.Ingredients[0];

        if (ing.Quantity <= triggerQuantity) Trigger();
    }

    public void Trigger()
    {
        OnPour.Raise();
        hasAlreadyTriggered = true;
    }

    public void ReplaceWith(GameObject potionToSpawn)
    {
        if (potionToSpawn != null)
        {
            Instantiate(potionToSpawn, initialPosition, Quaternion.identity, transform.parent);
        }

        Destroy(gameObject);
    }

    public void ReplaceWithSafe(GameObject potionToSpawn)
    {
        var instance = Instantiate(potionToSpawn, initialPosition, Quaternion.identity, transform.parent);
        var wrapper = instance.GetComponent<IngredientWrapper>();
        wrapper.Empty();
        wrapper.Ingredients = ingredients;
        wrapper.CallOnQuantityUpdated();

        Destroy(gameObject);
    }
}
