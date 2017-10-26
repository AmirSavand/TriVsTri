using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Game
	public string gameStatus = "menu";
	public string gameMode;
	public float gameTimer = 0f;

	// Defaults
	public float startingTime = 180f;

	// UI objects
	public GameObject menuUI;
	public GameObject playersUI;
	public GameObject shopUI;
	public GameObject pauseUI;
	public GameObject timerUI;
	public GameObject finishUI;

	// Alert
	public GameObject alertText;

	// Winner
	public Text winnerText;

	// Players
	private GameObject[] players;

	void Start ()
	{
		// Get players
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	void Update ()
	{
		// If game is running (status)
		if (gameStatus == "running") {

			// Check timer and reduce it over time
			if (gameTimer >= 0f) {
			
				// Reduce timer
				gameTimer -= Time.deltaTime;

				// Update timer UI
				timerUI.GetComponentInChildren<Text> ().text = "" + (int)gameTimer;
			}

			// Timer reached 0
			else {

				// Stop game
				gameStatus = "finish";

				// Show finish UI
				menuUI.SetActive (false);
				playersUI.SetActive (false);
				shopUI.SetActive (false);
				finishUI.SetActive (true);

				// Top score
				int topScore = 0;
				PlayerController topPlayer = null;

				// Find winner
				foreach (GameObject player in players) {

					// Player controller
					PlayerController playerController = player.GetComponent<PlayerController> ();

					// If higher score than other player
					if (playerController.score >= topScore) {
					
						// Save top
						topScore = playerController.score;
						topPlayer = playerController;
					}

					// Stop player
					playerController.stop = true;
					playerController.isReady = false;
				}

				// Set winner
				finishUI.GetComponentInChildren<Text> ().text = topPlayer.playerName + " Won!";
				finishUI.GetComponentInChildren<Text> ().color = topPlayer.GetComponent<SpriteRenderer> ().material.color;
			}

			// Alive players
			GameObject alivePlayer = null;
			int alivePlayersCount = 0;

			// Count remaining players
			foreach (GameObject player in players) {
				
				// If alive
				if (!player.GetComponent<HitpointController> ().isDead) {
					
					// Increase count
					alivePlayersCount++;

					// Store player
					alivePlayer = player;
				}
			}

			// Less than 2 players remaining (stop game)
			if (alivePlayersCount < 2) {

				// Set status to menu
				gameStatus = "shop";

				// Show shop menu
				menuUI.SetActive (false);
				playersUI.SetActive (true);
				shopUI.SetActive (true);

				// Stop all players and mark as not ready
				foreach (GameObject player in players) {
					player.GetComponent<PlayerController> ().isReady = false;
					player.GetComponent<PlayerController> ().stop = true;
				}

				// Game has a winner
				if (alivePlayer) {

					// Show winner by name and color
					winnerText.text = alivePlayer.GetComponent<PlayerController> ().playerName + " Wins!";
					winnerText.color = alivePlayer.GetComponent<SpriteRenderer> ().material.color;

					// Add score to winner
					alivePlayer.GetComponent<PlayerController> ().score += 1;
				}

				// Draw
				else {
					winnerText.text = "Draw!";
					winnerText.color = new Color32 (0x66, 0x66, 0x66, 0xFF);
				}
			}
		}

		// Press pause button
		if (Input.GetButtonUp ("Cancel")) {
			
			// If game is running
			if (gameStatus == "running") {
			
				// Set mode to paused
				gameStatus = "pause";
			
				// Pause time/game
				Time.timeScale = 0;

				// Show pause menu
				pauseUI.SetActive (true);
			}

			// If game is paused
			else if (gameStatus == "pause") {
				resumeGame ();
			}
		}
	}

	public void startGame (string mode = "2players")
	{
		// Set game mode
		gameMode = mode;

		// Online mode
		if (mode == "online") {
			alert ("Playing online feature is coming soon!");
			return;
		}

		// Vs bot
		if (mode == "vsbot") {
			alert ("Playing vs bot feature is coming soon!");
			return;
		}

		// Only show players UI
		playersUI.SetActive (true);
		menuUI.SetActive (false);
		shopUI.SetActive (false);

		// Start em
		foreach (GameObject player in players) {
			
			// Make alive
			HitpointController playerHitpointController = player.GetComponent<HitpointController> ();
			playerHitpointController.heal ();
			playerHitpointController.isDead = false;

			// Start moving
			player.GetComponent<PlayerController> ().stop = false;
		}

		// Reset timer if starting game from menu
		if (gameStatus == "menu") {
			gameTimer = startingTime;
		}

		// Game is running
		gameStatus = "running";
	}

	public void readyPlayer (PlayerController playerController)
	{
		// Set player to ready
		playerController.isReady = true;

		// Count ready players
		int readyPlayers = 0;

		// Start game if all ready
		foreach (GameObject player in players) {
			// Increase ready players counter
			if (player.GetComponent<PlayerController> ().isReady) {
				readyPlayers++;
			}
		}

		// If all players are ready, start
		if (readyPlayers == players.Length) {
			startGame (gameMode);
		}
	}

	public void resumeGame ()
	{
		// Restore time scale
		Time.timeScale = 1f;

		// Hide pause menu
		pauseUI.SetActive (false);

		// Update status
		gameStatus = "running";
	}

	public void exitGame ()
	{
		// Restore time scale
		Time.timeScale = 1f;

		// Reload scene (restore)
		SceneManager.LoadScene ("Main");
	}

	public void alert (string message)
	{
		// Create the object
		GameObject textGameObject = Instantiate (alertText);
		textGameObject.transform.SetParent (GameObject.Find ("Canvas/Global/Alerts").transform, false);

		// Set message
		textGameObject.GetComponent<Text> ().text = message;
	}
}
