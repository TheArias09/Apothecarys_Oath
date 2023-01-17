using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LiquidVisualsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> liquidVolumes;
    [SerializeField] private List<LiquidVisuals> liquids;
    [SerializeField] private List<Material> liquidRenderersMaterials;

    [SerializeField] private GameObject liquidVolume;

    [SerializeField] private float recipientQuantity;
    [SerializeField] private int liquidCount;

    private IngredientWrapper ingredientWrapper;
    private int previousLiquidCount;
    private float trueFill; //Entre 0 et 1
    private float displayedFill; //Entre -0.1 et 0.1

    private float calculatedTrueFill;
    private float angleToTop;
    
    //[SerializeField] private float absoluteDisplayFill = 0.1f;
    //[SerializeField] private float minDisplayFill = -0.1f;
    //[SerializeField] private float maxDisplayFill = 0.1f;
    
    [SerializeField] private AnimationCurve displayFillCurve;
    [SerializeField] private AnimationCurve horizontalDisplayFillCurve;
    
    public float RecipientQuantity => recipientQuantity;
    public List<LiquidVisuals> Liquids => liquids;
    public int LiquidCount => liquidCount;
    
    
    private void Awake()
    {
        ingredientWrapper = GetComponent<IngredientWrapper>();
        recipientQuantity = ingredientWrapper.RecipientQuantity;

        if (ingredientWrapper.Ingredients == null) liquidCount = 0;
        else
        {
            liquidCount = ingredientWrapper.Ingredients.Count;
            previousLiquidCount = liquidCount;

            for (int i = 0; i < liquidCount; i++)
            {
                AddLiquid(ingredientWrapper.Ingredients[i]);
            }
        }

        UpdateVolumes();
    }

    private void FixedUpdate()
    {
        calculatedTrueFill = 0;
        
        for (int i = 0; i < liquids.Count; i++)
        {
            calculatedTrueFill += liquids[i].trueFill;
            liquids[i].displayedFill = CalculateDisplayedFill(calculatedTrueFill);
            liquidRenderersMaterials[i].SetFloat("_Fill", liquids[i].displayedFill);
        }
    }

    private void OnEnable()
    {
        ingredientWrapper.OnQuantityUpdated += UpdateVolumes;
    }

    private void OnDisable()
    {
        ingredientWrapper.OnQuantityUpdated -= UpdateVolumes;
    }
    
    public void UpdateVolumes()
    {
        liquidCount = ingredientWrapper.Ingredients.Count;

        if (liquidCount != previousLiquidCount)
        {
            RefreshLiquids();
            previousLiquidCount = liquidCount;
        }
        for (int i = 0; i < liquidCount; i++)
        {
            trueFill = ingredientWrapper.Ingredients[i].Quantity / recipientQuantity;

            if (Math.Abs(trueFill - liquids[i].trueFill) > 0.001f)
            {
                UpdateLiquidFill(i, trueFill);
            }
        }
    }

    private void RefreshLiquids()
    {
        for (int i = liquids.Count - 1; i >= 0; i--)
        {
            GameObject liquidToRemove = liquidVolumes[i];
            liquidVolumes.RemoveAt(i);
            liquids.RemoveAt(i);
            Destroy(liquidToRemove);
        }

        if (liquidCount <= 0) return;
        List<Ingredient> ingredients = ingredientWrapper.Ingredients;
        foreach (var t in ingredients) AddLiquid(t);
    }
    
    void UpdateLiquidFill(int liquidNumber, float desiredTrueFill)  // desiredTrueFill entre 0 et 1
    {
        //Material volumeMaterial;

        float previousTotalTrueFill = 0;

        for (int i = 0; i < liquidNumber; i++)
        {
            previousTotalTrueFill += liquids[i].trueFill;
        }
        
        float desiredDisplayedFill = CalculateDisplayedFill(previousTotalTrueFill + desiredTrueFill);
         
        float displayedFillDifference = desiredDisplayedFill - liquids[liquidNumber].displayedFill; //Negative if lowering

        liquids[liquidNumber].trueFill = desiredTrueFill;
        
        for (int i = liquidNumber; i < liquidVolumes.Count; i++)
        {
            //volumeMaterial = liquidVolumes[i].GetComponent<Renderer>().material;
            //volumeMaterial.SetFloat("_Fill",volumeMaterial.GetFloat("_Fill")+displayedFillDifference);
            liquidRenderersMaterials[i].SetFloat("_Fill",liquidRenderersMaterials[i].GetFloat("_Fill")+displayedFillDifference);
            liquids[i].displayedFill += displayedFillDifference;
        }
    }

    void AddLiquid(Ingredient ingredient)
    {
        float trueFill;
        float displayedFill;
        Color liquidColor;
        Color surfaceColor;
        float fresnelPower;
        Color fresnelColor;
        float viscosity;

        trueFill = ingredient.Quantity / recipientQuantity;
        
        float previousTotalTrueFill = 0;

        for (int i = 0; i < liquids.Count; i++)
        {
            previousTotalTrueFill += liquids[i].trueFill;
        }
        
        displayedFill = CalculateDisplayedFill(previousTotalTrueFill + trueFill);
        
        fresnelPower = ingredient.Data.fresnelPower;
        viscosity = ingredient.Data.viscosity;

        liquidColor = ingredient.Color;
        surfaceColor = new Color(
            Mathf.Min(1f, liquidColor.r + 0.05f),
            Mathf.Min(1f, liquidColor.g + 0.05f),
            Mathf.Min(1f, liquidColor.b + 0.05f)
        );
        fresnelColor = new Color(
            Mathf.Min(1f, liquidColor.r + 0.30f),
            Mathf.Min(1f, liquidColor.g + 0.30f),
            Mathf.Min(1f, liquidColor.b + 0.30f)
        );

        int liquidNumber = (liquids.Count);


        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;

        LiquidVisuals newLiquid = newLiquidVolume.AddComponent<LiquidVisuals>();
        newLiquid.trueFill = trueFill;
        newLiquid.displayedFill = displayedFill;
        newLiquid.liquidColor = liquidColor;
        newLiquid.surfaceColor = surfaceColor;
        newLiquid.fresnelPower = fresnelPower;
        newLiquid.fresnelColor = fresnelColor;
        newLiquid.viscosity = viscosity;

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        liquidRendererMaterial.SetFloat("_Fill", displayedFill);
        liquidRendererMaterial.SetColor("_LiquidColor", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColor", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPower", fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColor", fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
        liquidRenderersMaterials.Add(liquidRendererMaterial);
    }

    public float FindTotalTrueFill()
    {
        float totalTrueFill = 0f;

        foreach (LiquidVisuals liquid in liquids)
        {
            totalTrueFill += liquid.trueFill;
        }

        return totalTrueFill;
    }

    public float FindTotalDisplayedFill()
    {
        if (liquidCount <= 0 )
        {
            return displayFillCurve.Evaluate(0f);
        }
        return liquids[liquidCount - 1].displayedFill;
    }

    public float CalculateDisplayedFill(float totalTrueFill)
    {
        
        angleToTop = (FindAngleToTop() - 0.5f) * 2;

        // f(x) = - x * a + b * (1 - abs(x))
        // a étant le dFC et b le hDFC
        float calculatedDisplayedFill = -angleToTop * displayFillCurve.Evaluate(totalTrueFill)
                                        + horizontalDisplayFillCurve.Evaluate(totalTrueFill) * (1 - Mathf.Abs(angleToTop));

        return calculatedDisplayedFill;
    }
    
    public float FindViscosityFactor()
    {
        float returnFactor = 0f;

        for (int i = 0; i < Liquids.Count; i++)
        {
            returnFactor += Liquids[i].viscosity * Liquids[i].trueFill;
        }
        
        returnFactor /= FindTotalTrueFill();
        return returnFactor;
    }

    public float FindAngleToTop() //0 Si vers le haut, 1 si vers le bas
    {
        var axis = Vector3.Cross(Vector3.up, transform.up);
        var flowAngle = Vector3.SignedAngle(Vector3.up, transform.up, axis);
        
        float finalAngle = Mathf.Abs(flowAngle / 180f) ;

        return finalAngle;
    }
    
    void AddRandomLiquid()
    {
        float trueFill;
        float displayedFill;
        Color liquidColor;
        Color surfaceColor;
        float fresnelPower;
        Color fresnelColor;
        float viscosity;

        float previousTotalTrueFill = 0;

        for (int i = 0; i < liquids.Count; i++)
        {
            previousTotalTrueFill += liquids[i].trueFill;
        }
        
        trueFill = Random.Range(0f, 1f - previousTotalTrueFill);

        displayedFill = CalculateDisplayedFill(previousTotalTrueFill + trueFill);

        fresnelPower = Random.Range(4.5f, 5.5f);
        viscosity = Random.Range(0.5f, 1.5f);
        
        liquidColor = new Color(
            (float)Random.Range(0f, 1f),
            (float)Random.Range(0f, 1f),
            (float)Random.Range(0f, 1f)
        );
        surfaceColor = new Color(
            Mathf.Min(1f, liquidColor.r + 0.05f),
            Mathf.Min(1f, liquidColor.g + 0.05f),
            Mathf.Min(1f, liquidColor.b + 0.05f)
        );
        fresnelColor = new Color(
            Mathf.Min(1f, liquidColor.r + 0.30f),
            Mathf.Min(1f, liquidColor.g + 0.30f),
            Mathf.Min(1f, liquidColor.b + 0.30f)
        );

        int liquidNumber = (liquids.Count);

        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;

        LiquidVisuals newLiquid = newLiquidVolume.AddComponent<LiquidVisuals>();
        newLiquid.trueFill = trueFill;
        newLiquid.displayedFill = displayedFill;
        newLiquid.liquidColor = liquidColor;
        newLiquid.surfaceColor = surfaceColor;
        newLiquid.fresnelPower = fresnelPower;
        newLiquid.fresnelColor = fresnelColor;
        newLiquid.viscosity = viscosity;

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        liquidRendererMaterial.SetFloat("_Fill", displayedFill);
        liquidRendererMaterial.SetColor("_LiquidColor", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColor", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPower", fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColor", fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
        liquidRenderersMaterials.Add(liquidRendererMaterial);
    }

    void RemoveLiquid()
    {
        if (liquids.Count >= 1)
        {
            GameObject liquidToRemove = liquidVolumes[^1];
            liquidVolumes.RemoveAt(liquidVolumes.Count - 1);
            liquids.RemoveAt(liquids.Count - 1);
            liquidRenderersMaterials.RemoveAt(liquidRenderersMaterials.Count - 1);
            Destroy(liquidToRemove);
        }
    }
    
    
    void RemoveLiquidAt(int liquidNumber)
    {
        Material volumeMaterial;
        float displayedFillDifference;
        if (liquidNumber == 0)
        {
            displayedFillDifference = liquids[0].displayedFill;
        }
        else
        {
            displayedFillDifference = liquids[liquidNumber].displayedFill - liquids[liquidNumber - 1].displayedFill;
        }

        for (int i = liquidNumber; i < liquidVolumes.Count; i++)
        {
            volumeMaterial = liquidVolumes[i].GetComponent<Renderer>().material;
            volumeMaterial.SetFloat("_Fill", volumeMaterial.GetFloat("_Fill") + displayedFillDifference);
            liquids[i].displayedFill += displayedFillDifference;
        }

        GameObject liquidToRemove = liquidVolumes[liquidNumber];
        liquidVolumes.RemoveAt(liquidNumber);
        liquids.RemoveAt(liquidNumber);
        liquidRenderersMaterials.RemoveAt(liquidNumber);
        Destroy(liquidToRemove);
    }

    //Sert à des tests
    
    private void Update()
    {
        if (Input.GetKeyDown("p") /*|| OVRInput.GetDown(OVRInput.Button.One) */ ) //p for plus
        {
            Debug.Log("Adding Liquid");
            AddRandomLiquid();
            liquidCount += 1;
        }

        if (Input.GetKeyDown("m") /*|| OVRInput.GetDown(OVRInput.Button.Two) */ ) //m for minus
        {
            Debug.Log("Removing Liquid");
            RemoveLiquid();
            liquidCount -= 1;
        }
        
        if (Input.GetKeyDown("r")) //r for refresh
        {
            Debug.Log("Refresh Liquids");
            RefreshLiquids();
        }
        
        /*
        if (Input.GetKeyDown("u")) //u for update
        {
            Debug.Log("Updating Liquids");
            UpdateVolumes();
        }
        

        */
    }
}
