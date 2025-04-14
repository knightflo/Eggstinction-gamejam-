using Bas.Pennings.DevTools;
using System;
using System.ComponentModel;
using UnityEngine;

public class EnemyManager : AbstractSingleton<EnemyManager>
{
    [Header("References")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyObjectsParent;
    [SerializeField] private SpawnPoint[] _enemySpawnPoints;
    private GameObject[] enemyObjects;

    public void ResetEnemies()
    {
        DestroyEnemies();
        InstantiateEnemies();
    }

    private void Start()
        => enemyObjects = InstantiateEnemies();

    private void DestroyEnemies()
    {
        foreach (var enemy in enemyObjects) Destroy(enemy);
    }

    private GameObject[] InstantiateEnemies()
    {
        var enemies = new GameObject[_enemySpawnPoints.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            var spawnPoint = _enemySpawnPoints[i];
            for (int j = 0; j < spawnPoint.EnemyCount; j++)
                enemies[i + j] = InstantiateEnemy(spawnPoint.SpawnPosition);
        }

        return enemies;
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
        public Transform SpawnPosition;

        [Description("The amount of enemies which will spawn at the spawn position")]
        public int EnemyCount;
    }
}
