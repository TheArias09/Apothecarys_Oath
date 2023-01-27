using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Dependancies")]
    [SerializeField] private DiseaseBook diseaseBook;
    [SerializeField] private ProgressionParameters progressionParameters;
    [SerializeField] private Transform clientsParent;
    [SerializeField] private ScoreDisplay scoreDisplay;
    [SerializeField] private GameObject winPage;
    [SerializeField] private GameObject firedPage;

    [Header("Clients")]
    [SerializeField] private int minSymptoms = 2;
    [SerializeField] private int maxSymptoms = 3;
    [SerializeField] private float minPotionQty = 0.5f;

    [Header("Parameters")]
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private int maxErrors = 3;
    [SerializeField] private float scoreMultiplier = 5;
    [Space(10)]
    [SerializeField] private float speedMultiplier = 2;
    [SerializeField] private float minSpeedBonus = 0.5f;
    [SerializeField] private float maxSpeedBonus = 1.5f;

    [Header("Ranks")]
    [SerializeField] private int[] potionRankThresholds;
    [SerializeField] private int[] gameRankThresholds;
    [SerializeField] private string[] rankTitles;

    [Header("Events")]
    [SerializeField] UnityEvent OnGameStart;
    [SerializeField] UnityEvent OnDefeat;
    [SerializeField] UnityEvent OnOneChanceLeft;
    [SerializeField] UnityEvent OnVictory;

    private float timer = 0;
    private int phase = 0;
    private int score = 0;
    private int errors = 0;
    private int maxTickets;

    private int maxClients;
    private int currentClients = 0;
    private int clientNumber = 0;
    private int clientsHealed = 0;
    private int lastDisease = -1;

    private float timeBetweenClients;
    private float clientStayTime;

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
        maxClients = progressionParameters.totalClients;

        timeBetweenClients = progressionParameters.timeBetweenClients[0];
        clientStayTime = progressionParameters.clientStayTime[0];

        if (gameRankThresholds.Count() +1 != rankTitles.Count()) 
            Debug.LogWarning("There should be an equal amount of ranks and rank titles.");
    }

    private void Update()
    {
        if (!gameStarted || currentClients >= maxTickets || clientNumber >= maxClients) return;

        timer -= Time.deltaTime;

        if (timer <= 0 || currentClients == 0) CreateClient();
    }

    private void CreateClient()
    {
        //Never pick twice the same disease in a row
        int random;
        do random = Random.Range(progressionParameters.diseases[phase].x, progressionParameters.diseases[phase].y +1);
        while (random == lastDisease);
        lastDisease = random;

        int symptoms = Random.Range(minSymptoms, maxSymptoms +1);

        Client client = new((clientNumber+1).ToString(), diseaseBook.diseases[random].disease, symptoms);
        Transform clientObject = clientsParent.GetChild(clientNumber % maxTickets);
        ClientBehavior behavior = clientObject.GetComponentInChildren<ClientBehavior>();

        behavior.Setup(client, clientStayTime, clientNumber % maxTickets);
        behavior.UpdateDisplay();

        clientObject.gameObject.SetActive(true);

        currentClients++;
        clientNumber++;

        //Changing phase, new parameters
        if (phase < progressionParameters.phaseEnd.Count &&
            clientNumber >= progressionParameters.phaseEnd[phase])
        {
            phase++;
            timeBetweenClients = progressionParameters.timeBetweenClients[phase];
            clientStayTime = progressionParameters.clientStayTime[phase];
            Debug.Log("Starting phase " + phase);
        }

        timer = timeBetweenClients;
    }

    private string GetRank()
    {
        int index = 0;

        while (index < rankTitles.Length - 1 && score > gameRankThresholds[index]) index++;
        return rankTitles[index];
    }

    public int GetPotionRank(int value)
    {
        int index = 0;

        while (index < rankTitles.Length - 1 && value > potionRankThresholds[index]) index++;
        return index;
    }

    public string GetRankName(int index) => rankTitles[index];

    public int GivePotion(Ingredient potion)
    {
        foreach (Transform child in clientsParent)
        {
            if (!child.gameObject.activeInHierarchy) continue;

            child.GetChild(0).TryGetComponent(out ClientBehavior behavior);
            if (behavior.Client.Disease.name == potion.Cures)
            {
                return behavior.ReceivePotion(potion);
            }
        }
        
        return -1;
    }

    public void ClientLeave(int position)
    {
        currentClients--;
        clientsParent.GetChild(position).gameObject.SetActive(false);

        if (clientsHealed + errors >= maxClients) GameOver(true);
    }

    public int AddScore(float quality, float quantity, float speed)
    {
        clientsHealed++;

        float quantScore = Mathf.Min(quantity / minPotionQty, 1);
        float speedBonus = Mathf.Clamp(speed * speedMultiplier, minSpeedBonus, maxSpeedBonus);

        int value = (int)(quality * quantScore * speedBonus * scoreMultiplier);
        score += value;

        Debug.Log("Potion scores: Qual=" + quality + ", Quant=" + quantity + ", Speed=" + speedBonus);
        scoreDisplay.UpdateScore(score);
        return value;
    }

    public void AddError()
    {
        errors++;
        scoreDisplay.UpdateErrors(errors);

        if(errors == maxErrors - 1)
        {
            OnOneChanceLeft.Invoke();
        }

        if (errors >= maxErrors) GameOver(false);
    }

    public void StartGame()
    {
        OnGameStart.Invoke();
        gameStarted = true;
    }

    public void GameOver(bool win)
    {
        Debug.Log("Game Over");
        gameStarted = false;

        string rank = GetRank();

        if (win)
        {
            OnVictory.Invoke();
            winPage.SetActive(true);
            winPage.GetComponentInChildren<Pouf>().MakePouf();
            winPage.GetComponent<WinningScroll>().WinDisplay(clientsHealed, score, rank);
        }
        else
        {
            OnDefeat.Invoke();
            firedPage.SetActive(true);
            firedPage.GetComponentInChildren<Pouf>().MakePouf();
            firedPage.GetComponent<WinningScroll>().FiredDisplay(score, rank);
        }

        foreach (Transform child in clientsParent) child.gameObject.SetActive(false);
    }

    public void QuitGame() => Application.Quit();
}