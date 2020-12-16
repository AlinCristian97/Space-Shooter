using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //protected IShootBehaviour _shootBehaviour;

    //TODO: Implement scriptableobject configs
    [SerializeField] private EnemySO _enemyConfig;
    [SerializeField] private GameObject _laserPrefab;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _canFire = -1;
    private float _speed;
    private float _fireRate;

    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _speed = _enemyConfig.speed;
        _fireRate = _enemyConfig.fireRate;
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            RandomRespawn();
        }
    }

    private void RandomRespawn()
    {
        float randomX = Random.Range(-8f, 8f);
        Vector3 respawnLocation = new Vector3(randomX, 7f, 0);

        transform.position = respawnLocation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage();
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);
        }
    }
}