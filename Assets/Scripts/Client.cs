using Facebook.WitAi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Client
{
    [SerializeField] private string name;
    [SerializeField] private DiseaseName disease;

    private readonly List<Symptom> symptoms;

    public Client(string name, DiseaseName disease)
    {
        this.name = name;
        this.disease = disease;
        symptoms = new List<Symptom>();
    }

    public void AddSymptom(Symptom symptom) => symptoms.Add(symptom);

    public void RemoveSymptom(Symptom symptom) => symptoms.Remove(symptom);

    public void Heal() => symptoms.Clear();
}
