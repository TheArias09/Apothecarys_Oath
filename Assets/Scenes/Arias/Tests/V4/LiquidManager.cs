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
        
        fresnelPower = 5f;
        
        /*
        LiquidColor = new Color(
            (float)Random.Range(0, 255),
            (float)Random.Range(0, 255),
            (float)Random.Range(0, 255)
        );
        SurfaceColor = new Color(
            Mathf.Max(255f, LiquidColor.r + 40),
            Mathf.Max(255f, LiquidColor.g + 40),
            Mathf.Max(255f, LiquidColor.b + 40)
        );
        FresnelColor = new Color(
            Mathf.Max(255f, LiquidColor.r + 100),
            Mathf.Max(255f, LiquidColor.g + 100),
            Mathf.Max(255f, LiquidColor.b + 100)
        );
        */

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


        GameObject newLiquidVolume = Instantiate(liquidVolume, transform) as GameObject;
        newLiquidVolume.transform.parent = gameObject.transform;
        newLiquidVolume.transform.position += new Vector3(0f, 0f, 0.00025f);

        newLiquidVolume.name = "Liquid" + (liquids.Count + 1);
        
        Liquid newLiquid = newLiquidVolume.AddComponent<Liquid>();

        Material liquidRendererMaterial = newLiquidVolume.GetComponent<Renderer>().material;

        //liquidRendererMaterial = new Material(liquidRendererMaterial);
        liquidRendererMaterial.SetFloat(FillV4, fill);
        liquidRendererMaterial.SetColor(LiquidColorV4, liquidColor);
        liquidRendererMaterial.SetColor(SurfaceColorV4, surfaceColor);
        liquidRendererMaterial.SetFloat(FresnelPowerV4,fresnelPower);
        liquidRendererMaterial.SetColor(FresnelColorV4,fresnelColor);

        liquidVolumes.Add(newLiquidVolume);
        liquids.Add(newLiquid);
    }

    void SyncLiquids()
    {
        
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
