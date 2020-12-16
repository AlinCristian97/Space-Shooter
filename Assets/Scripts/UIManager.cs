using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
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
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
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