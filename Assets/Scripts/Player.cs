using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _lives;
    [SerializeField] private float _speed;

    [Header("Firing Variables")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private float _fireRate;
    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    private float _speedMultiplier = 2;
    private bool _isSpeedBoostActive;
    private bool _isTripleShotActive;
    private bool _isShieldActive;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngineDestroyedVisualizer, _rightEngineDestroyedVisualizer;
    [SerializeField] private int _score;
    private UIManager _uiManager;
    [SerializeField] private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        transform.position = new Vector3(0, -2, 0);
        _audioSource.clip = _laserSoundClip;
    }

    private void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireProjectile();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        MovementConstraints();
    }

    private void MovementConstraints()
    {
        float yMinLimit = -3.8f;
        float yMaxLimit = 0f;

        float xMinLimit = -11.3f;
        float xMaxLimit = 11.3f;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, yMinLimit, yMaxLimit), 0);

        if (transform.position.x > xMaxLimit)
        {
            transform.position = new Vector3(xMinLimit, transform.position.y, 0);
        }
        else if (transform.position.x < xMinLimit)
        {
            transform.position = new Vector3(xMaxLimit, transform.position.y, 0);
        }
    }

    private void FireProjectile()
    {
        Vector3 spawnOffset = new Vector3(0, 0.75f, 0);

        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

        }
        else if (!_isTripleShotActive)
        {
            Instantiate(_projectilePrefab, transform.position + spawnOffset, Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void TakeDamage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftEngineDestroyedVisualizer.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngineDestroyedVisualizer.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownCoroutine());
    }

    private IEnumerator TripleShotPowerDownCoroutine()
    {
        float activeDuration = 5.0f;

        yield return new WaitForSeconds(activeDuration);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownCoroutine());
    }

    private IEnumerator SpeedBoostPowerDownCoroutine()
    {
        float activeDuration = 5.0f;

        yield return new WaitForSeconds(activeDuration);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}