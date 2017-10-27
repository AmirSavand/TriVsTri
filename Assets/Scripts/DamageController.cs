using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageController : NetworkBehaviour
{
	public float damage = 20f;

	private float age = 0f;
	public float maxLifeTime = 10f;

	public GameObject issuer;

	[ServerCallback]
	void Update ()
	{
		// Face where it's going
		transform.up = GetComponent<Rigidbody2D> ().velocity;

		// Destroy when reaches maxLifetime
		age += Time.deltaTime;
		if (age >= maxLifeTime) {
			NetworkServer.Destroy (gameObject);
		}
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
			NetworkServer.Destroy (gameObject);
		}
	}
}
