using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public string gameStatus = "menu";
	public string gameMode;

	public GameObject menuUI;
	public GameObject playersUI;
	public GameObject shopUI;
	public GameObject pauseUI;
	public GameObject alertText;

	public Text winnerText;

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

		// Update status to running
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
			startGame ();
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
