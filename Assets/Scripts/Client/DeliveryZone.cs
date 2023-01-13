using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.TryGetComponent<IngredientWrapper>(out var ingredientWrapper);
        if (ingredientWrapper == null) return;

        GameManager.Instance.GivePotion(ingredientWrapper);
        ingredientWrapper.Empty();
    }
}
