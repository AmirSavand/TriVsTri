using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	private string status = "menu";

	public GameObject menuUI;
	public GameObject playersUI;

	public Text winner;

	public List<GameObject> spawnPoints;

	void Update ()
	{
		// If game is running (status)
		if (status == "running") {

			// Count remaining players
			int playerCount = GameObject.FindGameObjectsWithTag ("Player").Length;

			// Less than 2 players remaining
			if (playerCount < 2) {
				
				// Set status to menu
				status = "menu";

				// Show menu and hide player UI
				menuUI.SetActive (true);
				playersUI.SetActive (false);

				// Get winner
				GameObject winnerPlayer = GameObject.FindGameObjectWithTag ("Player");

				// Game has a winner
				if (winnerPlayer) {

					// Show winner by name and color
					winner.text = winnerPlayer.name + " Wins!";
					winner.color = winnerPlayer.GetComponent<SpriteRenderer> ().material.color;

					// Destroy winner object
					Destroy (winnerPlayer);
				}

				// Draw
				else {
					winner.text = "Draw!";
					winner.color = new Color32 (0x66, 0x66, 0x66, 0xFF);
				}
			}
		}
	}

	public void StartGame ()
	{
		// Hide menu and show player UI
		menuUI.SetActive (false);
		playersUI.SetActive (true);

		// Find all spawn points
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("Respawn");

		// Spawn all
		foreach (GameObject spawn in spawns) {
			spawn.GetComponent<PlayerSpawn> ().SpawnPlayer ();
		}

		// Update status to running
		status = "running";
	}
}
