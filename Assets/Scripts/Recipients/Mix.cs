using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mix : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float accelerationThreshold = 0.01f;
    [SerializeField] private float mixCompletionIncreaseSpeed = 1f;
    [SerializeField] private float mixCompletionDecreaseSpeed = 1f;
    [SerializeField] private float mixCompletionThreshold = 1f;
    
    [Header(("Debug"))]
    [SerializeField] float mixCompletion = 0;
    
    Vector3 previousPreviousPosition;
    Vector3 previousPosition;
    IngredientWrapper ingredientWrapper;

    private float startClock = 1f;

    [SerializeField] UnityEvent OnMixComplete;

    private void Awake()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>(); 
    }

    private void Start()
    {
        previousPreviousPosition = transform.position;
        previousPosition = transform.position;
    }

    private void Update()
    {
        if (startClock > 0f)
        {
            startClock -= Time.deltaTime;
            return;
        }

        float currentAcceleration = (transform.position - 2 * previousPosition + previousPreviousPosition).sqrMagnitude * Time.deltaTime;
        if(currentAcceleration > accelerationThreshold * accelerationThreshold)
        {
            mixCompletion += mixCompletionIncreaseSpeed * Time.deltaTime;
        }
        else
        {
            mixCompletion -= mixCompletionDecreaseSpeed * Time.deltaTime;
            if(mixCompletion < 0) mixCompletion = 0;
        }

        if(mixCompletion > mixCompletionThreshold  && !ingredientWrapper.Mixed) MixComplete();

        previousPreviousPosition = previousPosition;
        previousPosition = transform.position;
    }

    private void MixComplete()
    {
        if (ingredientWrapper.GetTotalQty() == 0) return;

        OnMixComplete?.Invoke();

        ingredientWrapper.Mixed = PotionMaker.Instance.CheckPotion(ingredientWrapper);
        ingredientWrapper.CallOnQuantityUpdated();

        mixCompletion = 0f;
        Debug.Log("Mix complete!");
    }

    //TODO: Prendre en compte le d�bordement (possibilit� de boucher ?)
}
