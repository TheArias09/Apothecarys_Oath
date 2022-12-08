using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LiquidManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> liquidVolumes;
    [SerializeField] private List<Liquid> liquids;

    [SerializeField] private GameObject liquidVolume;

    [SerializeField] private float recipientQuantity;
    [SerializeField] private int potionCount;

    private IngredientWrapper _ingredientWrapper;
    private int _previousPotionCount;
    private float _trueFill; //Entre 0 et 1
    private float _displayedFill; //Entre -0.1 et 0.1
    
    public float RecipientQuantity { get => recipientQuantity;  }

    private void Awake()
    {
        _ingredientWrapper = GetComponent<IngredientWrapper>();
        recipientQuantity = _ingredientWrapper.RecipientQuantity;

        if (_ingredientWrapper.Ingredients == null) potionCount = 0;
        else
        {
            potionCount = _ingredientWrapper.Ingredients.Count;
            _previousPotionCount = potionCount;

            for (int i = 0; i < potionCount; i++)
            {
                AddLiquid(_ingredientWrapper.Ingredients[i]);
            }
        }

        UpdateVolumes();
    }

    private void OnEnable()
    {
        _ingredientWrapper.OnQuantityUpdated += UpdateVolumes;
    }

    private void OnDisable()
    {
        _ingredientWrapper.OnQuantityUpdated -= UpdateVolumes;
    }

    //Sert à des tests
    private void Update()
    {
        if (Input.GetKeyDown("p") || OVRInput.GetDown(OVRInput.Button.One)) //p for plus
        {
            Debug.Log("Adding Liquid");
            AddRandomLiquid();
            potionCount += 1;
        }

        if (Input.GetKeyDown("m") || OVRInput.GetDown(OVRInput.Button.Two)) //m for minus
        {
            Debug.Log("Removing Liquid");
            RemoveLiquid();
            potionCount -= 1;
        }
        
        if (Input.GetKeyDown("u")) //u for update
        {
            Debug.Log("Updating Liquids");
            UpdateVolumes();
        }
        
        if (Input.GetKeyDown("r")) //r for refresh
        {
            Debug.Log("Refresh Liquids");
            RefreshLiquids();
        }
    }


    public void UpdateVolumes()
    {
        potionCount = _ingredientWrapper.Ingredients.Count;

        if (potionCount != _previousPotionCount)
        {
            RefreshLiquids();
            _previousPotionCount = potionCount;
        }
        for (int i = 0; i < potionCount; i++)
        {
            _trueFill = _ingredientWrapper.Ingredients[i].Quantity / recipientQuantity;

            if (Math.Abs(_trueFill - liquids[i].trueFill) > 0.001f)
            {
                UpdateLiquidFill(i, _trueFill);
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

        if (potionCount <= 0) return;
        List<Ingredient> ingredients = _ingredientWrapper.Ingredients;
        foreach (var t in ingredients) AddLiquid(t);
    }
    
    void UpdateLiquidFill(int liquidNumber, float desiredTrueFill)  // desiredTrueFill entre 0 et 1
    {
        Material volumeMaterial;

        float previousDisplayedFill = -0.1f;

        if (liquidNumber > 0)
        {
            previousDisplayedFill = liquids[liquidNumber - 1].displayedFill;
        }
        
        float desiredDisplayedFill = previousDisplayedFill + desiredTrueFill * 0.2f;
        
        float displayedFillDifference = desiredDisplayedFill - liquids[liquidNumber].displayedFill; //Negative if lowering
        //float fillDifference = (desiredFill * 0.2f - 0.1f) - liquids[liquidNumber].fill; //Negative if lowering

        for (int i = liquidNumber; i < liquidVolumes.Count; i++)
        {
            volumeMaterial = liquidVolumes[i].GetComponent<Renderer>().material;
            volumeMaterial.SetFloat("_FillV4",volumeMaterial.GetFloat("_FillV4")+displayedFillDifference);
            liquids[i].trueFill = desiredTrueFill;
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

        trueFill = ingredient.Quantity / recipientQuantity;
        
        float previousDisplayedFill = -0.1f;
        if (liquids.Count > 0)
        {
            previousDisplayedFill = liquids[liquids.Count-1].displayedFill  ;
        }

        displayedFill = previousDisplayedFill + (trueFill * 0.2f) ;
        
        fresnelPower = Random.Range(4f, 6f);
        
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

        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();
        newLiquid.trueFill = trueFill;
        newLiquid.displayedFill = displayedFill;
        newLiquid.liquidColor = liquidColor;
        newLiquid.surfaceColor = surfaceColor;
        newLiquid.fresnelPower = fresnelPower;
        newLiquid.fresnelColor = fresnelColor;

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        liquidRendererMaterial.SetFloat("_FillV4", displayedFill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }
    
    
    void AddRandomLiquid()
    {
        float trueFill;
        float displayedFill;
        Color liquidColor;
        Color surfaceColor;
        float fresnelPower;
        Color fresnelColor;

        float previousDisplayedFill = -0.1f;
        if (liquids.Count > 0)
        {
            previousDisplayedFill = liquids[liquids.Count-1].displayedFill  ;
        }

        float PreviousTotalTrueFill = (previousDisplayedFill + 0.1f) * 5;

        trueFill = Random.Range(0f, 1f - PreviousTotalTrueFill);

        displayedFill = previousDisplayedFill + trueFill * 0.2f;
        

        fresnelPower = Random.Range(4.5f, 5.5f);
        
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

        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();
        //Liquid newLiquid = Liquid(fill, liquidColor, surfaceColor, fresnelPower, fresnelColor);
        newLiquid.trueFill = trueFill;
        newLiquid.displayedFill = displayedFill;
        newLiquid.liquidColor = liquidColor;
        newLiquid.surfaceColor = surfaceColor;
        newLiquid.fresnelPower = fresnelPower;
        newLiquid.fresnelColor = fresnelColor;

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        //liquidRendererMaterial = new Material(liquidRendererMaterial);
        liquidRendererMaterial.SetFloat("_FillV4", displayedFill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }

    void RemoveLiquid()
    {
        if (liquids.Count >= 1)
        {
            GameObject liquidToRemove = liquidVolumes[liquidVolumes.Count - 1];
            liquidVolumes.RemoveAt(liquidVolumes.Count - 1);
            liquids.RemoveAt(liquids.Count - 1);
            Destroy(liquidToRemove);
        }
    }
    
    /*
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
            volumeMaterial.SetFloat("_FillV4", volumeMaterial.GetFloat("_FillV4") + displayedFillDifference);
            liquids[i].displayedFill += displayedFillDifference;
        }

        GameObject liquidToRemove = liquidVolumes[liquidNumber];
        liquidVolumes.RemoveAt(liquidNumber);
        liquids.RemoveAt(liquidNumber);
        Destroy(liquidToRemove);
    }
    
    void AddLiquid()
    {
        float trueFill;
        float displayedFill;
        Color liquidColor;
        Color surfaceColor;
        float fresnelPower;
        Color fresnelColor;

        trueFill = 0;

        float previousdisplayedFill = -0.1f;
        if (liquids.Count > 0)
        {
            previousdisplayedFill = liquids[liquids.Count-1].displayedFill  ;
        }

        displayedFill = previousdisplayedFill + (trueFill * 0.2f) ;
        
        fresnelPower = Random.Range(4.5f, 5.5f);
        liquidColor = Color.white;
        surfaceColor = Color.white;
        fresnelColor = Color.white;

        /*
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
        */
/*

        int liquidNumber = (liquids.Count);


        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;

        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();
        //Liquid newLiquid = Liquid(fill, liquidColor, surfaceColor, fresnelPower, fresnelColor);
        newLiquid.trueFill = trueFill;
        newLiquid.displayedFill = displayedFill;
        newLiquid.liquidColor = liquidColor;
        newLiquid.surfaceColor = surfaceColor;
        newLiquid.fresnelPower = fresnelPower;
        newLiquid.fresnelColor = fresnelColor;

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        //liquidRendererMaterial = new Material(liquidRendererMaterial);
        liquidRendererMaterial.SetFloat("_FillV4", displayedFill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }
    
    void AddLiquid(Liquid addedLiquid)
    {
        float trueFill = addedLiquid.trueFill;
        float displayedFill = liquids[^1].displayedFill + trueFill * 0.2f;
        addedLiquid.displayedFill = displayedFill;

        int liquidNumber = (liquids.Count);

        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;

        //Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        //liquidRendererMaterial = new Material(liquidRendererMaterial);
        liquidRendererMaterial.SetFloat("_FillV4", displayedFill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", addedLiquid.liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", addedLiquid.surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", addedLiquid.fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", addedLiquid.fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(addedLiquid);
    }
    */
    
}
