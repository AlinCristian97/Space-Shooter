﻿using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyHolder;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _timeBetweenSpawns;

    [Header("Powerups")]
    [SerializeField] private GameObject[] _powerup;
    private bool _stopSpawning;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnPowerupCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        float startDelay = 3f;
        yield return new WaitForSeconds(startDelay);

        while (!_stopSpawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 positionToSpawn = new Vector3(randomX, 7f, 0f);

            GameObject newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyHolder.transform;

            yield return new WaitForSeconds(_timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnPowerupCoroutine()
    {
        float startDelay = 3f;
        yield return new WaitForSeconds(startDelay);

        while (!_stopSpawning)
        {
            int randomPowerupIndex = Random.Range(0, _powerup.Length);
            float randomX = Random.Range(-8f, 8f);
            Vector3 positionToSpawn = new Vector3(randomX, 7f, 0f);
            float randomTimeBetweenSpawns = Random.Range(3, 8);

            yield return new WaitForSeconds(randomTimeBetweenSpawns);

            Instantiate(_powerup[randomPowerupIndex], positionToSpawn, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}