using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotateController : MonoBehaviour
{

	void Start ()
	{
		// Set random rotation
		transform.localRotation = Quaternion.Euler (0f, 0f, Random.Range (0, 180));
	}
}
