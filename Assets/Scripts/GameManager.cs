using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform clientsParent;

    [Header("Parameters")]
    [SerializeField] private float timeBetweenClients = 60;
    [SerializeField] private float clientStayTime = 120;
    [SerializeField] private float scoreMultiplier = 5;
    [SerializeField] private int maxErrors = 3;

    private float timer = 0;
    private int score = 0;
    private int errors = 0;
    private int clientNumber = 0;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (timer <= 0)
        {
            timer = timeBetweenClients;

            GameObject newClientInstance = Instantiate(clientPrefab, clientsParent);
            Client client = new (clientNumber.ToString(), DiseaseName.STRESS);
            newClientInstance.GetComponent<ClientBehavior>().Client = client;
            newClientInstance.GetComponent<ClientBehavior>().StayTime = clientStayTime;
        }

        timer += Time.deltaTime;
    }

    public void AddScore(float value)
    {
        score += (int) (value * scoreMultiplier);
    }

    public void AddError()
    {
        errors++;
        if (errors >= maxErrors) GameOver();
    }

    public void GameOver()
    {
        Application.Quit();
    }
}
