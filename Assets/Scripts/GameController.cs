﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	private string status = "menu";

	public GameObject menu;

	public Text winner;

	public List<GameObject> spawnPoints;

	void Update ()
	{
		// If game is running (status)
		if (status == "running") {

			// Count remaining players
			int playerCount = GameObject.FindGameObjectsWithTag ("Player").Length;

			// Only 1 player remaining (winner)
			if (playerCount == 1) {
				
				// Set status to menu
				status = "menu";

				// Show menu
				menu.SetActive (true);

				// Get winner
				GameObject player = GameObject.FindGameObjectWithTag ("Player");

				// Show winner by name and color
				winner.text = player.name + " Wins!";
				winner.color = player.GetComponent<SpriteRenderer> ().material.color;

				// Destroy winner object
				Destroy (player);
			}
		}
	}

	public void StartGame ()
	{
		// Hide menu
		menu.SetActive (false);

		// Find all spawn points
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("Respawn");

		// Spawn all
		foreach (GameObject spawn in spawns) {
			spawn.GetComponent<SpawnPoint> ().SpawnPlayer ();
		}

		// Update status to running
		status = "running";
	}
}
