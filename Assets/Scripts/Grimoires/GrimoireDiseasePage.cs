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

    public void DisplayData(GrimoireDiseasePageData data)
    {
        symptomImage.sprite = data.symptomImage;
        potionImage.sprite = data.potionImage;
        diseaseDescription.text = data.diseaseDescription;
        potionDescription.text = data.potionDescription;
        diseaseName.text = data.name;
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
