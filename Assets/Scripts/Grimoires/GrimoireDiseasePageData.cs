using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grimoires/GrimoireDiseasePageData", fileName = "GrimoireDiseasePageData")]
public class GrimoireDiseasePageData : ScriptableObject
{
    public DiseaseData disease;
    public IngredientData curePotion;

    public bool isFrontCover;
    public bool isBackCover;
    public Sprite coverSprite;
    public string coverTitle;
    public bool isTutorialPage;
    public string tutorialDescription;
}
