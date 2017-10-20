using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
	public GameObject[] items;

	void Start ()
	{
		// Call spawn
		InvokeRepeating ("spawn", 0f, 1f);
	}

	void spawn ()
	{
		// 5% chance of spawn
		if (Random.value <= 0.25f) {
			
			// Get a random item from list
			GameObject randomItem = items [Random.Range (0, items.Length)];

			// Random spawn point
			Transform randomPoint = transform.GetChild (Random.Range (0, transform.childCount));

			// Instantiate the item
			Instantiate (randomItem, randomPoint.transform.position, randomPoint.transform.rotation);
	
		}
	}
}
