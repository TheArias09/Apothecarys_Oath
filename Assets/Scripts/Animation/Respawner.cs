using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawner : MonoBehaviour
{
    [SerializeField] Transform respawnTarget;
    [SerializeField] Transform ultimateParentTransform;
    [SerializeField] Vector3 respawnEulerAngles = Vector3.zero;

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
        if (other.gameObject.layer != LayerMask.NameToLayer("Respawn Area")) return;

        colliderCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Respawn Area")) return;

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

        ultimateParentTransform.position = respawnTarget.position;
        ultimateParentTransform.eulerAngles = respawnEulerAngles;

        OnRespawnStart?.Invoke();
        yield return new WaitForSeconds(respawnTime);
        OnRespawnEnd?.Invoke();
    }
}
