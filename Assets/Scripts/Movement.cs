﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

	public float moveSpeed = 2f;

	private Rigidbody2D rigidBody;

	void Start ()
	{
		// Get components
		rigidBody = GetComponent<Rigidbody2D> ();

		// No gravity scale
		rigidBody.gravityScale = 0f;
	}

	void Update ()
	{
		// Move down
		transform.Translate (Vector2.down * Time.deltaTime * moveSpeed, Space.World);
	}
}
