using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolColour : MonoBehaviour
{
	public Color colour
	{
		get
		{
			return default;
		}

		set
		{
			foreach (var renderer in GetComponentsInChildren<Renderer>())
			{
				renderer.material.color = value;
			}
		}
	}
}
