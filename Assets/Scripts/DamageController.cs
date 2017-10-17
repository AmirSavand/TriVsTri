using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
	public float damage = 20f;

	public GameObject issuer;

	void Start ()
	{
		// Destroy bullet after 4 seconds
		Destroy (gameObject, 4f);
	}

	void Update ()
	{
		transform.up = GetComponent<Rigidbody2D> ().velocity;
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		// Get HP controller
		HitpointController hitpointController = other.GetComponent<HitpointController> ();

		// If other has HP
		if (hitpointController && !hitpointController.isDead) {
			
			// Deal damage
			hitpointController.damage (damage, issuer);

			// Self distruct (if target is not dead)
			Destroy (gameObject);
		}
	}
}
