using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public string fireKey = "Fire1";

	public int stars = 1;
	public int diamonds = 1;

	public float moveSpeed = 20f;
	public bool stop = false;

	public float firePower = 20f;
	public float fireRate = 0.5f;
	public Rigidbody2D bullet;
	public Transform fireTransform;
	public AudioSource fireSound;

	public int weaponLevel = 0;

	public GameObject UI;

	private float lastTimeFired;

	private HitpointController hitpointController;

	void Start ()
	{
		// Get components
		hitpointController = GetComponent<HitpointController> ();

		// Assign variables
		hitpointController.UI = UI;

		// Initial UI
		updateResources ();
	}

	void Update ()
	{
		if (hitpointController.isDead || stop) {
			return;
		}

		// Key detection, fire
		if (Input.GetButton (fireKey)) {
			fire ();
		}

		// Move player
		transform.Translate (Vector3.up * Time.deltaTime * moveSpeed, Space.World);

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		// Reverse moving if hit bullets or edges
		if (other.tag == "Edge" || other.tag == "Bullet") {
			moveSpeed *= -1f;
		}

		// Collect if hit item
		if (other.tag == "Item") {

			// Item name
			Item item = other.GetComponent<Item> ();

			if (item.type == "Star") {
				stars += item.amount;
			}

			if (item.type == "Diamond") {
				diamonds += item.amount;
			}

			// Update UI
			updateResources ();

			// Destroy item
			Destroy (other.gameObject);
		}
	}

	public void fire (bool ignoreRate = false, int shotNumber = 1)
	{
		if (hitpointController.isDead || stop) {
			return;
		}

		// Check fire rate
		if (!ignoreRate && Time.fixedTime - lastTimeFired < fireRate) {
			return;
		}

		// Save fire rate
		lastTimeFired = Time.fixedTime;

		// Initial position and velocity of bullet
		Vector2 position = fireTransform.position;
		Vector3 velocity = fireTransform.up * firePower;

		// Push bullets up if not initial bullet
		if (ignoreRate) {
			position.y += 0.5f * shotNumber;
			velocity.y += 0.5f * shotNumber;
		}

		// Create bullet
		Rigidbody2D bulletInstance = Instantiate (bullet, position, transform.rotation) as Rigidbody2D;

		// Shoot bullet
		bulletInstance.GetComponent<Transform> ().localRotation = fireTransform.rotation;
		bulletInstance.velocity = velocity;

		// Set issuer
		bulletInstance.GetComponent<DamageController> ().issuer = gameObject;

		// Fire sound
		fireSound.Play ();

		// Fire multiple shots based on weapon level
		if (!ignoreRate) {
			for (int i = 0; i < weaponLevel; i++) {
				fire (true, i + 1);
			}
		}
	}

	public void updateResources ()
	{
		// Update resources
		UI.transform.Find ("Diamonds").GetComponentInChildren<Text> ().text = "" + diamonds;
		UI.transform.Find ("Stars").GetComponentInChildren<Text> ().text = "" + stars;
	}
}
