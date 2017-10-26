using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyController : MonoBehaviour
{
	public PlayerController player;

	public Button button;

	void Start ()
	{
		// Add button event
		button.onClick.AddListener (() => setReady (true));
	}

	void OnEnable ()
	{
		// Interactable
		button.interactable = true;
	}

	public void setReady (bool ready = true)
	{
		// Update button status
		button.interactable = !ready;

		// Update player status
		player.isReady = ready;
	}
}
