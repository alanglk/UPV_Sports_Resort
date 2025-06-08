using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("What to Spawn")]
    [Tooltip("The prefab to spawn")]
    public GameObject basketballPrefab;

    [Header("Spawn Timing")]
    [Tooltip("Seconds between each individual spawn (ball by ball)")]
    public float spawnInterval = 1f;

    [Header("Spawn Settings")]
    [Tooltip("Maximum total number of balls to spawn in this session")]    
    public int maxSpawnCount = 4;

    [Tooltip("Offset from this object's position (X, Y, Z)")]
    public Vector3 spawnOffset = new Vector3(0f, 1f, 0f);

    [Header("Lifetime Settings")]
    [Tooltip("How long (in seconds) each spawned ball remains before being destroyed")]
    public float objectLifetime = 10f;

    // Tracks how many balls have been spawned so far
    private int currentSpawnCount = 0;

    void Start()
    {
        // Only start spawning if prefab is set and maxSpawnCount > 0
        if (basketballPrefab != null && maxSpawnCount > 0)
        {
            currentSpawnCount = 0;
            InvokeRepeating(nameof(SpawnWave), 0f, spawnInterval);
        }
    }

    void SpawnWave()
    {
        // If we've reached the max number, stop repeating
        if (currentSpawnCount >= maxSpawnCount)
        {
            CancelInvoke(nameof(SpawnWave));
            return;
        }

        // Spawn one ball
        Vector3 spawnPos = transform.position + spawnOffset;
        GameObject ball = Instantiate(basketballPrefab, spawnPos, Quaternion.identity);

        // Schedule it for destruction after objectLifetime seconds
        if (objectLifetime > 0f)
        {
            Destroy(ball, objectLifetime);
        }

        currentSpawnCount++;
    }

}