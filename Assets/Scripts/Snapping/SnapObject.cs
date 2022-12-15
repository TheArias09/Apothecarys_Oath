using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapObject : MonoBehaviour
{
    [SerializeField] List<SnapTag> snapTags = new List<SnapTag>();
    
    public bool Contains(SnapTag snapTag)
    {
        return snapTags.Contains(snapTag);
    }
}
