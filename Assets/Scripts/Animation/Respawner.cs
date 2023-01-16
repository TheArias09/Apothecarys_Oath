using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawner : MonoBehaviour
{
    [SerializeField] Transform respawnTarget;
    [SerializeField] Transform respawnPoint;

    [SerializeField] float collidingTimeBeforeRespawn;
    [SerializeField] float collidingTimeBeforeRespawnClock;
    [SerializeField] int colliderCount;

    [SerializeField] float despawnTime;
    [SerializeField] float respawnTime;

    [SerializeField] UnityEvent OnDespawnStart;
    [SerializeField] UnityEvent OnDespawnEnd;
    [SerializeField] UnityEvent OnRespawnStart;
    [SerializeField] UnityEvent OnRespawnEnd;

    private void OnTriggerEnter(Collider other)
    {
        colliderCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        colliderCount--;
    }

    private void Update()
    {
        if (colliderCount <= 0)
        {
            collidingTimeBeforeRespawnClock = 0f;
            return;
        }

        collidingTimeBeforeRespawnClock += Time.deltaTime;

        if (collidingTimeBeforeRespawnClock < collidingTimeBeforeRespawn) return;

        collidingTimeBeforeRespawnClock = 0f;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        OnDespawnStart?.Invoke();
        yield return new WaitForSeconds(despawnTime);
        OnDespawnEnd?.Invoke();

        respawnPoint.position = respawnTarget.position;

        OnRespawnStart?.Invoke();
        yield return new WaitForSeconds(respawnTime);
        OnRespawnEnd?.Invoke();
    }
}
