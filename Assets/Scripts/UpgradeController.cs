using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
	public PlayerController player;
	public HitpointController hitpointController;

	public Dictionary<Upgrade, int> stocks = new Dictionary<Upgrade, int> ();

	private Upgrade[] upgrades;

	void Start ()
	{
		// Get upgrades
		upgrades = GameObject.Find ("Data").GetComponentsInChildren<Upgrade> ();

		// Initial upgrade stocks
		foreach (Upgrade upgrade in upgrades) {
			
			// Add it to stocks
			stocks.Add (upgrade, 0);
		}
	}

	public bool upgrade (Upgrade upgrade)
	{
		// Is able to stock more
		if (upgrade.stock == stocks [upgrade]) {
			return false;
		}

		// Pay if has enought resource and stock
		if (!payment (upgrade)) {
			return false;
		}

		// Redice stock
		stocks [upgrade]++;

		// Upgrade player
		if (upgrade.name == "Weapon") {
		
			player.weaponLevel += (int)upgrade.amount;
			return true;

		} else if (upgrade.name == "Bullet Speed") {

			player.firePower += upgrade.amount;
			return true;

		} else if (upgrade.name == "Fire Speed") {
		
			player.fireRate += upgrade.amount;
			return true;
		
		} else if (upgrade.name == "Damage") {

			player.fireDamage += upgrade.amount;
			return true;
		
		} else if (upgrade.name == "Hitpoints") {
		
			hitpointController.maxHitpoints += upgrade.amount;
			hitpointController.updateHitpointSlider ();
			return true;
		}

		// Fail, Upgrade not found in player
		return false;
	}

	public bool payment (Upgrade upgrade)
	{
		// Paying in diamonds
		if (upgrade.priceType.type == "Diamond") {

			// Has enough resource
			if (player.diamonds < upgrade.price) {
				return false;
			}

			// Pay and update UI
			player.diamonds -= upgrade.price;
			player.updateResources ();

			// Payment successful
			return true;
		}

		// Paying in stars
		if (upgrade.priceType.type == "Star") {

			// Has enough resource
			if (player.stars < upgrade.price) {
				return false;
			}

			// Pay and update UI
			player.stars -= upgrade.price;
			player.updateResources ();

			// Payment successful
			return true;
		}

		// Failed paymenet since priceType was not found
		return false;
	}
}
