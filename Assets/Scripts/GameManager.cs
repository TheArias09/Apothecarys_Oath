using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform clientsParent;

    [Header("Parameters")]
    [SerializeField] private float timeBetweenClients = 60;

    private float timer = 0;
    private int score = 0;
    private int clientNumber = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (timer <= 0)
        {
            timer = timeBetweenClients;

            GameObject newClientInstance = Instantiate(clientPrefab, clientsParent);
            Client client = new (clientNumber.ToString(), DiseaseName.STRESS);
            newClientInstance.GetComponent<ClientBehavior>().Client = client;
        }
    }
}
