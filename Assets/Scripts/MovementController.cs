using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	public float moveSpeed = 5f;

	void Start ()
	{
		
	}

	void Update ()
	{
		if (transform.position.y > 14.7f || transform.position.y < -3.6f) {
			moveSpeed *= -1f;
		}

		transform.Translate (Vector3.up * Time.deltaTime * moveSpeed, Space.World);
	}
}
