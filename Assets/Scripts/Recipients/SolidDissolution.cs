using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SolidDissolution : MonoBehaviour
{
    [SerializeField] List<Ingredient> ingredients;
    [SerializeField] string collisionTag = "Bottleneck";

    [SerializeField] UnityEvent OnDissolutionStart;
    [SerializeField] UnityEvent OnDissolutionEnd;

    [SerializeField] float pourSpeed = 2f;

    private bool hasAlreadyBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasAlreadyBeenTriggered) return;

        if (other.gameObject.CompareTag(collisionTag))
        {
            StartCoroutine(Dissolve(other));
        }
    }

    private IEnumerator Dissolve(Collider other)
    {
        hasAlreadyBeenTriggered = true;

        var targetIngredientWrapper = other.GetComponentInParent<IngredientWrapper>();
        var totalQuantity = ingredients.Sum(ing => ing.Quantity);

        OnDissolutionStart?.Invoke();

        var quantityToPourLeft = totalQuantity; 
        while(totalQuantity > 0f)
        {
            var quantityMaxToPourThisFrame = pourSpeed * Time.deltaTime;
            var quantityToPourThisFrame = 0f;

            ingredients.RemoveAll(ing => ing.Quantity == 0);

            if (quantityToPourLeft < quantityMaxToPourThisFrame)
            {
                quantityToPourThisFrame = quantityToPourLeft;
                quantityToPourLeft = 0f;
            }
            else
            {
                quantityToPourThisFrame = quantityMaxToPourThisFrame;
                quantityToPourLeft -= quantityMaxToPourThisFrame;
            }

            List<Ingredient> pouredIngredientsThisFrame = new();

            foreach (var ing in ingredients)
            {
                float removedQty = Mathf.Min(ing.Quantity, quantityToPourThisFrame * ing.Quantity / totalQuantity);
                ing.Quantity -= removedQty;

                Ingredient pouredIngredient = new(ing.Name, removedQty, ing.Quality, ing.Color, ing.Cures);
                pouredIngredientsThisFrame.Add(pouredIngredient);
            }

            targetIngredientWrapper.FillWith(pouredIngredientsThisFrame, quantityToPourThisFrame);

            totalQuantity = ingredients.Sum(ing => ing.Quantity);

            yield return new WaitForEndOfFrame();
        }

        OnDissolutionStart?.Invoke();
        Destroy(gameObject);
    }
}
