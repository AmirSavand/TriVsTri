using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateController : MonoBehaviour
{
	public GameObject toActivate;

	public float activationDelay = 0f;

	void OnEnable ()
	{
		// Deactivate
		toActivate.SetActive (false);

		// Activate after delay
		Invoke ("activate", activationDelay);
	}

	public void activate ()
	{
		// Activate object
		toActivate.SetActive (true);
	}
}
