using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject bestDeliveryEffect;
    [SerializeField] private GameObject goodDeliveryEffect;
    [SerializeField] private GameObject badDeliveryEffect;

    [Header("Parameters")]
    [SerializeField] private int badThreshold;
    [SerializeField] private int goodThreshold;

    private GameObject deliveryEffect;

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
        DeliveryEffect(deliveryRank);

        if (ingredientWrapper.Respawner)
        {
            ingredientWrapper.Respawner.MovePosition();
            ingredientWrapper.Respawner.StartRespawnCoroutine();
        }
    }

    private void DeliveryEffect(int potionRank)
    {
        Debug.Log("DeliveryEffect");

        if (potionRank <= badThreshold)
        {
            deliveryEffect = badDeliveryEffect;
        }
        else if (potionRank <= goodThreshold)
        {
            deliveryEffect = goodDeliveryEffect;
        }
        else
        {
            deliveryEffect = bestDeliveryEffect;
        }

        Instantiate(deliveryEffect, transform);
    }
}
