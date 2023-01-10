using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreDisplay scoreDisplay;
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
            Client client = new (clientNumber.ToString(), DiseaseName.FATIGUE);
            newClientInstance.GetComponent<ClientBehavior>().Client = client;
            newClientInstance.GetComponent<ClientBehavior>().StayTime = clientStayTime;

            clientNumber++;
        }

        timer += Time.deltaTime;
    }

    public void GivePotion(GameObject potionContainer)
    {
        potionContainer.TryGetComponent<IngredientWrapper>(out IngredientWrapper wrapper);
        if (wrapper == null || wrapper.Ingredients.Count > 1)
        {
            Debug.Log("No correct potion submited");
            return;
        }

        Ingredient potion = wrapper.Ingredients[0];

    }

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
