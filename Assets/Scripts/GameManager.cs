using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreDisplay scoreDisplay;
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform clientsParent;

    [Header("Clients")]
    [SerializeField] private float timeBetweenClients = 60;
    [SerializeField] private float clientStayTime = 120;
    [SerializeField] private int maxClients = 6;
    [SerializeField] private int minSymptoms = 2;
    [SerializeField] private int maxSymptoms = 3;

    [Header("Parameters")]
    [SerializeField] private float spaceBetweenTickets = 0.55f;
    [SerializeField] private float scoreMultiplier = 5;
    [SerializeField] private int maxErrors = 3;


    private float timer = 0;
    private int score = 0;
    private int errors = 0;

    private int currentClients = 0;
    private int clientNumber = 0;

    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (currentClients >= maxClients) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = timeBetweenClients;

            Client client = new(clientNumber.ToString(), DiseaseName.FATIGUE);
            Transform clientObject = clientsParent.GetChild(clientNumber % maxClients);
            ClientBehavior behavior = clientObject.GetComponent<ClientBehavior>();

            behavior.Setup(client, clientStayTime, clientNumber % maxClients);
            behavior.UpdateDisplay();

            clientObject.gameObject.SetActive(true);

            currentClients++;
            clientNumber++;
        }
    }

    public void GivePotion(GameObject potionContainer)
    {
        potionContainer.TryGetComponent(out IngredientWrapper wrapper);
        if (wrapper == null || wrapper.Ingredients.Count != 1)
        {
            Debug.Log("No correct potion submited");
            return;
        }

        Ingredient potion = wrapper.Ingredients[0];

        if (potion.Cures == null)
        {
            Debug.Log("No correct potion submited");
            return;
        }

        foreach (Transform child in clientsParent)
        {
            child.TryGetComponent(out ClientBehavior behavior);
            if (behavior.Client.Disease == potion.Cures)
            {
                behavior.ReceivePotion(potion);
                return;
            }
        }

        Debug.Log("Potion does not correspond to any client");
    }

    public void ClientLeave(int position)
    {
        currentClients--;
        clientsParent.GetChild(position).gameObject.SetActive(false);
    }

    public void AddScore(float value)
    {
        score += (int)(value * scoreMultiplier);
        scoreDisplay.UpdateDisplay(score, maxErrors - errors);
    }

    public void AddError()
    {
        errors++;
        if (errors >= maxErrors) GameOver();

        scoreDisplay.UpdateDisplay(score, maxErrors - errors);
    }

    public void GameOver()
    {
        Application.Quit();
    }
}