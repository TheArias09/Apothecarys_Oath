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
    private static readonly int FillV4 = Shader.PropertyToID("_FillV4");
    private static readonly int LiquidColorV4 = Shader.PropertyToID("LiquidColorV4");
    private static readonly int SurfaceColorV4 = Shader.PropertyToID("SurfaceColorV4");
    private static readonly int FresnelPowerV4 = Shader.PropertyToID("FresnelPowerV4");
    private static readonly int FresnelColorV4 = Shader.PropertyToID("FresnelColorV4");

    private void Update()
    {
        if (Input.GetKeyDown("p")) //p for plus
        {
            Debug.Log("Adding Liquid");
            AddLiquid();
        }

        if (Input.GetKeyDown("m")) //m for minus
        {
            Debug.Log("Removing Liquid");
            RemoveLiquid();
        }
    }

    void RenderLiquids()
    {
        Liquid l;
        for (int i = 0; i < liquids.Count; i++)
        {
            l = liquids[i];
        }
    }

    // Update is called once per frame
    void AddLiquid()
    {
        float fill;
        Color liquidColor;
        Color surfaceColor;
        float fresnelPower;
        Color fresnelColor;

        float currentFill = 0;
        if (liquids.Count > 0)
        {
            currentFill = (liquids[^1].fill + 0.1f) * 5f ;
        }
        
        fill = (Mathf.Min(1f, currentFill + Random.Range(0.0f, 1 - currentFill))) * 0.2f - 0.1f;

        if (currentFill > 0.9)
        {
            fill = 1f * 0.2f - 0.1f;
        }
        
        
        fresnelPower = Random.Range(4.5f,5.5f);
        
        
        liquidColor = new Color(
            (float)Random.Range(0f, 1f),
            (float)Random.Range(0f, 1f),
            (float)Random.Range(0f, 1f)
        );
        surfaceColor = liquidColor; /*new Color(
            Mathf.Max(1f, liquidColor.r + 0.15f),
            Mathf.Max(1f, liquidColor.g + 0.15f),
            Mathf.Max(1f, liquidColor.b + 0.15f)
        );*/
        fresnelColor = new Color(
            Mathf.Max(1f, liquidColor.r + 0.45f),
            Mathf.Max(1f, liquidColor.g + 0.45f),
            Mathf.Max(1f, liquidColor.b + 0.45f)
        );
        

        /*
        switch (liquids.Count)
        {
            case 0 :
                liquidColor = Color.blue;
                surfaceColor = Color.blue;
                fresnelColor = Color.cyan;
                break;
            case 1 :
                liquidColor = Color.red;
                surfaceColor = Color.red;
                fresnelColor = Color.magenta;
                break;
            case 2 :
                liquidColor = Color.green;
                surfaceColor = Color.green;
                fresnelColor = Color.yellow;
                break;
            default:
                liquidColor = Color.black;
                surfaceColor = Color.gray;
                fresnelColor = Color.white;
                break;
        }
        */

        int liquidNumber = (liquids.Count + 1);


        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;
        
        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        //liquidRendererMaterial = new Material(liquidRendererMaterial);
        liquidRendererMaterial.SetFloat("_FillV4", fill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4",fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4",fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }
    
    void AddLiquid(Liquid addedLiquid)
    {
        int liquidNumber = (liquids.Count + 1);


        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);
        newLiquidVolume.transform.localScale -=
            new Vector3(liquidNumber / 1000f, liquidNumber / 1000f, liquidNumber / 1000f);

        newLiquidVolume.name = "Liquid" + liquidNumber;
        
        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        //liquidRendererMaterial = new Material(liquidRendererMaterial);
        liquidRendererMaterial.SetFloat("_FillV4", addedLiquid.fill);
        liquidRendererMaterial.SetColor("_LiquidColorV4", addedLiquid.liquidColor);
        liquidRendererMaterial.SetColor("_SurfaceColorV4", addedLiquid.surfaceColor);
        liquidRendererMaterial.SetFloat("_FresnelPowerV4", addedLiquid.fresnelPower);
        liquidRendererMaterial.SetColor("_FresnelColorV4", addedLiquid.fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }

    void RemoveLiquid()
    {
        if (liquids.Count >= 1)
        {
            GameObject liquidToRemove = liquidVolumes[^1];
            liquidVolumes.RemoveAt(liquidVolumes.Count - 1);
            liquids.RemoveAt(liquids.Count - 1);
            Destroy(liquidToRemove);
        }
    }
}
