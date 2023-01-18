using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Respawner : MonoBehaviour
{
    [SerializeField] Transform respawnTarget;
    [SerializeField] BoxCollider respawnTargetBox;
    [SerializeField] Transform ultimateParentTransform;
    [SerializeField] Rigidbody body;
    [SerializeField] Vector3 respawnEulerAngles = Vector3.zero;
    [SerializeField] float yThreshold = -50f;

    [SerializeField] float collidingTimeBeforeRespawn;
    [SerializeField] float collidingTimeBeforeRespawnClock;
    [SerializeField] int colliderCount;

    [SerializeField] float despawnTime;
    [SerializeField] float respawnTime;

    [SerializeField] UnityEvent OnDespawnStart;
    [SerializeField] UnityEvent OnDespawnEnd;
    [SerializeField] UnityEvent OnRespawnStart;
    [SerializeField] UnityEvent OnRespawnEnd;

    [SerializeField] List<GameObject> trackedGameObjects;
    public List<GameObject> TrackedGameObjects => trackedGameObjects;

    private bool coroutineLock = false;


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
        if(transform.position.y < yThreshold)
        {
            if(!coroutineLock) StartCoroutine(SpawnAndRespawnCoroutine());
        }

        if (colliderCount <= 0)
        {
            collidingTimeBeforeRespawnClock = 0f;
            return;
        }

        collidingTimeBeforeRespawnClock += Time.deltaTime;

        if (collidingTimeBeforeRespawnClock < collidingTimeBeforeRespawn) return;


        if (coroutineLock) return;

        collidingTimeBeforeRespawnClock = 0f;
        if (!coroutineLock) StartCoroutine(SpawnAndRespawnCoroutine());
    }

    private IEnumerator SpawnAndRespawnCoroutine()
    {
        coroutineLock = true;

        OnDespawnStart?.Invoke();
        yield return new WaitForSeconds(despawnTime);
        OnDespawnEnd?.Invoke();


        var position = respawnTarget != null ? respawnTarget.position : RandomPointInBounds(respawnTargetBox.bounds);
        ultimateParentTransform.position = position;
        ultimateParentTransform.eulerAngles = respawnEulerAngles;
        body.velocity = Vector3.zero;

        OnRespawnStart?.Invoke();
        yield return new WaitForSeconds(respawnTime);
        OnRespawnEnd?.Invoke();

        coroutineLock = false;
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
