using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
	public string playerName = "Blue Player";

	public string playerFireKey = "Fire1";

	public float playerMoveFactor = 1f;

	public GameObject playerUI;

	public GameObject playerPrefab;

	public Color playerColor = Color.blue;

	public void SpawnPlayer ()
	{
		// Spawn player
		GameObject player = Instantiate (playerPrefab, transform.position, transform.rotation) as GameObject;

		// Get player controller
		PlayerController playerController = player.GetComponent<PlayerController> ();

		// Set name
		player.name = playerName;

		// Set color
		player.GetComponent<SpriteRenderer> ().material.color = playerColor;

		// Set movement factor (direction)
		playerController.moveSpeed *= playerMoveFactor;

		// Set fire key
		playerController.fireKey = playerFireKey;

		// Set UI
		playerController.UI = playerUI;
	}
}
