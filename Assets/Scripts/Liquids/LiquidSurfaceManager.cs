using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make the plane disappear outside of the glass
//Sources :
//https://www.patreon.com/posts/19530112
//https://forum.unity.com/threads/how-to-reveal-parts-of-a-mesh-with-collider.687757/


public class LiquidSurfaceManager : MonoBehaviour
{
    private LiquidVisualsManager liquidVisualsManager;
    private WobbleManager wobbleManager;
    
    [SerializeField] private float wobbleAmountX;
    [SerializeField] private float wobbleAmountZ;

    private float previousDisplayedFill;
    [SerializeField] private float currentDisplayedFill;

    private Vector3 Position;
    //private Vector3 DefaultPosition;
    private Vector3 nextPositon;
    private Quaternion Rotation;
    
    private float rotationZ;
    private float rotationX;
    
    private void Awake()
    {
        var parent = transform.parent;
        liquidVisualsManager = parent.GetComponent<LiquidVisualsManager>();
        wobbleManager = parent.GetComponent<WobbleManager>();

        currentDisplayedFill = liquidVisualsManager.FindTotalDisplayedFill();
        previousDisplayedFill = currentDisplayedFill;
        //DefaultPosition = transform.position;
        
        rotationZ = 0f;
        rotationX = 0f;
    }

    private void Update()
    {
        SetPosition();
        SetRotation();
    }

    private void SetPosition()
    {
        currentDisplayedFill = liquidVisualsManager.FindTotalDisplayedFill();
        
        //previousDisplayedFill = currentDisplayedFill;
        nextPositon = transform.parent.position + new Vector3(0,currentDisplayedFill,0);

        transform.position = nextPositon;

    }

    private void SetRotation()
    {
        wobbleAmountX = wobbleManager.WobbleAmountX;
        wobbleAmountZ = wobbleManager.WobbleAmountZ;
        
        //Setting the desired rotation
        rotationZ = -90f * wobbleAmountZ * Mathf.Deg2Rad;
        rotationX = -90f * wobbleAmountX * Mathf.Deg2Rad;
        
        //rotationZ += 10f * Mathf.Deg2Rad;
        //rotationX += 10f * Mathf.Deg2Rad;
        if (liquidVisualsManager.LiquidCount == 0)
        {
            rotationZ = 0f;
            rotationX = 0f;
        }

        Quaternion q = Quaternion.Euler(rotationZ,0f,rotationX);

        transform.rotation = q;

        /*
        Dans l'ordre :
        
        Ici ils utilisent le position node dans le shader (donc pour chaque vertex)
        a voir si ça cause de la merde plus tard
        
        result1 = RotateAboutAxis(position référentiel objet, axe (010), rotation de 90)
        result1 = (z,y,-x)
        result1.2 = Multiply result1 by wobbleAmountZ
        
        result2 = RotateAboutAxis(position référentiel objet, axe (001), rotation de 90)
        result2 = (-y,x,z)
        result2.2 = Multiply result2 by wobbleAmountX
        
        
        result3 = add result 1.2 and result 2.2
        
        result3 devrait être la rotation par rapport au sol en soit
        
        result 4 = position référentiel monde - position objet (ici théoriquement ça va être 0,0,0 je crois)
        
        result 5 = resulte 3 + result 4
        
        là il split et prend la valeur en G (donc on perdrait tout en X ?!?)
        
        
        */
    }
}
