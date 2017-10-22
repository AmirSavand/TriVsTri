using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public GameObject player;
	private PlayerController playerController;
	private UpgradeController playerUpgradeController;

	private GameManager gameManager;

	public Button weaponButton;
	public Button fireRateButton;
	public Button firePowerButton;
	public Button maxHitpointsButton;
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
			GameObject button = Instantiate (panelButton);

			// Attach to panel
			button.transform.SetParent (transform);

			// Set texts
			button.GetComponentsInChildren<Text> () [0].text = "+" + Mathf.Abs (upgrade.amount);
			button.GetComponentsInChildren<Text> () [1].text = "" + upgrade.title;
			button.GetComponentsInChildren<Text> () [2].text = "" + upgrade.price;

			// Set price type
			button.transform.Find ("Price " + upgrade.priceType.type).gameObject.SetActive (true);

			// On click event
			button.GetComponent<Button> ().onClick.AddListener (() => upgradeClick (upgrade));
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

	void upgradeClick (Upgrade upgrade)
	{
		// Upgrade via controller
		playerUpgradeController.upgrade (upgrade);
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

	public void upgradeWeapon ()
	{
		// If payment succesfull
		if (payUpgrade (weaponButton)) {

			// Decrease fire rate
			playerController.weaponLevel += (int)getButtonUpgradeAmount (weaponButton);
		}
	}

	public void upgradeFireRate ()
	{
		// If payment succesfull
		if (payUpgrade (fireRateButton)) {
			
			// Decrease fire rate
			playerController.fireRate -= 0.1f;
		}
	}

	public void upgradeFirePower ()
	{
		// If payment succesfull
		if (payUpgrade (firePowerButton)) {

			// Decrease fire rate
			playerController.firePower += getButtonUpgradeAmount (firePowerButton);
		}
	}

	public void upgradeMaxHitpoints ()
	{
		// If payment succesfull
		if (payUpgrade (maxHitpointsButton)) {
		
			// Increase max HP and update UI
			player.GetComponent<HitpointController> ().maxHitpoints += getButtonUpgradeAmount (maxHitpointsButton);
			player.GetComponent<HitpointController> ().updateHitpointSlider ();
		}
	}

	private float getButtonUpgradeAmount (Button button)
	{
		// Turn the text to float (ex: "+10" returns 10f)
		return int.Parse (button.transform.Find ("Upgrade Amount").GetComponent<Text> ().text);
	}

	private int getButtonUpgradePrice (Button button)
	{
		// Turn the text to int (ex: "10" returns 10)
		return int.Parse (button.transform.Find ("Price").GetComponent<Text> ().text);
	}

	private string getButtonUpgradePriceType (Button button)
	{
		// Stars
		if (button.transform.Find ("Price Star")) {
			return "Star";
		}
		// Diamonds
		if (button.transform.Find ("Price Diamond")) {
			return "Diamond";
		}
		// Not found
		return "None";
	}

	private bool payUpgrade (Button button)
	{
		// Get price
		int price = getButtonUpgradePrice (button);

		// Get price resource
		string priceType = getButtonUpgradePriceType (button);

		// Pay in stars
		if (priceType == "Star") {

			// Has enough resource
			if (playerController.stars < price) {
				return false;
			}

			// Pay and update UI
			playerController.stars -= price;
			playerController.updateResources ();
			return true;
		}

		// Pay in diamonds
		if (priceType == "Diamond") {

			// Has enough resource
			if (playerController.diamonds < price) {
				return false;
			}

			// Pay and update UI
			playerController.diamonds -= price;
			playerController.updateResources ();
			return true;
		}

		// Failed paymenet since priceType was not found
		return false;
	}
}
