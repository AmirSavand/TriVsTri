using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitpointController : MonoBehaviour
{
	public float maxHitpoints = 100f;
	public float hitpoints;

	public bool destroyAfterDeath = true;
	public bool isDead = false;
	public bool takeBullet = true;

	public AudioSource hitSound;
	public AudioSource deadSound;

	public GameObject UI;

	public Transform movingToCollector;
	public Transform movingFrom;

	public GameObject deathItem;
	public int deathItemCount;

	private PlayerController playerController;

	void Start ()
	{
		// Set HP to max HP
		hitpoints = maxHitpoints;

		// Update HP slider
		updateHitpointSlider ();

		// Get player controller
		playerController = GetComponent<PlayerController> ();
	}

	void Update ()
	{
		// Dead and has a collector (being collected)
		if (isDead && movingToCollector) {
			
			// Move to target
			transform.position = Vector2.Lerp (movingFrom.position, movingToCollector.transform.position, Time.deltaTime * 2f);

			// Scale down
			transform.localScale = Vector3.Lerp (transform.localScale, new Vector2 (0.1f, 0.1f), Time.deltaTime);

			// Remove gravity
			GetComponent<Rigidbody2D> ().gravityScale = 0f;
		}
	}

	public void damage (float amount, GameObject issuer)
	{	
		if (isDead || (playerController && playerController.stop)) {
			return;
		}

		// Deal damage
		hitpoints = Mathf.Clamp (hitpoints -= amount, 0f, maxHitpoints);

		// Update HP slider
		updateHitpointSlider ();

		// Kill if no HP left
		if (hitpoints == 0f) {

			// Set to death
			isDead = true;

			// Get destroy time
			float time = 0f;

			// Dead sound
			if (deadSound) {
				deadSound.Play ();
				time = deadSound.clip.length;
			}

			// If has item drop
			if (deathItemCount > 0 && issuer != gameObject) {
				
				// As much as item count
				for (int i = 0; i < deathItemCount; i++) {
					
					// Different positions
					Vector2 position = new Vector2 (transform.position.x * i, transform.position.y * i);

					// Drop items
					HitpointController deathItemInstance = Instantiate (deathItem, position, transform.rotation).GetComponent<HitpointController> ();

					// Collect by issuer
					deathItemInstance.damage (1f, issuer);

					// Sound like 1 item
					deathItemInstance.GetComponent<AudioSource> ().volume /= deathItemCount;
				}
			}

			// If collectable (item)
			if (tag == "Item") {

				// Set to collector
				movingToCollector = issuer.transform;

				// Save start position
				movingFrom = transform;
			
				// No movement
				GetComponent<MovementController> ().moveSpeed = 0;
			}

			// If needs to be destroyed after dying
			else if (destroyAfterDeath) {
				
				// Destroy after audio finished
				Destroy (gameObject, time);
			}

		} else {

			// Hit sound
			if (hitSound) {
				hitSound.Play ();
			}
		}
	}

	public void heal (float amount = -1f)
	{
		// If amount was not given, heal fully
		if (amount == -1f) {
			amount = maxHitpoints;
		}

		// Otherwize heal by amount
		else {
			// Set min and max amount of healing
			amount = Mathf.Clamp (amount, 1f, maxHitpoints);
		}

		// Heal and update UI
		isDead = false;
		hitpoints = amount;
		updateHitpointSlider ();
	}

	public void updateHitpointSlider ()
	{
		if (!UI) {
			return;
		}

		// Update value
		UI.transform.Find ("Hitpoints/Slider").GetComponent<Slider> ().value = hitpoints / maxHitpoints * 100;
	}
}
