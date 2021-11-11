using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GameState
{
	Paused,
	Running
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GUIManager guiManager;
	public TargetManager targetManager;
	public GameObject player;
	public Text scoreText;
	public int countdownMinutes = 3;
	public Text countdownText;
	public int winningScore = 2000;

	[HideInInspector]
	public GameState gameState;

	private int score;

	void Awake()
	{
		
		// Set game manager instance to be accessible from everywhere
		if (GameManager.instance == null)
		{
			GameManager.instance = this;
		}

		// Keep screen on
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	void Start()
	{
		Init();
	}

	public void StartNewGame()
	{
		// Reset score
		ResetScore();

		// Enable target manager
		targetManager.enabled = true;

		// Start countdown
		StartCoroutine(Countdown());
	}

	public void EndGame()
	{
		// Disable target manager
		targetManager.enabled = false;

		// Stop coroutines
		StopAllCoroutines();
	}

	public void ExitApplication()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void AddPoints(int points)
	{
		// Add points to total score
		score += points;

		// Check if player has reached the winning score
		if (score >= winningScore)
		{
			StartCoroutine(WinGame());
		}

		// Update UI
		UpdateScoreText();
	}

	private void Init()
	{
		/*
		// Set game manager instance to be accessible from everywhere
		if (GameManager.instance == null)
		{
			GameManager.instance = this;
		}
		*/
				// Init targets
				targetManager.InitTargets();
	}

	private void UpdateScoreText()
	{
		// Update score UI text field
		scoreText.text = score.ToString();
	}

	private void ResetScore()
	{
		// Reset score to zero
		score = 0;
		UpdateScoreText();
	}

	private IEnumerator Countdown()
	{
		int i = countdownMinutes * 60;
		WaitForSeconds tick = new WaitForSeconds(0.1f);

		int minutes;
		int seconds;

		// Countdown timer
		while (i >= 0)
		{
			minutes = i / 60;
			seconds = i - (minutes * 60);

			countdownText.text = String.Format("{0:00}:{1:00}", minutes, seconds);

			yield return tick;

			i--;
		}

		// Game over
		EndGame();
		guiManager.ShowGameOver();
	}

	private IEnumerator WinGame()
	{
		guiManager.ShowVictory();

		yield return new WaitForSeconds(1);

		EndGame();
	}
}
