using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro board;

    public void UpdateDisplay(int score, int trust)
    {
        board.text = "Score: " + score + "\nConfiance: " + trust;
    }
}
