using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy Variants")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        SpawnEnemies(); // Debug.Log($"{name}: Spawning Enemy.");

    }

    public void SpawnEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning($"{name}: No Enemy Prefabs assigned.");
            return;
        }

        foreach (Transform point in spawnPoints)
        {
            if (point == null)
                continue;

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            Instantiate(enemyPrefab, point.position, Quaternion.identity);
        }
    }
}