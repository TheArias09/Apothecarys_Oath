using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grimoires/GrimoirePotionPageData", fileName = "GrimoirePotionPageData")]
public class GrimoirePotionPageData : ScriptableObject
{
    public Sprite potionImage;
    public string potionDescription;
    public string recipeDescription;

    public bool isFrontCover;
    public bool isBackCover;
}
