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
    private float _newFill;

    public float RecipientQuantity { get => recipientQuantity;  }


    private void Awake()
    {
        _ingredientWrapper = GetComponent<IngredientWrapper>();
        _ingredientWrapper.SetRecipient(this);

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
    }

    //Sert à des tests
    private void Update()
    {
        if (Input.GetKeyDown("p") || OVRInput.GetDown(OVRInput.Button.One)) //p for plus
        {
            Debug.Log("Adding Liquid");
            AddLiquid();
        }

        if (Input.GetKeyDown("m") || OVRInput.GetDown(OVRInput.Button.Two)) //m for minus
        {
            Debug.Log("Removing Liquid");
            RemoveLiquid();
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
        else
        {
            for (int i = 0; i < potionCount; i++)
            {
                _newFill = (recipientQuantity / _ingredientWrapper.Ingredients[i].Quantity) * 0.2f - 0.1f;

                if (Math.Abs(_newFill - liquids[i].fill) > 0.0001f)
                {
                    UpdateLiquidFill(i, _newFill);
                }
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

        if (potionCount > 0)
        {
            List<Ingredient> ingredients = _ingredientWrapper.Ingredients;

            for (int i = 0; i < ingredients.Count ; i++)
            {
                AddLiquid(ingredients[i]);
            }
        }
    }
    
    void UpdateLiquidFill(int liquidNumber, float desiredFill)
    {
        Material volumeMaterial;
        float fillDifference = (desiredFill * 0.2f - 0.1f) - liquids[liquidNumber].fill; //Negative if lowering

        for (int i = liquidNumber; i < liquidVolumes.Count; i++)
        {
            volumeMaterial = liquidVolumes[i].GetComponent<Renderer>().material;
            volumeMaterial.SetFloat("_FillV4",volumeMaterial.GetFloat("_FillV4")+fillDifference);
            liquids[i].fill += fillDifference;
        }
    }

    void AddLiquid(Ingredient ingredient)
    {
        float fill;
        Color liquidColor;
        Color surfaceColor;
        float fresnelPower;
        Color fresnelColor;

        
        float currentFill = -0.1f;
        if (liquids.Count > 0)
        {
            currentFill = liquids[liquids.Count-1].fill  ;
        }

        fill = currentFill + (ingredient.Quantity / recipientQuantity) * 0.2f - 0.1f ;
        
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

        int liquidNumber = (liquids.Count + 1);


        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;

        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();
        newLiquid.fill = fill;
        newLiquid.liquidColor = liquidColor;
        newLiquid.surfaceColor = surfaceColor;
        newLiquid.fresnelPower = fresnelPower;
        newLiquid.fresnelColor = fresnelColor;

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        liquidRendererMaterial.SetFloat("_FillV4", fill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }
    void AddLiquid()
    {
        float fill;
        Color liquidColor;
        Color surfaceColor;
        float fresnelPower;
        Color fresnelColor;

        /*
        float currentFill = -0.1f;
        if (liquids.Count > 0)
        {
            currentFill = liquids[liquids.Count-1].fill  ;
        }
        currentFill = (currentFill + 0.1f) *5f; //Entre 0 et 1
        
        fill = (currentFill + Random.Range(0.0f, 1f - currentFill)) * 0.2f - 0.1f; //Entre -0.1f et 0.1f

        if (currentFill > 0.9f)
        {
            fill = 0.1f;
        }
        */

        fill = -0.1f;
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

        int liquidNumber = (liquids.Count);


        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;

        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();
        //Liquid newLiquid = Liquid(fill, liquidColor, surfaceColor, fresnelPower, fresnelColor);
        newLiquid.fill = fill;
        newLiquid.liquidColor = liquidColor;
        newLiquid.surfaceColor = surfaceColor;
        newLiquid.fresnelPower = fresnelPower;
        newLiquid.fresnelColor = fresnelColor;

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        //liquidRendererMaterial = new Material(liquidRendererMaterial);
        liquidRendererMaterial.SetFloat("_FillV4", fill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }
    
    void AddLiquid(Liquid addedLiquid)
    {
        float visibleFill = liquids[^1].fill + addedLiquid.fill;

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
        liquidRendererMaterial.SetFloat("_FillV4", visibleFill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", addedLiquid.liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", addedLiquid.surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", addedLiquid.fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", addedLiquid.fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(addedLiquid);
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
    void RemoveLiquidAt(int liquidNumber)
    {
        Material volumeMaterial;
        float fillDifference;
        if (liquidNumber == 0)
        {
            fillDifference = liquids[0].fill;
        }
        else
        {
            fillDifference = liquids[liquidNumber - 1].fill - liquids[liquidNumber - 1].fill;
        }

        for (int i = liquidNumber; i < liquidVolumes.Count; i++)
        {
            volumeMaterial = liquidVolumes[i].GetComponent<Renderer>().material;
            volumeMaterial.SetFloat("_FillV4", volumeMaterial.GetFloat("_FillV4") + fillDifference);
            liquids[i].fill += fillDifference;
        }

        GameObject liquidToRemove = liquidVolumes[liquidNumber];
        liquidVolumes.RemoveAt(liquidNumber);
        liquids.RemoveAt(liquidNumber);
        Destroy(liquidToRemove);
    }
}
