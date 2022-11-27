using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseasesList : ScriptableObject
{
    private Dictionary<DiseaseName, Disease> diseases = new();
}
