using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
	public float destroyDelay = 0f;

	void Start ()
	{
		// Destroy after delay
		Destroy (gameObject, destroyDelay);
	}
}
