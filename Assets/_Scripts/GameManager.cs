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
    [SerializeField] private float timerThreshold = 0.25f;
    [SerializeField] private float minSpeedBonus = 0.5f;
    [SerializeField] private float maxSpeedBonus = 1.5f;

    [Header("Ranks")]
    [SerializeField] private float[] rankThresholds;
    [SerializeField] private string[] rankTitles;

    [Header("GameModes")] 
    private bool boardIsInPlay = false;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject inPlayBoard;
    [SerializeField] private GameObject outPlayBoard;
    [Space(5)]
    [SerializeField] private GameObject demoCheck;
    [SerializeField] private Collider demoCheckCollider;
    [SerializeField] private ProgressionParameters progressionParametersDemo;
    [Space(2)]
    [SerializeField] private GameObject infiniteCheck;
    [SerializeField] private Collider infiniteCheckCollider;
    [SerializeField] private ProgressionParameters progressionParametersInfinite;
    
    [Header("Events")]
    [SerializeField] private UnityEvent OnGameStart;
    [SerializeField] private UnityEvent OnDefeat;
    [SerializeField] private UnityEvent OnOneChanceLeft;
    [SerializeField] private UnityEvent OnVictory;
    [SerializeField] private UnityEvent OnGameStop;

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
        SetBoard();
        
        maxTickets = clientsParent.childCount;
        maxClients = progressionParameters.totalClients;

        timeBetweenClients = progressionParameters.timeBetweenClients[0];
        clientStayTime = progressionParameters.clientStayTime[0];

        if (rankThresholds.Count() +1 != rankTitles.Count()) 
            Debug.LogWarning("There should be an equal amount of ranks and rank titles.");
    }

    private void Update()
    {
        if (gameStarted && !boardIsInPlay)
        {
            outPlayBoard.SetActive(false);
            inPlayBoard.SetActive(true);
            boardIsInPlay = true;
        }
        else if (!gameStarted && boardIsInPlay)
        {
            inPlayBoard.SetActive(false);
            outPlayBoard.SetActive(true);
            boardIsInPlay = false;
        }

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

    private string GetGameRank(int value)
    {
        int index = 0;
        float maxScore = scoreMultiplier * maxSpeedBonus * progressionParameters.totalClients;

        while (index < rankTitles.Length - 1 && value > rankThresholds[index] * maxScore) index++;
        return rankTitles[index];
    }

    public int GetPotionRank(int value)
    {
        int index = 0;
        float maxScore = scoreMultiplier * maxSpeedBonus;

        while (index < rankTitles.Length - 1 && value > rankThresholds[index] * maxScore) index++;
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

    public int AddScore(float quality, float quantity, float timer)
    {
        clientsHealed++;

        float quantScore = Mathf.Min(quantity / minPotionQty, 1);
        float speedBonus = minSpeedBonus + (timer - timerThreshold) * (maxSpeedBonus - minSpeedBonus) / (1 - 2*timerThreshold);
        float speedScore = Mathf.Clamp(speedBonus, minSpeedBonus, maxSpeedBonus);

        int value = (int)(quality * quantScore * speedScore * scoreMultiplier);
        score += value;

        Debug.Log("Potion scores: Qual=" + quality + ", Quant=" + quantScore + ", Speed=" + speedScore);
        scoreDisplay.UpdateScore(score);
        return value;
    }

    public void AddError()
    {
        errors++;
        scoreDisplay.UpdateErrors(errors);

        if(errors == maxErrors - 1) OnOneChanceLeft.Invoke();

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

        string rank = GetGameRank(score);

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

    public void ResetGame(bool restart)
    {

        Debug.Log("Reset");
        timer = 0;
        phase = 0;
        score = 0;
        errors = 0;
        currentClients = 0;
        clientNumber = 0;
        clientsHealed = 0;
        lastDisease = -1;

        maxTickets = clientsParent.childCount;
        maxClients = progressionParameters.totalClients;

        timeBetweenClients = progressionParameters.timeBetweenClients[0];
        clientStayTime = progressionParameters.clientStayTime[0];

        scoreDisplay.ResetErrors();
        gameStarted = restart;

        foreach (Transform child in clientsParent) child.gameObject.SetActive(false);

        if (restart) OnGameStart?.Invoke();
        else OnGameStop?.Invoke();
    }

    public void QuitGame() => Application.Quit();

    

    #region GameModes

    public void SetBoard()
    {
        board.SetActive(false);
        SetDemoMode();
        board.SetActive(true);
    }
    
    public void SetDemoMode()
    {
        demoCheck.SetActive(true);
        demoCheckCollider.gameObject.SetActive(false);
        infiniteCheck.SetActive(false);
        infiniteCheckCollider.gameObject.SetActive(true);
        progressionParameters = progressionParametersDemo;
    }
    
    public void SetInfiniteMode()
    {
        infiniteCheck.SetActive(true);
        infiniteCheckCollider.gameObject.SetActive(false);
        demoCheck.SetActive(false);
        demoCheckCollider.gameObject.SetActive(true);
        progressionParameters = progressionParametersInfinite;
    }

    #endregion
}