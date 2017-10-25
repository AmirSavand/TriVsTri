using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeController : MonoBehaviour
{
	public int changeSpeedTo = -1;

	public float changeSpeed (float speed)
	{
		return Mathf.Abs (speed) * changeSpeedTo;
	}
}
