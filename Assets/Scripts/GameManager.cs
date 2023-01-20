using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Dependancies")]
    [SerializeField] private ScoreDisplay scoreDisplay;
    [SerializeField] private Transform clientsParent;
    [SerializeField] private DiseaseBook diseaseBook;
    [SerializeField] private GameObject winPage;
    [SerializeField] private GameObject firedPage;

    [Header("Clients")]
    [SerializeField] private float timeBetweenClients = 60;
    [SerializeField] private float clientStayTime = 120;
    [SerializeField] private int maxClients = 10;
    [SerializeField] private int minSymptoms = 2;
    [SerializeField] private int maxSymptoms = 3;

    [Header("Parameters")]
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private float scoreMultiplier = 5;
    [SerializeField] private int maxErrors = 3;
    [Space(10)]
    [SerializeField] private int[] ranks;
    [SerializeField] private string[] rankTitles;

    private float timer = 0;
    private int score = 0;
    private int errors = 0;
    private int maxTickets;

    private int currentClients = 0;
    private int clientNumber = 0;
    private int clientsHealed = 0;

    public static GameManager Instance;
    public bool GameStarted { get => gameStarted; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        maxTickets = clientsParent.childCount;

        if (ranks.Count() != rankTitles.Count()) 
            Debug.LogWarning("There should be an equal amount of ranks and rank titles.");
    }

    private void Update()
    {
        if (!gameStarted || currentClients >= maxTickets) return;

        timer -= Time.deltaTime;

        if (timer <= 0 || currentClients == 0) CreateClient();
    }

    private void CreateClient()
    {
        timer = timeBetweenClients;

        int random = Random.Range(0, diseaseBook.diseases.Count);
        int symptoms = Random.Range(minSymptoms, maxSymptoms + 1);

        Client client = new(clientNumber.ToString(), diseaseBook.diseases[random].disease, symptoms);
        Transform clientObject = clientsParent.GetChild(clientNumber % maxTickets);
        ClientBehavior behavior = clientObject.GetComponent<ClientBehavior>();

        behavior.Setup(client, clientStayTime, clientNumber % maxTickets);
        behavior.UpdateDisplay();

        clientObject.gameObject.SetActive(true);

        currentClients++;
        clientNumber++;
    }

    private string GetRank()
    {
        int index = 0;
        while (ranks[index] < score) index++;
        return rankTitles[index];
    }

    public void GivePotion(Ingredient potion)
    {
        foreach (Transform child in clientsParent)
        {
            child.TryGetComponent(out ClientBehavior behavior);
            if (behavior.Client.Disease.name == potion.Cures)
            {
                behavior.ReceivePotion(potion);
                Debug.Log("Good Potion");
                return;
            }
        }

        Debug.Log("Potion does not correspond to any client");
    }

    public void ClientLeave(int position)
    {
        currentClients--;
        clientsParent.GetChild(position).gameObject.SetActive(false);

        if (clientNumber >= maxClients) GameOver(true);
    }

    public void AddScore(float value)
    {
        clientsHealed++;
        score += (int)(value * scoreMultiplier);
        Debug.Log("score added: " + value);
        scoreDisplay.UpdateScore(score);
    }

    public void AddError()
    {
        errors++;
        scoreDisplay.UpdateErrors(errors);

        if (errors >= maxErrors) GameOver(false);
    }

    public void StartGame()
    {
        Debug.Log("Start game");
        gameStarted = true;
    }

    public void Restart()
    {
        Debug.Log("Restart game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver(bool win)
    {
        Debug.Log("Game Over");
        gameStarted = false;

        string rank = GetRank();

        if (win)
        {
            winPage.SetActive(true);
            winPage.GetComponent<WinningScroll>().WinDisplay(clientsHealed, score, rank);
        }
        else
        {
            firedPage.SetActive(true);
            winPage.GetComponent<WinningScroll>().FiredDisplay(score, rank);
        }

        foreach (Transform child in clientsParent) child.gameObject.SetActive(false);
    }

    public void QuitGame() => Application.Quit();
}