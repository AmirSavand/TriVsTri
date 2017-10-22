using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public GameObject player;
	public PlayerController playerController;
	public UpgradeController playerUpgradeController;

	public GameManager gameManager;

	public Button readyButton;

	public Upgrade[] upgrades;

	void Awake ()
	{
		// Get components
		playerController = player.GetComponent<PlayerController> ();
		playerUpgradeController = player.GetComponent<UpgradeController> ();
		gameManager = GameObject.Find ("Game").GetComponent<GameManager> ();

		// Get upgrades
		upgrades = GameObject.Find ("Data").GetComponentsInChildren<Upgrade> ();

		// Panel button
		GameObject panelButton = GameObject.Find ("Panel Button");

		// Create upgrade buttons
		foreach (Upgrade upgrade in upgrades) {
			
			// Clone button
			GameObject buttonGameObject = Instantiate (panelButton);
			Button button = buttonGameObject.GetComponent<Button> ();

			// Attach to panel
			buttonGameObject.transform.SetParent (transform);

			// Set texts
			buttonGameObject.GetComponentsInChildren<Text> () [0].text = "+" + Mathf.Abs (upgrade.amount);
			buttonGameObject.GetComponentsInChildren<Text> () [1].text = "" + upgrade.title;
			buttonGameObject.GetComponentsInChildren<Text> () [2].text = "" + upgrade.price;

			// Set price type
			button.transform.Find ("Price " + upgrade.priceType.type).gameObject.SetActive (true);

			// On click
			button.onClick.AddListener (() => upgradeClick (upgrade, button));

			// Can upgrade
			button.interactable = playerUpgradeController.isAbleToUpgrade (upgrade);
		}

		// Create ready button
		readyButton = Instantiate (GameObject.Find ("Ready Button").GetComponent<Button> ()) as Button;

		// Get ready button
		readyButton.transform.SetParent (transform);
	}

	void OnEnable ()
	{
		// Interactiable
		readyButton.interactable = true;
		readyButton.onClick.AddListener (readyPlayer);
	}

	void upgradeClick (Upgrade upgrade, Button button)
	{
		// Upgrade and result in being interactable
		playerUpgradeController.upgrade (upgrade);

		// Is able to upgrade furthur
		button.interactable = playerUpgradeController.isAbleToUpgrade (upgrade);
	}

	public void readyPlayer ()
	{
		// Make player reaedy
		playerController.isReady = true;

		// Call it
		gameManager.readyPlayer (playerController);

		// Disable ready button
		readyButton.interactable = false;
	}
}
