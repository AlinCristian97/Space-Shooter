using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IObserver
{
    private ISubject _uiManager;

    [SerializeField] private bool _isGameOver;

    public VoidEventChannelSO OnExitGame;
    public VoidEventChannelSO OnRestartLevel;

    public void GetNotified()
    {
        GameOver();
    }

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.AddObserver(this);
    }

    private void Update()
    {
        if (_isGameOver && Keyboard.current[Key.R].wasPressedThisFrame)
        {
            OnRestartLevel.RaiseEvent();
        }

        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            OnExitGame.RaiseEvent();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1); // Game Scene
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        //Application.Quit();
    }
}