using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectReferenceAssigner : MonoBehaviour
{
    [SerializeField] GameObjectReference gameObjectReference;

    private void Awake()
    {
        gameObjectReference.gameObject = gameObject;
    }
}
