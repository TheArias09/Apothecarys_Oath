using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.TryGetComponent<IngredientWrapper>(out var ingredientWrapper);
        if (ingredientWrapper == null || ingredientWrapper.Ingredients.Count == 0) return;

        Ingredient potion = ingredientWrapper.Ingredients[0];

        if (potion.Cures == DiseaseName.NONE || ingredientWrapper.Ingredients.Count != 1)
        {
            Debug.Log("Incorrect potion submited");
            return;
        }

        int deliveryRank = GameManager.Instance.GivePotion(potion);
        if (deliveryRank == -1) return;
        
        ingredientWrapper.Empty();

        collision.gameObject.TryGetComponent<PotionEffectsManager>(out var effectsManager);
        if (effectsManager != null) effectsManager.DeliveryEffect(deliveryRank);

        if (ingredientWrapper.Respawner)
        {
            ingredientWrapper.Respawner.MovePosition();
            ingredientWrapper.Respawner.StartRespawnCoroutine();
        }
    }
}
