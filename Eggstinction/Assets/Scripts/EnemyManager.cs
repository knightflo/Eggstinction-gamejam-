using Bas.Pennings.DevTools;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class EnemyManager : AbstractSingleton<EnemyManager>
{
    [SerializeField] private float _enemySpawningDelay = 1;

    [Header("References")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyObjectsParent;
    [SerializeField] private SpawnPoint[] _enemySpawnPoints;
    private GameObject[] enemyObjects;

    public void ResetEnemies()
    {
        DestroyEnemies();
        StartEnemySpawning();
    }

    private void Start()
        => StartEnemySpawning();

    private void DestroyEnemies()
    {
        foreach (var enemy in enemyObjects) Destroy(enemy);
    }
    
    public void StartEnemySpawning()
    {
        StartCoroutine(InstantiateEnemiesWithDelay());

        IEnumerator InstantiateEnemiesWithDelay()
        {
            var totalEnemies = _enemySpawnPoints.Sum((sp) => sp.EnemyCount);
            enemyObjects = new GameObject[totalEnemies];

            for (int i = 0; i < _enemySpawnPoints.Length; i++)
            {
                var spawnPoint = _enemySpawnPoints[i];
                for (int j = 0; j < spawnPoint.EnemyCount; j++)
                {
                    enemyObjects[i + j] = InstantiateEnemy(spawnPoint.SpawnPosition);
                    yield return new WaitForSeconds(_enemySpawningDelay);
                }
            }
        }
    }

    private GameObject InstantiateEnemy(Transform transform)
        => Instantiate(
            _enemyPrefab,
            transform.position,
            transform.rotation,
            _enemyObjectsParent.transform);

    [Serializable]
    private struct SpawnPoint
    {
        public string Name;
        public Transform SpawnPosition;

        [Description("The amount of enemies which will spawn at the spawn position")]
        public int EnemyCount;
    }
}
