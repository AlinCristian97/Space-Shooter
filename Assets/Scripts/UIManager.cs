using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver, ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private ISubject _player;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;

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

    public void GetNotified()
    {
        if (_player is Player)
        {
            UpdateScore((_player as Player).Score);
            UpdateLives((_player as Player).Lives);
        }
    }

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _player.AddObserver(this);
    }

    private void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives < 1)
        {
            NotifyObservers();
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerCoroutine());
    }

    private IEnumerator GameOverFlickerCoroutine()
    {
        float flickerRate = 1f;

        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(flickerRate);
            _gameOverText.text = "";
            yield return new WaitForSeconds(flickerRate);
        }
    }


}