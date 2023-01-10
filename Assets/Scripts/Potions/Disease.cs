using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DiseaseName
{
    FATIGUE, STRESS, COLD, POISON
}

[Serializable]
public enum Symptom
{
    HEADACHE, TIRED, FEVER, COUGH, SNEEZE, CRAMPS, VOMIT, WEAK
}

[Serializable]
public class Disease
{
    public DiseaseName name;
    public List<Symptom> symptoms;
    public string cure;

}