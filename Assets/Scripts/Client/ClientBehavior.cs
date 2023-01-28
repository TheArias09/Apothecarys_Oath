using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientBehavior : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshPro title;
    [SerializeField] private TextMeshPro content;
    [SerializeField] private Image uiTimer;
    [SerializeField] private Gradient timerColor;
    [SerializeField] private Color uiColor;

    [Header("Back UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI rankText;

    [Header("Components")]
    [SerializeField] private MeshRenderer outline;
    [SerializeField] private MeshRenderer board;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material boardMaterial;

    [Header("Sounds")]
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip timeoutSound;

    [Header("Animation")]
    [SerializeField] private float tintTime;
    [SerializeField] private float scoreDisplayTime;
    [Space(10)]
    [SerializeField] private Material errorMaterial;
    [SerializeField] private Color errorColor;

    [Header("Shaking")]
    [Tooltip("Percent of lifetime left before starting to shake.")]
    [SerializeField, Range(0, 1)] private float shakeStart;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private AnimationCurve shakeEvolution;

    public Client Client { get; private set; }

    private float birthTime;
    private float stayTime;
    private int position;

    private bool hasLeft = false;
    private int score;
    private int rank;
   
    private Vector3 initialPosition;
    private float shakePeriod;
    private float shakeTimer;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        initialPosition = transform.localPosition;
        shakePeriod = 1 / shakeFrequency;
    }

    void Update()
    {
        if (hasLeft) return;

        float lifeTime = Time.time - birthTime;
        if (lifeTime > stayTime) Leave(false);

        uiTimer.fillAmount = 1 - (lifeTime / stayTime);
        uiTimer.color = timerColor.Evaluate(uiTimer.fillAmount);
    }

    private void FixedUpdate()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            return;
        }

        shakeTimer = shakePeriod;
        float shakeEvalutation = (shakeStart - uiTimer.fillAmount) / shakeStart;

        if (!hasLeft && shakeEvalutation > 0)
            transform.localPosition = initialPosition + shakeEvolution.Evaluate(shakeEvalutation) * shakeMagnitude * Random.insideUnitSphere;

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

    public int ReceivePotion(Ingredient potion)
    {
        if (potion.Cures != DiseaseName.NONE && potion.Cures == Client.Disease.name)
        {
            score = GameManager.Instance.AddScore(potion.Quality, potion.Quantity, uiTimer.fillAmount);
            rank = GameManager.Instance.GetRankIndex(score);

            Client.Cure();
            Leave(true);

            return rank;
        }

        return -1;
    }

    private void Leave(bool success)
    {
        hasLeft = true;

        if (success) StartCoroutine(SuccessAnimation());
        else
        {
            GameManager.Instance.AddError();
            StartCoroutine(FailAnimation());
        }
    }

    private IEnumerator FailAnimation()
    {
        audioSource.clip = timeoutSound;
        audioSource.Play();

        //Tint ticket
        outline.material = errorMaterial;
        board.material = errorMaterial;

        title.color = errorColor;
        content.color = errorColor;

        yield return new WaitForSeconds(tintTime);

        //Back to normal
        outline.material = outlineMaterial;
        board.material = boardMaterial;

        title.color = Color.white;
        content.color = uiColor;

        GameManager.Instance.ClientLeave(position);
    }

    private IEnumerator SuccessAnimation()
    {
        audioSource.clip = winSound;
        audioSource.Play();

        scoreText.text = "+" + score;
        rankText.text = GameManager.Instance.GetRankName(rank);
        uiTimer.fillAmount = 0;

        Debug.Log("Client satisfied!" + " Score: " + score + " Rank: " + rankText.text);
        GetComponent<Animator>().Play("Leave");

        yield return new WaitForSeconds(scoreDisplayTime);
        GameManager.Instance.ClientLeave(position);
    }
}