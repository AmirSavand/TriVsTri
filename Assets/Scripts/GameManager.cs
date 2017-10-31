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
	public PlayerController[] players;

	void Update ()
	{
		// If game is running (status)
		if (gameStatus == "running") {

			// Check timer and reduce it over time
			if (gameTimer > 0f) {
			
				// Reduce timer
				gameTimer -= Time.deltaTime;

				// Update timer UI
				timerUI.GetComponentInChildren<Text> ().text = "" + (int)gameTimer;
			}

			// Timers time's up
			else {

				// Stop game
				gameStatus = "finish";

				// Show finish UI
				playersUI.SetActive (false);
				finishUI.SetActive (true);

				// Top score
				int topScore = 0;
				PlayerController topPlayer = null;

				// Find winner
				foreach (PlayerController player in players) {

					// If higher score than other player
					if (player.score >= topScore) {
					
						// Save top
						topScore = player.score;
						topPlayer = player;
					}
				}

				// Stop all players
				startPlayers (false);

				// Set winner
				finishUI.GetComponentInChildren<Text> ().text = topPlayer.playerName + " Won!";
				finishUI.GetComponentInChildren<Text> ().color = topPlayer.GetComponent<SpriteRenderer> ().material.color;
			}

			// Alive players
			PlayerController alivePlayer = null;
			int alivePlayersCount = 0;

			// Count alive players
			foreach (PlayerController player in players) {
				
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
				playersUI.SetActive (true);
				shopUI.SetActive (true);

				// Stop all players and mark as not ready
				foreach (PlayerController player in players) {
					player.isReady = false;
					player.stop = true;
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

					// Update text and color
					winnerText.text = "Draw!";
					winnerText.color = new Color32 (0x66, 0x66, 0x66, 0xFF);
				}
			}
		}

		// If game is in shop/finish
		if (gameStatus == "shop" || gameStatus == "finish") {

			// All players are ready
			if (getReadyPlayers ().Count == players.Length) {
			
				// Hide shop/finish
				playersUI.SetActive (true);
				shopUI.SetActive (false);
				finishUI.SetActive (false);

				// Reset timer if from finish
				if (gameStatus == "finish") {
					gameTimer = startingTime;
				}

				// Update game
				gameStatus = "running";

				// Start em
				startPlayers (true, true);
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
		startPlayers (true, true);

		// Reset timer if starting game from menu or continuing from finish
		if (gameStatus == "menu" || gameStatus == "finish") {
			gameTimer = startingTime;
		}

		// Game is running
		gameStatus = "running";
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
		// If from pause menu
		if (gameStatus == "pause") {
			
			// Restore time scale
			Time.timeScale = 1f;

			// Reload scene (restore)
			SceneManager.LoadScene ("Main");
		} 

		// Anywhere else
		else {

			// Quit game and exit to desktop
			Application.Quit ();
		}
	}

	public void alert (string message)
	{
		// Create the object
		GameObject textGameObject = Instantiate (alertText);
		textGameObject.transform.SetParent (GameObject.Find ("Canvas/Global/Alerts").transform, false);

		// Set message
		textGameObject.GetComponent<Text> ().text = message;
	}

	public void startPlayers (bool start = true, bool death = false)
	{
		// All players
		foreach (PlayerController player in players) {

			// Get HP controller
			HitpointController hitpointController = player.GetComponent<HitpointController> ();

			// Heal (if start)
			if (start) {
				hitpointController.heal ();
			}

			// Update death status (if death)
			if (death) {
				hitpointController.isDead = !start;
			}

			// Update status
			player.stop = !start;
			player.isReady = start;
		}
	}

	public List<PlayerController> getReadyPlayers ()
	{
		// Ready players
		List<PlayerController> readyPlayers = new List<PlayerController> ();
	
		// For all players
		foreach (PlayerController player in players) {

			// Is ready
			if (player.isReady) {
			
				// Add to list
				readyPlayers.Add (player);
			}
		}

		// Return ready players list
		return readyPlayers;
	}
}
