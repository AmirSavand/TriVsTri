﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

	public float moveSpeed = 2f;

	private Rigidbody2D rigidBody;

	void Start ()
	{
		// Get components
		rigidBody = GetComponent<Rigidbody2D> ();

		// No gravity scale
		rigidBody.gravityScale = 0f;

		// Random move speed
		moveSpeed = Random.Range (moveSpeed - 1f, moveSpeed + 1f);
	}

	void Update ()
	{
		// Move down
		transform.Translate (Vector2.down * Time.deltaTime * moveSpeed, Space.World);
	}

	void OnTriggerExit2D (Collider2D other)
	{
		// Hit inner edge
		if (other.name == "Inner Edge") {
			Destroy (gameObject);
		}
	}
}
