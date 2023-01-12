using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrimoireDiseasePage : MonoBehaviour
{
    [SerializeField] Image symptomImage;
    [SerializeField] Image potionImage;
    [SerializeField] TMP_Text diseaseDescription;
    [SerializeField] TMP_Text potionDescription;
    [SerializeField] TMP_Text diseaseName;

    [SerializeField] TMP_Text bookTitle;
    [SerializeField] Image bookImage;

    [SerializeField] GameObject pagePanel;
    [SerializeField] GameObject frontCoverPanel;
    [SerializeField] GameObject backCoverPanel;

    public void DisplayData(GrimoireDiseasePageData data)
    {
        pagePanel.SetActive(false);
        frontCoverPanel.SetActive(false);
        backCoverPanel.SetActive(false);

        if (data.isFrontCover)
        {
            frontCoverPanel.SetActive(true);
            bookTitle.text = data.diseaseDescription;
            bookImage.sprite = data.symptomImage;
        }
        else if (data.isBackCover)
        {
            backCoverPanel.SetActive(true);
        }
        else
        {
            pagePanel.SetActive(true);
            symptomImage.sprite = data.symptomImage;
            potionImage.sprite = data.potionImage;
            diseaseDescription.text = data.diseaseDescription;
            potionDescription.text = data.potionDescription;
            diseaseName.text = data.name;
        }
    }

    public void Clear()
    {
        symptomImage.sprite = null;
        potionImage.sprite = null;
        diseaseDescription.text = "";
        potionDescription.text = "";
        diseaseName.text = "";
    }
}
