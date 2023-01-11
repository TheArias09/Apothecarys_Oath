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

    public void DisplayDataForLeftPage(GrimoirePotionPageData data)
    {
        potionImage.sprite = data.potionImage;
        potionDescription.text = data.potionDescription;
        potionName.text = data.name;
    }

    public void DisplayDataForRightPage(GrimoirePotionPageData data)
    {
        recipeDescription.text = data.recipeDescription;
    }

    public void Clear()
    {
        if(potionImage) potionImage.sprite = null;
        if(potionDescription) potionDescription.text = "";
        if(recipeDescription) recipeDescription.text = "";
        if(potionName) potionName.text = "";
    }
}
