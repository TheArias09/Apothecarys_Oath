using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mix : MonoBehaviour
{
    [SerializeField] float accelerationThreshold = 0.01f;
    [SerializeField] float mixCompletion = 0;
    [SerializeField] float mixCompletionIncreaseSpeed = 1f;
    [SerializeField] float mixCompletionDecreaseSpeed = 1f;
    [SerializeField] float mixCompletionThreshold = 1f;

    Vector3 previousPreviousPosition;
    Vector3 previousPosition;

    IngredientWrapper ingredientWrapper;

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

        if(mixCompletion > mixCompletionThreshold)
        {
            MixComplete();
        }

        previousPreviousPosition = previousPosition;
        previousPosition = transform.position;
    }

    private void MixComplete()
    {
        PotionMaker.Instance.CheckPotion(ingredientWrapper);
        ingredientWrapper.CallOnQuantityUpdated();
        Debug.Log("MixComplete!");
        mixCompletion = 0f;
    }

    //TODO: Prendre en compte le débordement (possibilité de boucher ?)
}
