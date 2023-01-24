using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(IngredientWrapper))]
public class StartStopPotion : MonoBehaviour
{
    public enum MenuPotionType { Start, Stop, Help }

    [SerializeField] MenuPotionType type;

    [SerializeField] private float triggerQuantity;
    [SerializeField] private GameObject uiText;

    private IngredientWrapper ingredientWrapper;

    [SerializeField] UnityEvent OnHelpEvent;

    private void Start()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
    }

    void Update()
    {
        Ingredient ing = ingredientWrapper.Ingredients[0];

        if (ing.Quantity <= triggerQuantity)
        {
            Debug.Log("Game potion triggered");

            if (type == MenuPotionType.Start)
            {
                GameManager.Instance.StartGame();
            }
            else if (type == MenuPotionType.Stop)
            {
                GameManager.Instance.QuitGame();
            }
            else if (type == MenuPotionType.Help)
            {
                OnHelpEvent?.Invoke();
            }

            uiText.SetActive(false);
            Destroy(gameObject);
        }
    }
}
