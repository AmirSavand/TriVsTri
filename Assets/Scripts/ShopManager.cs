using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public GameObject player;
	public PlayerController playerController;

	public Button weaponButton;
	public Button fireRateButton;
	public Button firePowerButton;
	public Button maxHitpointsButton;

	void Start ()
	{
		// Get player controller
		playerController = player.GetComponent<PlayerController> ();

		// Get buttons
		weaponButton = transform.Find ("Weapon Button").GetComponent<Button> ();
		fireRateButton = transform.Find ("Fire Rate Button").GetComponent<Button> ();
		firePowerButton = transform.Find ("Fire Power Button").GetComponent<Button> ();
		maxHitpointsButton = transform.Find ("Max Hitpoints Button").GetComponent<Button> ();

		// Set listeners
		weaponButton.onClick.AddListener (upgradeWeapon);
		fireRateButton.onClick.AddListener (upgradeFireRate);
		firePowerButton.onClick.AddListener (upgradeFirePower);
		maxHitpointsButton.onClick.AddListener (upgradeMaxHitpoints);
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
