using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] float spawnRate = 2;
    [SerializeField] int spawnAmount = 1;
    [SerializeField] float spawnDistance = 15;
    [SerializeField] float trajectoryVariant = 15;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;

            var spawnPoint = transform.position + spawnDirection;

            var variance = Random.Range(-trajectoryVariant, trajectoryVariant);
            var rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            var asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
