﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;

    private void Update()
    {
        //if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene(1); // Game Scene
        //}

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}