using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBehavior : MonoBehaviour
{
    public Client Client { get; set; }

    public float BirthTime { get; private set; }
    public float StayTime { get; set; }

    private bool hasLeft = false;
    
    // Start is called before the first frame update
    void Start()
    {
        BirthTime = Time.time;
        Debug.Log(Client.Name + "has arrived ! He has " + Client.Disease.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > BirthTime + StayTime || !hasLeft) Leave(true);
    }

    public void ReceivePotion(Ingredient potion)
    {
        if (potion.Cures != null && potion.Cures == Client.Disease)
        {
            Debug.Log(Client.Name + " was cured correclty!");
            GameManager.Instance.AddScore(potion.Quality);
        }
        else
        {
            Debug.Log(Client.Name + " was not given the correct potion...");
            GameManager.Instance.AddError();
        }

        Leave(false);
    }

    private void Leave(bool timeout)
    {
        hasLeft = true;

        if (timeout)
        {
            Debug.Log(Client.Name + " has had enough, he left.");
            GameManager.Instance.AddError();
        }
        else
        {
            Debug.Log(Client.Name + " left!");
        }

        Destroy(this);
    }
}
