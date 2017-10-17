using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public string fireKey = "Fire1";

	public float moveSpeed = 20f;

	public float firePower = 20f;
	public float fireRate = 0.5f;
	public Rigidbody2D bullet;
	public Transform fireTransform;
	public AudioSource fireSound;

	public GameObject UI;

	private float lastTimeFired;

	private HitpointController hitpointController;

	void Start ()
	{
		// Get components
		hitpointController = GetComponent<HitpointController> ();

		// Assign variables
		hitpointController.UI = UI;
	}

	void Update ()
	{
		if (hitpointController.isDead) {
			return;
		}

		// Key detection, fire
		if (Input.GetButton (fireKey)) {
			fire ();
		}

		// Move player
		transform.Translate (Vector3.up * Time.deltaTime * moveSpeed, Space.World);
	}

	public void fire ()
	{
		if (hitpointController.isDead) {
			return;
		}

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

		// Fire sound
		fireSound.Play ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		// Reverse moving if hit bullets or edges
		if (other.tag == "Edge" || other.tag == "Bullet") {
			moveSpeed *= -1f;
		}
	}
}
