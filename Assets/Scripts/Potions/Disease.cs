using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DiseaseName
{
    FATIGUE, MYSTICOLD, POISON, INSOMNIA, EXISTANTIAL_FLU, AGING, GROGGINESS, MISFORTUNE, LIBIDOWN, NONE
}

[Serializable]
public enum Symptom
{
    Headache, Tired, Fever, Cough, Sneeze, Cramps, Vomit, Weak, Hallucinations, Loneliness, Depression, Anxiety, Irritated, Wrinkles, Clumsy
}

[Serializable]
public class Disease
{
    public DiseaseName name;
    public List<Symptom> symptoms;
    public Sprite sprite;
}