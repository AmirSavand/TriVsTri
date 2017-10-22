using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private string status = "menu";

	public GameObject menuUI;
	public GameObject playersUI;
	public GameObject shopUI;

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
		if (status == "running") {

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
				status = "shop";

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
	}

	public void startGame ()
	{
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
		status = "running";
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
}
