using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitpointController : MonoBehaviour
{

	public float maxHitpoints = 100f;
	public float hitpoints;

	public bool isDead = false;

	public AudioSource hitSound;
	public AudioSource deadSound;

	public GameObject UI;

	public Transform movingToCollector;
	public Transform movingFrom;

	void Start ()
	{
		// Set HP to max HP
		hitpoints = maxHitpoints;

		// Update HP slider
		updateHitpointSlider ();
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

			// Get destroy time
			float time = 0f;

			// Dead sound
			if (deadSound) {
				deadSound.Play ();
				time = deadSound.clip.length;
			}

			// If collectable (item)
			if (tag == "Item") {

				// Set to collector
				movingToCollector = issuer.transform;

				// Save start position
				movingFrom = transform;

			} else {
				
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

	private void updateHitpointSlider ()
	{
		if (!UI) {
			return;
		}

		// Get HP slider
		Slider hitpointSlider = UI.transform.Find ("Hitpoint Slider").GetComponent<Slider> ();

		// Update values
		hitpointSlider.value = hitpoints;
		hitpointSlider.maxValue = maxHitpoints;
	}
}
