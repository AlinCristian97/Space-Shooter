using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private int _lives;
    [SerializeField] private float _speed;
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputMovement;

    public bool IsAlive { get => Lives > 0; }

    [Header("Shooting Variables")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _fireRate;
    private float _canFire = -1f;
    private bool _isFiring;

    [Header("Powerups")]
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngineDestroyedVisualizer, _rightEngineDestroyedVisualizer;
    private float _speedMultiplier = 2;
    private bool _isSpeedBoostActive;
    private bool _isTripleShotActive;
    private bool _isShieldActive;

    [SerializeField] private int _score;

    [Header("Audio")]
    [SerializeField] private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    public int Score { get => _score; private set => _score = value; }
    public int Lives { get => _lives; private set => _lives = value; }

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    public void NotifyObservers()
    {
        if (_observers.Count > 0)
        {
            foreach (IObserver observer in _observers)
            {
                observer.GetNotified();
            }
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerInputActions = new PlayerInputActions();
    }


    private void Start()
    {
        transform.position = new Vector3(0, -2, 0);
        _audioSource.clip = _laserSoundClip;        
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
        _inputReader.attackEvent += OnAttack;
        _inputReader.attackCanceledEvent += OnAttackCanceled;
        _inputReader.moveEvent += OnMove;

        //TODO: Restart level
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    private void Update()
    {
        HandleMovement();

        CheckIfFiring();
    }

    private void CheckIfFiring()
    {
        if (Mouse.current.leftButton.isPressed || Keyboard.current[Key.Space].isPressed) //this is bullcrap workaround.
        {
            _isFiring = true;
        }
    }

    private void HandleMovement()
    {
        transform.Translate(_inputMovement * _speed * Time.deltaTime);

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

    private IEnumerator FireProjectile()
    {
        Vector3 spawnOffset = new Vector3(0, 0.75f, 0);
        Debug.Log("Started Coroutine.");

        while (_isFiring)
        {
            if (Time.time > _canFire)
            {
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
            yield return new WaitForSeconds(_fireRate);
        }
    }

    public void TakeDamage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        Lives--;

        if (Lives == 2)
        {
            _leftEngineDestroyedVisualizer.SetActive(true);
        }
        else if (Lives == 1)
        {
            _rightEngineDestroyedVisualizer.SetActive(true);
        }

        NotifyObservers();

        if (!IsAlive)
        {
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
        Score += points;
        NotifyObservers();
    }


    //Event Listeners
    public void OnAttack()
    {
        _isFiring = true;
        StartCoroutine(FireProjectile());
    }

    public void OnAttackCanceled()
    {
        _isFiring = false;
    }

    private void OnMove(Vector2 movement)
    {
        _inputMovement = movement;
    }
}