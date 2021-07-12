using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnDistance = 5;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] float raspawnTime = 5.0f;

    private void Awake()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;

        var spawnPoint = transform.position + spawnDirection;
        var enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }
}
