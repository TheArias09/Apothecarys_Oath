using Facebook.WitAi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class Client
{
    public string Name { get; private set; }
    public Disease Disease { get; private set; }
    public List<Symptom> Symptoms { get; private set; }
    public float DesiredQty { get; private set; }

    public Client(string name, Disease disease, int symptoms, float quantity)
    {
        Name = name;
        Disease = disease;
        DesiredQty = quantity;
        Symptoms = new();
        AddSymptoms(symptoms);
    }

    public void AddSymptoms(int quantity)
    {
        List<Symptom> symptomsCopy = new(Disease.symptoms);

        //Get quantity number of random symptoms from disease without repetition 
        for(int i=0; i< quantity; i++)
        {
            int rand = UnityEngine.Random.Range(0, symptomsCopy.Count);
            Symptom s = symptomsCopy[rand];

            symptomsCopy.RemoveAt(rand);
            Symptoms.Add(s);
        }
    }

    public void Cure()
    {
        Disease = null;
        Symptoms.Clear();
    }
}
