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

	void Start ()
	{
		// Set HP to max HP
		hitpoints = maxHitpoints;

		// Update HP slider
		updateHitpointSlider ();
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

	private void updateHitpointSlider ()
	{
		// Get HP slider
		Slider hitpointSlider = UI.transform.Find ("Hitpoint Slider").GetComponent<Slider> ();

		// Update values
		hitpointSlider.value = hitpoints;
		hitpointSlider.maxValue = maxHitpoints;
	}
}
