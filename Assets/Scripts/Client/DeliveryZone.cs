using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.TryGetComponent<IngredientWrapper>(out var ingredientWrapper);
        if (ingredientWrapper == null) return;

        Ingredient potion = ingredientWrapper.Ingredients[0];

        if (potion.Cures == DiseaseName.NONE || ingredientWrapper.Ingredients.Count != 1)
        {
            Debug.Log("Incorrect potion submited");
            return;
        }

        GameManager.Instance.GivePotion(potion);
        ingredientWrapper.Empty();

        if (ingredientWrapper.Respawner)
        {
            ingredientWrapper.Respawner.MovePosition();
            ingredientWrapper.Respawner.StartRespawnCoroutine();
        }
    }
}
