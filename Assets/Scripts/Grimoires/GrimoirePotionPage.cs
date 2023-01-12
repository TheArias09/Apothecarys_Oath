using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrimoirePotionPage : MonoBehaviour
{
    [SerializeField] Image potionImage;
    [SerializeField] TMP_Text potionDescription;
    [SerializeField] TMP_Text recipeDescription;
    [SerializeField] TMP_Text potionName;

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
            bookTitle.text = data.potionDescription;
            bookImage.sprite = data.potionImage;
        }
        else if (data.isBackCover)
        {
            backCoverPanel.SetActive(true);
        }
        else
        {
            pagePanel.SetActive(true);
            if (recipeDescription)
            {
                recipeDescription.text = data.recipeDescription;
            }
            else
            {
                potionImage.sprite = data.potionImage;
                potionDescription.text = data.potionDescription;
                potionName.text = data.name;
            }
        }
    }

    public void Clear()
    {
        if(potionImage) potionImage.sprite = null;
        if(potionDescription) potionDescription.text = "";
        if(recipeDescription) recipeDescription.text = "";
        if(potionName) potionName.text = "";
    }
}
