using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro board;
    [SerializeField] private Transform icons;

    [Header("Icons")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite errorSprite;


    public void UpdateScore(int score)
    {
        board.text = "Score: " + score + "\nReputation: ";
    }

    public void UpdateErrors(int errors)
    {
        icons.GetChild(errors - 1).GetComponent<Image>().sprite = errorSprite;
    }

    public void ResetErrors()
    {
        for (int i = 0; i < icons.childCount; i++)
        {
            icons.GetChild(i).GetComponent<Image>().sprite = normalSprite;
        }
    }
}
