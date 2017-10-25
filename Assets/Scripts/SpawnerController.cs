using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
	public float spawnDelay = 0f;

	public float spawnRepeat = 4f;

	public GameObject[] items;

	void Start ()
	{
		// Call spawn
		InvokeRepeating ("spawn", spawnDelay, spawnRepeat);
	}

	void spawn ()
	{
		// Get a random item from list
		GameObject randomItem = items [Random.Range (0, items.Length)];

		// Random spawn point
		Transform randomPoint = transform.GetChild (Random.Range (0, transform.childCount));

		// Instantiate the item
		Instantiate (randomItem, randomPoint.transform.position, randomPoint.transform.rotation);
	}
}
