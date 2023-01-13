using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SolidDissolution : MonoBehaviour
{
    [SerializeField] List<Ingredient> ingredients;
    [SerializeField] int layer;

    [SerializeField] UnityEvent OnDissolution;

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Triggered");
        if (other.gameObject.CompareTag("Bottleneck"))
        {
            Debug.LogWarning("Layered");
            var targetIngredientWrapper = other.GetComponentInParent<IngredientWrapper>();
            var totalQuantity = ingredients.Sum(ing => ing.Quantity);
            targetIngredientWrapper.FillWith(ingredients, totalQuantity);
            OnDissolution?.Invoke();
            Destroy(gameObject);
        }
    }
}
