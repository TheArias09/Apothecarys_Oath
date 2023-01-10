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

    [Header("Parameters")]
    [SerializeField] private float spaceBetweenTickets = 0.55f;
    [SerializeField] private float scoreMultiplier = 5;
    [SerializeField] private int maxErrors = 3;


    private float timer = 0;
    private int score = 0;
    private int errors = 0;

    private int currentClients = 0;
    private int clientNumber = 0;
    private float xPos = 0;

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
            
            GameObject newClientInstance = Instantiate(clientPrefab, clientsParent);
            newClientInstance.transform.localPosition = new Vector3(xPos, 0, 0);

            Client client = new (clientNumber.ToString(), DiseaseName.FATIGUE);
            newClientInstance.GetComponent<ClientBehavior>().Client = client;
            newClientInstance.GetComponent<ClientBehavior>().StayTime = clientStayTime;

            currentClients++;
            clientNumber++;

            //Update ticket position for next ticket
            xPos += spaceBetweenTickets;
            if (clientNumber % maxClients == 0) xPos = 0;
        }
    }

    public void GivePotion(GameObject potionContainer)
    {
        potionContainer.TryGetComponent<IngredientWrapper>(out IngredientWrapper wrapper);
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
            child.TryGetComponent<ClientBehavior>(out ClientBehavior behavior);
            if (behavior.Client.Disease == potion.Cures)
            {
                behavior.ReceivePotion(potion);
                return;
            }
        }

        Debug.Log("Potion does not correspond to any client");
    }

    public void ClientLeave() => currentClients--;

    public void AddScore(float value)
    {
        score += (int) (value * scoreMultiplier);
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
