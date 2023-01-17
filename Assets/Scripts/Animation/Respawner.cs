using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawner : MonoBehaviour
{
    [SerializeField] Transform respawnTarget;
    [SerializeField] BoxCollider respawnTargetBox;
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

    public void Init(BoxCollider boxCollider)
    {
        respawnTargetBox = boxCollider;
    }

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
        StartCoroutine(SpawnAndRespawnCoroutine());
    }

    private IEnumerator SpawnAndRespawnCoroutine()
    {
        OnDespawnStart?.Invoke();
        yield return new WaitForSeconds(despawnTime);
        OnDespawnEnd?.Invoke();


        var position = respawnTarget != null ? respawnTarget.position : RandomPointInBounds(respawnTargetBox.bounds);
        ultimateParentTransform.position = position;
        ultimateParentTransform.eulerAngles = respawnEulerAngles;

        OnRespawnStart?.Invoke();
        yield return new WaitForSeconds(respawnTime);
        OnRespawnEnd?.Invoke();
    }

    public void StartRespawnCoroutine()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        OnRespawnStart?.Invoke();
        yield return new WaitForSeconds(respawnTime);
        OnRespawnEnd?.Invoke();
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
