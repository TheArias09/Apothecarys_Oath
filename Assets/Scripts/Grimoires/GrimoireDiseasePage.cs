using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrimoireDiseasePage : MonoBehaviour
{
    [SerializeField] Image diseaseImage;
    [SerializeField] Image potionImage;
    [SerializeField] TMP_Text diseaseDescription;
    [SerializeField] TMP_Text potionDescription;
    [SerializeField] TMP_Text diseaseName;

    [SerializeField] TMP_Text bookTitle;
    [SerializeField] Image bookImage;

    [SerializeField] TMP_Text tutorialTitle;
    [SerializeField] TMP_Text tutorialDescription;
    [SerializeField] Image tutorialImage;

    [SerializeField] GameObject pagePanel;
    [SerializeField] GameObject frontCoverPanel;
    [SerializeField] GameObject backCoverPanel;
    [SerializeField] GameObject tutorialPanel;

    public void DisplayData(GrimoireDiseasePageData data)
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
        }
        else if (data.isTutorialPage)
        {
            tutorialPanel.SetActive(true);
            tutorialDescription.text = data.tutorialDescription;
            tutorialTitle.text = data.coverTitle;
            tutorialImage.sprite = data.coverSprite;
        }
        else
        {
            pagePanel.SetActive(true);
            diseaseImage.sprite = data.disease.disease.sprite;
            potionImage.sprite = data.curePotion.symbol;

            var descriptionText = "";
            for(int i = 0; i < data.disease.disease.symptoms.Count - 1; ++i)
            {
                descriptionText += "• " + data.disease.disease.symptoms[i].ToString();
                descriptionText += "\n";
            }
            descriptionText += "• " + data.disease.disease.symptoms[data.disease.disease.symptoms.Count - 1].ToString();

            diseaseDescription.text = descriptionText;
            potionDescription.text = "Treatment:\n" + data.curePotion.name;
            diseaseName.text = data.name;
        }
    }

    public void Clear()
    {
        diseaseImage.sprite = null;
        potionImage.sprite = null;
        diseaseDescription.text = "";
        potionDescription.text = "";
        diseaseName.text = "";
    }
}
