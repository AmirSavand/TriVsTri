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
		// Get HP controller
		HitpointController hitpointController = other.GetComponent<HitpointController> ();

		// If other has HP
		if (hitpointController) {
			
			// Deal damage
			hitpointController.damage (damage);

			// Self distruct
			Destroy (gameObject);
		}
	}
}
