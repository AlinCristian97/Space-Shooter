using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speedRotation;
    [SerializeField] private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Awake()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * _speedRotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.25f);
        }
    }
}