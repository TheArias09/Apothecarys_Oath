using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grimoires/GrimoirePotionPageData", fileName = "GrimoirePotionPageData")]
public class GrimoirePotionPageData : ScriptableObject
{
    public RecipeData recipeData;

    public bool isFrontCover;
    public bool isBackCover;
    public Sprite coverSprite;
    public string coverTitle;
}
