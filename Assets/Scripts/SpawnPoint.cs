using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public string name = "Blue Player";

	public string fireKey = "Fire1";

	public float movementFactor = 1f;

	public Color color = Color.blue;

	public GameObject playerPrefab;

	public void SpawnPlayer ()
	{
		// Spawn player
		GameObject player = Instantiate (playerPrefab, transform.position, transform.rotation) as GameObject;

		// Set name
		player.name = name;

		// Set movement factor (direction)
		player.GetComponent<MovementController> ().moveSpeed *= movementFactor;

		// Set fire key
		player.GetComponent<PlayerController> ().fireKey = fireKey;

		// Set color
		player.GetComponent<SpriteRenderer> ().material.color = color;
	}
}
