using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinningScroll : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI rankText;

    public void Display(int clientsHealed, int score, string rank)
    {
        text.text = "You healed" + clientsHealed + "clients\n\nFinal score: " + score;
        rankText.text = rank;
    }
}
