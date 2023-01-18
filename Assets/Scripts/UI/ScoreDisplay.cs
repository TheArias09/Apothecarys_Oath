using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro board;
    [SerializeField] private Transform icons;
    [SerializeField] private Sprite errorSprite;

    public void UpdateScore(int score)
    {
        board.text = "Score: " + score + "\nConfiance: ";
    }

    public void UpdateErrors(int errors)
    {
        icons.GetChild(errors - 1).GetComponent<Image>().sprite = errorSprite;
    }
}
