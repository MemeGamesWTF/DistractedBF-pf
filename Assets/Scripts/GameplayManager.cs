using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices;
public class GameplayManager : MonoBehaviour
{
    private bool hasGameFinished;
    private bool isPaused; // Added for pause functionality

    public GameObject GameOverPanel;
    public GameObject Player;
    public GameObject Obstacle;
    public GameObject PausePanel; // Add this in Unity Inspector for your pause menu UI

    [SerializeField] private TMP_Text _scoreText;

    private float score;
    private float scoreSpeed;
    private int currentLevel;

    [SerializeField] private List<int> _levelSpeed, _levelMax;

       [DllImport("__Internal")]
  private static extern void SendScore(int score, int game);

    private void Awake()
    {
        GameManager.Instance.IsInitialized = true;

        score = 0;
        currentLevel = 0;
        isPaused = false; // Initialize paused state
        _scoreText.text = ((int)score).ToString();

        scoreSpeed = _levelSpeed[currentLevel];
    }

    private void Update()
    {
        if (hasGameFinished || isPaused) return; // Modified to include pause check

        score += scoreSpeed * Time.deltaTime;
        _scoreText.text = ((int)score).ToString();

        if (score > _levelMax[Mathf.Clamp(currentLevel, 0, _levelMax.Count - 1)])
        {
            currentLevel = Mathf.Clamp(currentLevel + 1, 0, _levelMax.Count - 1);
            scoreSpeed = _levelSpeed[currentLevel];
        }

        // Check for pause input (using Escape key as example)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void GameEnded()
    {
        hasGameFinished = true;
        GameManager.Instance.CurrentScore = (int)score;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        Player.SetActive(false);
        Obstacle.SetActive(false);
        GameOverPanel.SetActive(true);
        SendScore((int)score, 106);
        // GameManager.Instance.GotoMainMenu();
    }

    public void Restart()
    {
        GameManager.Instance.GotoGameplay();
    }

    // New pause functionality
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freezes the game
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resumes normal time flow
        PausePanel.SetActive(false);
    }
}