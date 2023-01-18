using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenewableObjectSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> objects;
    [SerializeField] GameObject objectPrefab;
    [SerializeField] BoxCollider box;
    [SerializeField] int maxCount;

    [SerializeField] float maxTimeBetweenSpawn = .1f;
    private float betweenSpawnClock = 0f;

    private void Update()
    {
        IncreaseClock();
        RemoveDestroyedObjects();
        SpawnIfNeeded();
    }

    private void SpawnIfNeeded()
    {
        if (objects.Count < maxCount && betweenSpawnClock == maxTimeBetweenSpawn)
        {
            betweenSpawnClock = 0f;

            var position = RandomPointInBounds(box.bounds);
            var objectInstance = Instantiate(objectPrefab, position, Quaternion.identity);

            var respawn = objectInstance.GetComponentInChildren<Respawner>();
            if(respawn)
            {
                respawn.Init(box);
                respawn.StartRespawnCoroutine();
            }

            foreach(var trackedGameObject in respawn.TrackedGameObjects)
            {
                objects.Add(trackedGameObject);
            }
        }
    }

    private void RemoveDestroyedObjects()
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i] == null)
            {
                objects.RemoveAt(i);
            }
        }
    }

    private void IncreaseClock()
    {
        betweenSpawnClock += Time.deltaTime;
        if (betweenSpawnClock > maxTimeBetweenSpawn)
            betweenSpawnClock = maxTimeBetweenSpawn;
    }

    private Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
