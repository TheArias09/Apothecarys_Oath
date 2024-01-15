using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrimoirePotionPage : MonoBehaviour
{
    [SerializeField] Image potionImage;
    [SerializeField] TMP_Text potionDescription;
    [SerializeField] TMP_Text potionName;

    [SerializeField] List<GameObject> recipeParts;
    [SerializeField] List<Image> recipePartImages;
    [SerializeField] List<TMP_Text> recipePartTexts;
    [SerializeField] int recipePartMaxCount;

    [SerializeField] TMP_Text bookTitle;
    [SerializeField] Image bookImage;

    [SerializeField] GameObject pagePanel;
    [SerializeField] GameObject frontCoverPanel;
    [SerializeField] GameObject backCoverPanel;

    public void DisplayData(GrimoirePotionPageData data)
    {
        pagePanel.SetActive(false);
        frontCoverPanel.SetActive(false);
        backCoverPanel.SetActive(false);

        if (data.isFrontCover)
        {
            frontCoverPanel.SetActive(true);
            bookTitle.text = data.coverTitle;
            bookImage.sprite = data.coverSprite;
        }
        else if (data.isBackCover)
        {
            backCoverPanel.SetActive(true);

            potionImage.sprite = data.recipeData.recipe.Result.symbol;
            potionDescription.text = data.recipeData.recipe.Result.description;
            potionName.text = data.recipeData.recipe.Name;
        }
        else
        {
            pagePanel.SetActive(true);
            if (recipeParts.Count > 0)
            {
                for(int i = 0; i < recipePartMaxCount; i++)
                {
                    if(i < data.recipeData.recipe.Ingredients.Count)
                    {
                        var ingredientData = data.recipeData.recipe.Ingredients[i];
                        recipePartImages[i].sprite = ingredientData.Data.symbol;
                        var displayPartText = i < data.recipeData.recipe.RecipeInstructions.Count;
                        recipePartTexts[i].text = displayPartText ? data.recipeData.recipe.RecipeInstructions[i] : "";
                    }
                    recipeParts[i].SetActive(i < data.recipeData.recipe.Ingredients.Count);
                }
            }
            else
            {
                potionImage.sprite = data.recipeData.recipe.Result.symbol;
                potionDescription.text = data.recipeData.recipe.Result.description;
                potionName.text = data.name;
            }
        }
    }

    public void Clear()
    {
        if(potionImage) potionImage.sprite = null;
        if(potionDescription) potionDescription.text = "";
        //if(recipeDescription) recipeDescription.text = "";
        if(potionName) potionName.text = "";
    }
}