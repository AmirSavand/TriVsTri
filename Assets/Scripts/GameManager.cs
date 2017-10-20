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

	public List<GameObject> spawnPoints;

	void Update ()
	{
		// If game is running (status)
		if (status == "running") {

			// Get current players 
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
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

			// Less than 2 players remaining
			if (alivePlayersCount < 2) {

				// Set status to menu
				status = "shop";

				// Show shop menu
				menuUI.SetActive (false);
				playersUI.SetActive (true);
				shopUI.SetActive (true);

				// Game has a winner
				if (alivePlayer) {

					// Show winner by name and color
					winnerText.text = alivePlayer.name + " Wins!";
					winnerText.color = alivePlayer.GetComponent<SpriteRenderer> ().material.color;

					// Stop winner
					alivePlayer.GetComponent<PlayerController> ().stop = true;
				}

				// Draw
				else {
					winnerText.text = "Draw!";
					winnerText.color = new Color32 (0x66, 0x66, 0x66, 0xFF);
				}
			}
		}
	}

	public void StartGame ()
	{
		// Only show players UI
		menuUI.SetActive (false);
		playersUI.SetActive (true);
		shopUI.SetActive (false);

		// Find all players
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		// Destroy them
		foreach (GameObject player in players) {
			Destroy (player);
		}

		// Find all spawn points
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("Respawn");

		// Spawn all
		foreach (GameObject spawn in spawns) {
			spawn.GetComponent<PlayerSpawn> ().SpawnPlayer ();
		}

		// Update status to running
		status = "running";
	}

	public void ContinueGame ()
	{
		// Only show players UI
		menuUI.SetActive (false);
		playersUI.SetActive (true);
		shopUI.SetActive (false);

		// Find all players
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		// Update status to running
		status = "running";

		// Start em
		foreach (GameObject player in players) {
			// Make alive
			HitpointController playerHitpointController = player.GetComponent<HitpointController> ();
			playerHitpointController.heal ();
			playerHitpointController.isDead = false;
			// Start moving
			player.GetComponent<PlayerController> ().stop = false;
		}
	}
}
