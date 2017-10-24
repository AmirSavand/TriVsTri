using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
	public float damage = 20f;

	public GameObject issuer;

	void Start ()
	{
		// Destroy bullet in case of not hitting any edge
		Destroy (gameObject, 10f);
	}

	void Update ()
	{
		// Face where it's going
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
			if (hitpointController.takeBullet) {
				Destroy (gameObject);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		// Hit inner edge
		if (other.name == "Inner Edge") {
			Destroy (gameObject);
		}
	}
}
