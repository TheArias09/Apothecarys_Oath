using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cuttable : MonoBehaviour
{
    [SerializeField] List<Transform> cuttedPieces;
    [SerializeField] UnityEvent OnCut;

    [SerializeField] GameObject ultimateParentGameObject;

    [SerializeField] Respawner respawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Cutter")) return;

        OnCut?.Invoke();
        foreach(var child in cuttedPieces)
        {
            child.gameObject.SetActive(true);
            child.SetParent(null);
            if(respawner.RespawnTargetBox == null)
            {
                Debug.LogError("Respawner should have a respawnTargetBox");
            }
            var childRespawner = child.GetComponentInChildren<Respawner>();
            if (childRespawner != null)
            {
                childRespawner.Init(respawner.RespawnTargetBox);
            }
        }

        Destroy(ultimateParentGameObject);
    }
}