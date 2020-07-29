using System;
using UnityEngine;

namespace LNE.ProstheticVision
{
	[Serializable]
	public class AxonPoint
	{
		public float phi0;
		public float rho;
		public float b;
		public float c;

		public Vector2 Polar { get => new Vector2(rho, AxonModel.CalculatePhi(phi0, rho, b, c)); }

		public Vector2 VisualAngle { get => CoordinateSystem.PolarToVisualAngle(Polar); }

		public Vector2 Retina { get => CoordinateSystem.PolarToRetina(Polar); }

		public Color Colour { get => new Color(phi0, rho, b, c); }

		public Vector2 ToPixel(HeadsetModel headset)
		{
			return CoordinateSystem.PolarToPixel(Polar, headset);
		}
	}
}
