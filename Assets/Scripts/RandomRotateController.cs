using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotateController : MonoBehaviour
{
	public float minAngle = 0f;

	public float maxAngle = 180f;

	void Start ()
	{
		// Set random rotation
		transform.localRotation = Quaternion.Euler (0f, 0f, Random.Range (minAngle, maxAngle));
	}
}
