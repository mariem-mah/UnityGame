using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject ingame; // score + countdown
	public GameObject gameOver;
	public GameObject playerCrosshair;
	public GameObject victory;

	private GameManager gameManager;

	void Start()
	{
		gameManager = GameManager.instance;
		ShowMainMenu();
	}

	public void ShowMainMenu()
	{
		mainMenu.SetActive(true);
		ingame.SetActive(false);
		victory.SetActive(false);

		gameManager.gameState = GameState.Paused;
	}

	public void ShowIngameGUI()
	{
		mainMenu.SetActive(false);
		ingame.SetActive(true);
		gameOver.SetActive(false);
		playerCrosshair.SetActive(true);

		gameManager.gameState = GameState.Running;
	}

	public void ShowGameOver()
	{
		gameOver.SetActive(true);
		playerCrosshair.SetActive(false);

		gameManager.gameState = GameState.Paused;
	}

	public void ShowVictory()
	{
		ingame.SetActive(false);
		victory.SetActive(true);

	gameManager.gameState = GameState.Paused;
	}
}
