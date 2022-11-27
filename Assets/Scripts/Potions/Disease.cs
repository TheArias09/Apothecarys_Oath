using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum DiseaseName
{
    STRESS, COLD, POISONED
}

[Serializable]
public enum Symptom
{
    HEADACHE, TIRED, FEVER, COUGH, SNEEZE, CRAMPS, VOMIT
}

[Serializable]
public class Disease
{
    public DiseaseName name;
    public List<Symptom> symptoms;
    public string cure;

}