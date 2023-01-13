using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DiseaseName
{
    FATIGUE, MYSTICOLD, POISON, INSOMNIA, EXISTANTIAL_FLU, UGLINESS, GROGGINESS, MISFORTUNE, CHASTITY
}

[Serializable]
public enum Symptom
{
    HEADACHE, TIRED, FEVER, COUGH, SNEEZE, CRAMPS, VOMIT, WEAK, HALLUCINATIONS, LONELINESS, DEPRESSION, ANXIETY, IRRITATED, BLUE_POX, CLUMSY
}

[Serializable]
public class Disease
{
    public DiseaseName name;
    public List<Symptom> symptoms;
}