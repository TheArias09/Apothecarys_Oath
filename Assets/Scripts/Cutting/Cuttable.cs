using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cuttable : MonoBehaviour
{
    [SerializeField] List<Transform> cuttedPieces;
    [SerializeField] UnityEvent OnCut;

    [SerializeField] GameObject ultimateParentGameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Cutter")) return;

        OnCut?.Invoke();
        foreach(var child in cuttedPieces)
        {
            child.gameObject.SetActive(true);
            child.SetParent(null);
        }

        Destroy(ultimateParentGameObject);
    }
}