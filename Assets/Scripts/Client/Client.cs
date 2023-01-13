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

    public string Name { get => name; }
    public DiseaseName Disease { get => disease; }
    public List<Symptom> Symptoms { get; private set; }

    public Client(string name, DiseaseName disease)
    {
        this.name = name;
        this.disease = disease;
        Symptoms = new List<Symptom>();
    }

    public void AddSymptoms(params Symptom[] symptoms)
    {
        foreach(Symptom symptom in symptoms) Symptoms.Add(symptom);
    }
}
