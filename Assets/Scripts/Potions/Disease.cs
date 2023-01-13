using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DiseaseName
{
    FATIGUE, STRESS, COLD, POISON, INSOMNIA
}

[Serializable]
public enum Symptom
{
    HEADACHE, TIRED, FEVER, COUGH, SNEEZE, CRAMPS, VOMIT, WEAK, HALLUCINATIONS
}

[Serializable]
public class Disease
{
    public DiseaseName name;
    public List<Symptom> symptoms;
}