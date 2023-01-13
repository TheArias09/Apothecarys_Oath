using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreDisplay scoreDisplay;
    [SerializeField] private Transform clientsParent;
    [SerializeField] private DiseaseBook diseaseBook;

    [Header("Clients")]
    [SerializeField] private float timeBetweenClients = 60;
    [SerializeField] private float clientStayTime = 120;
    [SerializeField] private int maxClients = 6;
    [SerializeField] private int minSymptoms = 2;
    [SerializeField] private int maxSymptoms = 3;

    [Header("Parameters")]
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

        if (timer <= 0) CreateClient();
    }

    private void CreateClient()
    {
        timer = timeBetweenClients;

        int random = Random.Range(0, diseaseBook.diseases.Count);
        int symptoms = Random.Range(minSymptoms, maxSymptoms + 1);

        Client client = new(clientNumber.ToString(), diseaseBook.diseases[random].disease, symptoms);
        Transform clientObject = clientsParent.GetChild(clientNumber % maxClients);
        ClientBehavior behavior = clientObject.GetComponent<ClientBehavior>();

        behavior.Setup(client, clientStayTime, clientNumber % maxClients);
        behavior.UpdateDisplay();

        clientObject.gameObject.SetActive(true);

        currentClients++;
        clientNumber++;
    }

    public void GivePotion(IngredientWrapper wrapper)
    {
        Ingredient potion = wrapper.Ingredients[0];

        if (potion.Cures == null || wrapper.Ingredients.Count != 1)
        {
            Debug.Log("No correct potion submited");
            return;
        }

        foreach (Transform child in clientsParent)
        {
            child.TryGetComponent(out ClientBehavior behavior);
            if (behavior.Client.Disease.name == potion.Cures)
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