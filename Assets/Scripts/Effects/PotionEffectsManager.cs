using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffectsManager : MonoBehaviour
{
    private IngredientWrapper ingredientWrapper;
    private List<Ingredient> ingredients;
    private Ingredient topIngredient;
    
    private LiquidVisualsManager liquidVisualsManager;
    private int liquidCount;
    private int previousLiquidCount;

    private GameObject mixEffect;
    private GameObject continuousEffect;
    private GameObject activeContinuousEffect;
    
    void Start()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
        ingredients = ingredientWrapper.Ingredients;
        
        liquidVisualsManager = GetComponent<LiquidVisualsManager>();
        liquidCount = ingredientWrapper.Ingredients.Count;
        previousLiquidCount = liquidCount;
        
        ContinuousEffect();
    }

    void Update()
    {
        liquidCount = ingredientWrapper.Ingredients.Count;;
        if (liquidCount != previousLiquidCount)
        {
            previousLiquidCount = liquidCount;
            ContinuousEffect();
        }
    }

    public void MixEffect()
    {
        Debug.Log("MixEffect");
        if (liquidCount > 0)
        {
            mixEffect = ingredients[^1].Data.mixEffect;
            if (mixEffect != null)
            {
                Instantiate(mixEffect, transform);
            }
        }
    }

    //supprime l'effet continu précédent et en lance un nouveau
    private void ContinuousEffect()
    {
        //delete last effect if it exists
        if (activeContinuousEffect != null)
        {
            Destroy(activeContinuousEffect);
        }

        //start new effect
        if (liquidCount > 0)
        {
            continuousEffect = ingredients[^1].Data.continuousEffect;
            if (continuousEffect != null)
            {
                activeContinuousEffect = Instantiate(continuousEffect, transform, true);
            }
        }
    }
}
