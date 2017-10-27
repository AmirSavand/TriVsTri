using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	public string playerName = "Player";
	public string fireKey = "Fire1";
	public bool isReady = true;

	public GameObject playerUI;
	public ShopManager shopManager;
	public GameManager gameManager;

	public HitpointController hitpointController;

	public int score = 0;
	public int stars = 1;
	public int diamonds = 1;

	public float moveSpeed = 20f;
	public bool stop = false;

	public float fireDamage = 20f;
	public float firePower = 20f;
	public float fireRate = 0.5f;
	public Rigidbody2D bullet;
	public Transform fireTransform;
	public AudioSource fireSound;

	public int weaponLevel = 0;

	private float lastTimeFired;

	void Start ()
	{
		// Destroy if not local
		if (!isLocalPlayer) {
			Destroy (this);
			return;
		}

		// Get spawn data
		playerUI = getSpawnController ().playerUI;
		shopManager = getSpawnController ().shopManager;
		gameManager = getSpawnController ().gameManager;

		// Assign variables
		shopManager.player = this;
		shopManager.playerUpgradeController = GetComponent<UpgradeController> ();
		hitpointController.UI = playerUI;
		gameManager.players.Add (this);

		// Initial UI
		updateResources ();
	}

	void Update ()
	{
		if (hitpointController.isDead || stop) {
			return;
		}

		// Key detection, fire
		if (Input.GetButton (fireKey)) {
			CmdFire ();
		}

		// Move player
		transform.Translate (Vector3.up * Time.deltaTime * moveSpeed, Space.World);

	}

	void OnTriggerStay2D (Collider2D other)
	{
		// Get edge
		EdgeController edgeController = other.GetComponent<EdgeController> ();

		// If hit edge with edge controller
		if (other.CompareTag ("Edge") && edgeController) {
			moveSpeed = edgeController.changeSpeed (moveSpeed);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (hitpointController.isDead) {
			return;
		}

		// Reverse moving if hit bullets
		if (other.CompareTag ("Bullet")) {
			moveSpeed *= -1f;
		}

		// Get edge
		EdgeController edgeController = other.GetComponent<EdgeController> ();

		// If hit edge with edge controller
		if (other.CompareTag ("Edge") && edgeController) {
			moveSpeed = edgeController.changeSpeed (moveSpeed);
		}

		// Collect if hit item
		if (other.tag == "Item") {

			// Item name
			Item item = other.GetComponent<Item> ();

			if (item.type == "Star") {
				stars += item.amount;
			}

			if (item.type == "Diamond") {
				diamonds += item.amount;
			}

			if (item.type == "Hitpoint") {
				hitpointController.heal (item.amount);
			}

			// Update UI
			updateResources ();

			// Destroy item
			Destroy (other.gameObject);
		}
	}

	[Command]
	public void CmdFire (/**bool ignoreRate = false, int shotNumber = 1**/)
	{
		if (hitpointController.isDead || stop) {
			return;
		}

		// Check fire rate
		//if (!ignoreRate && Time.fixedTime - lastTimeFired < fireRate) {
		//	return;
		//}

		// Save fire rate
		lastTimeFired = Time.fixedTime;

		// Initial position and velocity of bullet
		Vector2 position = fireTransform.position;
		Vector3 velocity = fireTransform.up * firePower;

		// Push bullets up if not initial bullet
		//if (ignoreRate) {
		//	position.y += 0.5f * shotNumber;
		//	velocity.y += 0.5f * shotNumber;
		//}

		// Create bullet
		Rigidbody2D bulletInstance = Instantiate (bullet, position, transform.rotation) as Rigidbody2D;

		// Shoot bullet
		bulletInstance.GetComponent<Transform> ().localRotation = fireTransform.rotation;
		bulletInstance.velocity = velocity;

		// Set issuer and power
		bulletInstance.GetComponent<DamageController> ().issuer = gameObject;
		bulletInstance.GetComponent<DamageController> ().damage = fireDamage;

		// Fire sound
		fireSound.Play ();

		// Spawn from server
		NetworkServer.Spawn (bulletInstance.gameObject);

		// Fire multiple shots based on weapon level
		//if (!ignoreRate) {
		//	for (int i = 0; i < weaponLevel; i++) {
		//		CmdFire (true, i + 1);
		//	}
		//}
	}

	public void updateResources ()
	{
		// Update resources
		playerUI.transform.Find ("Diamonds").GetComponentInChildren<Text> ().text = "" + diamonds;
		playerUI.transform.Find ("Stars").GetComponentInChildren<Text> ().text = "" + stars;

		// Update shop button status
		if (shopManager) {
			shopManager.updateUpgradeButtonStatus ();
		}
	}

	public PlayerSpawnController getSpawnController ()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Respawn");
		GameObject closest = null;
		Vector3 position = transform.position;
		float distance = Mathf.Infinity;

		foreach (GameObject go in gos) {
			
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;

			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}

		return closest.GetComponent<PlayerSpawnController> ();
	}
}
