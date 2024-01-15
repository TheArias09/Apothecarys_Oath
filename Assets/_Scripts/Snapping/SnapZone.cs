using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    [SerializeField] List<SnapTag> snapTags = new List<SnapTag>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var snapObject = collision.GetComponentInParent<SnapObject>();

        if(snapObject != null)
        {
            var grabbable = GetComponent<Grabbable>();
            if(!grabbable || !grabbable)

            foreach(var snapTag in snapTags)
            {
                if(snapObject.Contains(snapTag))
                {
                    Snap(collision);
                    break;
                }
            }
        }
    }

    private void Snap(Collider2D collision)
    {
        collision.transform.parent = transform;
        collision.transform.localPosition = Vector3.zero;
        collision.transform.localRotation = Quaternion.identity;
    }
}
