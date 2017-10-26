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

	public PlayerReadyController readyButtonController;

	public Upgrade[] upgrades;

	void Awake ()
	{
		// Get components
		playerController = player.GetComponent<PlayerController> ();
		playerUpgradeController = player.GetComponent<UpgradeController> ();
		gameManager = GameObject.Find ("Game").GetComponent<GameManager> ();

		// Assign this shop to player
		playerController.shopManager = GetComponent<ShopManager> ();

		// Get upgrades
		upgrades = GameObject.Find ("Data").GetComponentsInChildren<Upgrade> ();

		// Panel button
		GameObject panelButton = GameObject.Find ("Panel Button");

		// Create upgrade buttons
		foreach (Upgrade upgrade in upgrades) {
			
			// Clone button
			GameObject buttonGameObject = Instantiate (panelButton);
			Button button = buttonGameObject.GetComponent<Button> ();

			// Attach to panel and set name
			buttonGameObject.transform.SetParent (transform, false);
			buttonGameObject.name = upgrade.title;

			// Set texts
			buttonGameObject.GetComponentsInChildren<Text> () [0].text = "+" + Mathf.Abs (upgrade.amount);
			buttonGameObject.GetComponentsInChildren<Text> () [1].text = "" + upgrade.title;
			buttonGameObject.GetComponentsInChildren<Text> () [2].text = "" + upgrade.price;

			// Set price type
			button.transform.Find ("Price " + upgrade.priceType.type).gameObject.SetActive (true);

			// On click
			button.onClick.AddListener (() => upgradeClick (upgrade, button));
		}

		// Create ready button and assign player to it
		readyButtonController = Instantiate (GameObject.Find ("Ready Button").GetComponent<PlayerReadyController> ()) as PlayerReadyController;
		readyButtonController.transform.SetParent (transform, false);
		readyButtonController.player = player.GetComponent<PlayerController> ();
	}

	void OnEnable ()
	{
		// Update upgrade buttons status
		updateUpgradeButtonStatus ();
	}

	public void updateUpgradeButtonStatus ()
	{
		// All upgrades
		foreach (Upgrade upgrade in upgrades) {

			// Get the upgrade button
			Button button = transform.Find (upgrade.title).GetComponent<Button> ();

			// Update interactable status
			button.interactable = playerUpgradeController.isAbleToUpgrade (upgrade);

			// Update price
			button.transform.Find ("Price").GetComponent<Text> ().text = "" + upgrade.getPrice (playerUpgradeController.stocks [upgrade]);
		}
	}

	void upgradeClick (Upgrade upgrade, Button button)
	{
		// Upgrade and result in being interactable
		playerUpgradeController.upgrade (upgrade);

		// Update status
		updateUpgradeButtonStatus ();
	}
}
