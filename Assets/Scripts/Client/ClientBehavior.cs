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

    [Header("Sounds")]
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip timeoutSound;

    [Header("Animation")]
    [SerializeField] private float tintTime;
    [Space(5)]
    [SerializeField] private MeshRenderer outline;
    [SerializeField] private MeshRenderer board;
    [Space(5)]
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material boardMaterial;
    [Space(5)]
    [SerializeField] private Material errorMaterial;
    [SerializeField] private Material successMaterial;
    [Space(5)]
    [SerializeField] private Color successColor;
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
        float lifeTime = Time.time - birthTime;
        if (lifeTime > stayTime && !hasLeft) Leave(false);

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
        float shakeEvalutation = shakeStart - uiTimer.fillAmount;

        if (shakeEvalutation > 0)
            transform.localPosition = initialPosition + shakeEvolution.Evaluate(shakeEvalutation / shakeStart) * shakeMagnitude * Random.insideUnitSphere;

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

    public void ReceivePotion(Ingredient potion)
    {
        if (potion.Cures != null && potion.Cures == Client.Disease.name)
        {
            GameManager.Instance.AddScore(potion.Quality);
            Leave(true);
        }
        else
        {
            Debug.Log(Client.Name + " was not given the correct potion.");
            Leave(false);
        }
    }

    private void Leave(bool success)
    {
        hasLeft = true;

        if (!success) GameManager.Instance.AddError();
        if (GameManager.Instance.GameStarted) StartCoroutine(TintTicket(success));
    }

    private IEnumerator TintTicket(bool success)
    {
        if (success)
        {
            outline.material = successMaterial;
            board.material = successMaterial;

            title.color = successColor;
            content.color = successColor;

            audioSource.clip = winSound;
        }
        else
        {
            outline.material = errorMaterial;
            board.material = errorMaterial;

            title.color = errorColor;
            content.color = errorColor;

            audioSource.clip = timeoutSound;
        }

        audioSource.Play();
        yield return new WaitForSeconds(tintTime);

        outline.gameObject.SetActive(false);
        board.gameObject.SetActive(false);
        title.transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(tintTime);

        outline.material = outlineMaterial;
        board.material = boardMaterial;

        title.color = Color.white;
        content.color = uiColor;

        outline.gameObject.SetActive(true);
        board.gameObject.SetActive(true);
        title.transform.parent.gameObject.SetActive(true);

        GameManager.Instance.ClientLeave(position);
    }
}