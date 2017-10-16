using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
	public float damage = 20f;

	void Start ()
	{
		// Destroy bullet after 4 seconds
		Destroy (gameObject, 4f);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		// Hit player
		if (other.tag == "Player") {
			
			// Deal damage
			other.GetComponent<PlayerController> ().damage (damage);
		}
	}
}
