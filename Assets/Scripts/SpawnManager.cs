using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour, IObserver
{
    private ISubject _player;

    [SerializeField] private GameObject _enemyHolder;
    [SerializeField] private List<GameObject> _enemyPrefabs = new List<GameObject>();
    [SerializeField] private float _timeBetweenSpawns;

    [Header("Powerups")]
    [SerializeField] private GameObject[] _powerup;
    private bool _stopSpawning;

    public void GetNotified()
    {
        if ((_player as Player).IsAlive == false)
        {
            StopSpawiningOnPlayerDeath();
        }
    }

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _player.AddObserver(this);
    }

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
            int randomEnemyIndex = Random.Range(0, _enemyPrefabs.Count);
            float randomX = Random.Range(-8f, 8f);
            Vector3 positionToSpawn = new Vector3(randomX, 7f, 0f);

            GameObject newEnemy = Instantiate(_enemyPrefabs[randomEnemyIndex], positionToSpawn, Quaternion.identity);
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

    public void StopSpawiningOnPlayerDeath()
    {
        _stopSpawning = true;
    }
}