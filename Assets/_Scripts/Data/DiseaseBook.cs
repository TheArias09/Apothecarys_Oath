using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DiseaseBook", fileName = "DiseaseBook")]
public class DiseaseBook : ScriptableObject
{
    public List<DiseaseData> diseases = new();
}

