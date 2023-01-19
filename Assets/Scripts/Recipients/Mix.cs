using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if(mixCompletion > mixCompletionThreshold) MixComplete();

        previousPreviousPosition = previousPosition;
        previousPosition = transform.position;
    }

    private void MixComplete()
    {
        if (ingredientWrapper.GetTotalQty() == 0) return;
        PotionMaker.Instance.CheckPotion(ingredientWrapper);
        ingredientWrapper.CallOnQuantityUpdated();
        Debug.Log("Mix complete!");
        mixCompletion = 0f;
    }

    //TODO: Prendre en compte le d�bordement (possibilit� de boucher ?)
}
