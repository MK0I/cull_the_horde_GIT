using System.Collections;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy Variants")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Interval (seconds)")]
    [SerializeField] private float minInterval = 2f;
    [SerializeField] private float maxInterval = 5f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);

            SpawnOneEnemy();
        }
    }

    private void SpawnOneEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning($"{name}: No Enemy Prefabs assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"{name}: No Spawn Points assigned.");
            return;
        }

        // Randomizer
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        if (point != null && prefab != null)
            Instantiate(prefab, point.position, Quaternion.identity);
    }

    // Stopper
    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
