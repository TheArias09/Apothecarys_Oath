using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class ClientBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshPro title;
    [SerializeField] private TextMeshPro content;
    [SerializeField] private Image uiTimer;

    public Client Client { get; private set; }

    private float birthTime;
    private float stayTime;
    private int position;

    private bool hasLeft = false;

    // Update is called once per frame
    void Update()
    {
        float lifeTime = Time.time - birthTime;

        if (lifeTime > stayTime && !hasLeft) Leave(true);
        uiTimer.fillAmount = lifeTime / stayTime;
    }

    public void Setup(Client client, float staytime, int position)
    {
        birthTime = Time.time;

        this.Client = client;
        this.stayTime = staytime;
        this.position = position;

        hasLeft = false;
    }

    public void UpdateDisplay()
    {
        title.text = "Client #" + Client.Name;
        content.text = "Symptoms:\n";

        foreach (Symptom symptom in Client.Symptoms) content.text += symptom + "\n";
    }

    public void ReceivePotion(Ingredient potion)
    {
        if (potion.Cures != null && potion.Cures == Client.Disease.name)
        {
            Debug.Log(Client.Name + " was cured correctly!");
            GameManager.Instance.AddScore(potion.Quality);
        }
        else
        {
            Debug.Log(Client.Name + " was not given the correct potion...");
            GameManager.Instance.AddError();
        }

        Leave(false);
    }

    private void Leave(bool timeout)
    {
        hasLeft = true;

        if (timeout)
        {
            Debug.Log(Client.Name + " has had enough, he left.");
            GameManager.Instance.AddError();
        }
        else
        {
            Debug.Log(Client.Name + " left!");
        }

        GameManager.Instance.ClientLeave(position);
    }
}