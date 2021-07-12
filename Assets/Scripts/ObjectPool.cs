using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;
    public int poolSize = 5;
    [SerializeField] float spawnTimer = 1f;
    [SerializeField] float spawnDistance = 15;
    [SerializeField] float trajectoryVariant = 15;

    Asteroid[] pool;

    private void Awake()
    {
        PopulatePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }

    void PopulatePool()
    {
        pool = new Asteroid[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;

            var spawnPoint = transform.position + spawnDirection;

            var variance = Random.Range(-trajectoryVariant, trajectoryVariant);
            var rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            pool[i] = Instantiate(asteroidPrefab, spawnPoint, rotation);
            pool[i].transform.SetParent(transform);
            pool[i].gameObject.SetActive(false);

            pool[i].size = Random.Range(pool[i].minSize, pool[i].maxSize);
            pool[i].SetTrajectory(rotation * -spawnDirection);

        }
    }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].gameObject.activeInHierarchy == false)
            {
                var asteroid = pool[i];
                asteroid.gameObject.SetActive(true);
                ReSpawnAsteroid(i);
                return;
            }
        }
    }

    private void ReSpawnAsteroid(int i)
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        var variance = Random.Range(-trajectoryVariant, trajectoryVariant);
        var rotation = Quaternion.AngleAxis(variance, Vector3.forward);

        pool[i].SetTrajectory(rotation * -spawnDirection);
    }
}
