using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	public void Rotate(float angle)
	{
		transform.RotateAround(target.position, Vector3.up, angle);
	}
}
