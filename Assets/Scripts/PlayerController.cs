using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public string fireKey = "Fire1";

	public float maxHitpoints = 100f;
	public float hitpoints;

	public float firePower = 20f;
	public float fireRate = 0.5f;
	public Rigidbody2D bullet;
	public Transform fireTransform;

	private float lastTimeFired;

	void Start ()
	{
		hitpoints = maxHitpoints;
	}

	void Update ()
	{
		if (Input.GetButton (fireKey)) {

			fire ();
		}
	}

	public void damage (float amount)
	{
		// Deal damage
		hitpoints = Mathf.Clamp (hitpoints -= amount, 0f, maxHitpoints);

		// Destroy if no hp left
		if (hitpoints == 0f) {
			Destroy (gameObject);
		}
	}

	public void fire ()
	{
		// Check fire rate
		if (Time.fixedTime - lastTimeFired < fireRate) {
			return;
		}

		// Save fire rate
		lastTimeFired = Time.fixedTime;

		// Create bullet
		Rigidbody2D bulletInstance = Instantiate (bullet, fireTransform.position, fireTransform.rotation) as Rigidbody2D;

		// Shoot bullet
		bulletInstance.velocity = firePower * fireTransform.right;
	}
}
