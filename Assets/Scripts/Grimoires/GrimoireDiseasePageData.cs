using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grimoires/GrimoireDiseasePageData", fileName = "GrimoireDiseasePageData")]
public class GrimoireDiseasePageData : ScriptableObject
{
    public Sprite symptomImage;
    public string diseaseDescription;
    public Sprite potionImage;
    public string potionDescription;
}
