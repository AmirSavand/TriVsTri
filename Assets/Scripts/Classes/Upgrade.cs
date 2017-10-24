using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
	public string title;

	public float amount;

	public int price;

	public Item priceType;

	public int stock;

	public int stockFactor = 25;

	public int getPrice (int playerStock)
	{
		return (int)(price + (playerStock * stockFactor));
	}
}
