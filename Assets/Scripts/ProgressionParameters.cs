using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Utilities/ProgressionParameters", fileName = "ProgressionParameters")]
public class ProgressionParameters : ScriptableObject
{
    /// <summary>
    /// Number of clients to end a day.
    /// </summary>
    public int totalClients;

    [Space(10)]

    /// <summary>
    /// Number of the client at which the phase changes.
    /// </summary>
    public List<int> phaseEnd = new ();

    /// <summary>
    /// Index of min and max diseases accessible during each phase.
    /// </summary>
    public List<Vector2Int> diseases = new();

    public List<float> timeBetweenClients = new ();

    public List<float> clientStayTime = new ();
}
