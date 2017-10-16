using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public string fireKey = "Fire1";

	public float moveSpeed = 20f;

	public float maxHitpoints = 100f;
	public float hitpoints;
	public bool isDead = false;
	public AudioSource hitSound;
	public AudioSource deadSound;

	public float firePower = 20f;
	public float fireRate = 0.5f;
	public Rigidbody2D bullet;
	public Transform fireTransform;
	public AudioSource fireSound;

	public GameObject UI;

	private float lastTimeFired;

	void Start ()
	{
		// Set HP to max HP
		hitpoints = maxHitpoints;

		// Update HP slider
		updateHitpointSlider ();
	}

	void Update ()
	{
		if (isDead) {
			return;
		}

		// Key detection, fire
		if (Input.GetButton (fireKey)) {
			fire ();
		}

		// Stop player at edges (@TODO: Use edge collider)
		if (transform.position.y > 14.7f || transform.position.y < -3.6f) {
			moveSpeed *= -1f;
		}

		// Move player
		transform.Translate (Vector3.up * Time.deltaTime * moveSpeed, Space.World);
	}

	public void damage (float amount)
	{
		if (isDead) {
			return;
		}

		// Deal damage
		hitpoints = Mathf.Clamp (hitpoints -= amount, 0f, maxHitpoints);

		// Update HP slider
		updateHitpointSlider ();

		// Destroy if no HP left
		if (hitpoints == 0f) {

			// Set to death
			isDead = true;

			// Dead sound
			deadSound.Play ();

			// Destroy after audio finished
			Destroy (gameObject, deadSound.clip.length);
		
		} else {
			
			// Hit sound
			hitSound.Play ();
		}
	}

	public void fire ()
	{
		if (isDead) {
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

	private void updateHitpointSlider ()
	{
		// Get HP slider
		Slider hitpointSlider = UI.transform.Find ("Hitpoint Slider").GetComponent<Slider> ();

		// Update values
		hitpointSlider.value = hitpoints;
		hitpointSlider.maxValue = maxHitpoints;
	}
}
