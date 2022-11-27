using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBehavior : MonoBehaviour
{
    public Client Client { get; set; }

    public float BirthTime { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        BirthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
