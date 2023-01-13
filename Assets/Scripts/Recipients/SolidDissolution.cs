using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SolidDissolution : MonoBehaviour
{
    [SerializeField] List<Ingredient> ingredients;
    [SerializeField] string collisionTag = "Bottleneck";

    [SerializeField] UnityEvent OnDissolution;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(collisionTag))
        {
            var targetIngredientWrapper = other.GetComponentInParent<IngredientWrapper>();
            var totalQuantity = ingredients.Sum(ing => ing.Quantity);
            targetIngredientWrapper.FillWith(ingredients, totalQuantity);

            OnDissolution?.Invoke();
            Destroy(gameObject);
        }
    }
}
