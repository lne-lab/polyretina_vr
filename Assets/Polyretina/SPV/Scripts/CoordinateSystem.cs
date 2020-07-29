using UnityEngine;

namespace LNE.ProstheticVision
{
	/// <summary>
	/// Angle to Polar to Retina to Pixel conversions
	/// </summary>
	public static class CoordinateSystem
	{
		/*
		 * Public properties
		 */

		public static Vector2 OpticDisc { get; } = new Vector2(15, 2);

		/*
		 * Public methods
		 */

		/// <summary>
		/// Visual Angle (deg) -> Polar (rho, phi)
		/// </summary>
		public static Vector2 VisualAngleToPolar(Vector2 visualAngle)
		{
			var modified = new Vector2();
			modified.x = visualAngle.x - OpticDisc.x;

			if (visualAngle.x > 0)
			{
				modified.y = visualAngle.y - OpticDisc.y * Mathf.Pow(visualAngle.x / OpticDisc.x, 2);
			}
			else
			{
				modified.y = visualAngle.y;
			}

			var polar = new Vector2();
			polar.x = Mathf.Sqrt(Mathf.Pow(modified.x, 2) + Mathf.Pow(modified.y, 2));
			polar.y = Mathf.Atan2(modified.y, modified.x) * Mathf.Rad2Deg;

			return polar;
		}

		/// <summary>
		/// Polar (rho, phi) -> Visual Angle (deg)
		/// </summary>
		public static Vector2 PolarToVisualAngle(float rho, float phi)
		{
			var cartesian = new Vector2();

			var prime = rho * Mathf.Cos(phi * Mathf.Deg2Rad);

			cartesian.x = prime + OpticDisc.x;
			cartesian.y = rho * Mathf.Sin(phi * Mathf.Deg2Rad);
			
			if (prime > -OpticDisc.x)
			{
				cartesian.y += OpticDisc.y * Mathf.Pow(cartesian.x / OpticDisc.x, 2);
			}
			
			return cartesian;
		}

		/// <summary>
		/// Polar (rho, phi) -> Visual Angle (deg)
		/// </summary>
		public static Vector2 PolarToVisualAngle(Vector2 polar)
		{
			return PolarToVisualAngle(polar.x, polar.y);
		}

		/// <summary>
		/// Visual Angle (deg) -> Retinal position (um)
		/// </summary>
		public static float VisualAngleToRetina(float visualAngle)
		{
			var sign = Mathf.Sign(visualAngle);
			visualAngle = Mathf.Abs(visualAngle);
			var r_mm = 0.268f * visualAngle + 3.427e-4f * Mathf.Pow(visualAngle, 2) - 8.3309e-6f * Mathf.Pow(visualAngle, 3);
			var r_um = 1e3f * r_mm;
			return sign * r_um;
		}

		/// <summary>
		/// Visual Angle (deg) -> Retinal position (um)
		/// </summary>
		public static Vector2 VisualAngleToRetina(Vector2 visualAngle)
		{
			return new Vector2(
				VisualAngleToRetina(visualAngle.x), 
				VisualAngleToRetina(visualAngle.y)
			);
		}

		/// <summary>
		/// Retinal position (um) -> Visual Angle (deg)
		/// </summary>
		public static float RetinaToVisualAngle(float retina)
		{
			var sign = Mathf.Sign(retina);
			var r_mm = 1e-3f * Mathf.Abs(retina);
			var r_deg = 3.556f * r_mm + 0.05993f * Mathf.Pow(r_mm, 2) - 0.007358f * Mathf.Pow(r_mm, 3);
			r_deg += 3.027e-4f * Mathf.Pow(r_mm, 4);
			return sign * r_deg;
		}

		/// <summary>
		/// Retinal position (um) -> Visual Angle (deg)
		/// </summary>
		public static Vector2 RetinaToVisualAngle(Vector2 retina)
		{
			return new Vector2(
				RetinaToVisualAngle(retina.x), 
				RetinaToVisualAngle(retina.y)
			);
		}

		/// <summary>
		/// Retinal position (um) -> Pixel (i, j)
		/// </summary>
		public static Vector2 RetinaToPixel(Vector2 retina, HeadsetModel headset)
		{
			var horizontalDiameter = headset.GetRetinalDiameter(Axis.Horizontal);
			var verticalDiameter = headset.GetRetinalDiameter(Axis.Vertical);

			var x = (retina.x / horizontalDiameter + .5f) * headset.GetWidth();
			var y = (retina.y / verticalDiameter + .5f) * headset.GetHeight();

			return new Vector2(x, y);
		}

		/// <summary>
		/// Pixel (i, j) -> Visual Angle (um)
		/// </summary>
		public static Vector2 PixelToRetina(int i, int j, HeadsetModel headset)
		{
			var horizontalDiameter = headset.GetRetinalDiameter(Axis.Horizontal);
			var verticalDiameter = headset.GetRetinalDiameter(Axis.Vertical);

			var x = ((float)i / headset.GetWidth() - .5f) * horizontalDiameter;
			var y = ((float)j / headset.GetHeight() - .5f) * verticalDiameter;

			return new Vector2(x, y);
		}

		/// <summary>
		/// Visual Angle (deg) -> Pixel (i, j)
		/// </summary>
		public static Vector2 VisualAngleToPixel(Vector2 visualAngle, HeadsetModel headset)
		{
			var retina = VisualAngleToRetina(visualAngle);
			return RetinaToPixel(retina, headset);
		}

		/// <summary>
		/// Pixel (i, j) -> Visual Angle (deg)
		/// </summary>
		public static Vector2 PixelToVisualAngle(int i, int j, HeadsetModel headset)
		{
			var retina = PixelToRetina(i, j, headset);
			return RetinaToVisualAngle(retina);
		}

		/// <summary>
		/// Polar (rho, phi) -> Pixel (i, j)
		/// </summary>
		public static Vector2 PolarToPixel(float rho, float phi, HeadsetModel headset)
		{
			var visualAngle = PolarToVisualAngle(rho, phi);
			return VisualAngleToPixel(visualAngle, headset);
		}

		/// <summary>
		/// Polar (rho, phi) -> Pixel (i, j)
		/// </summary>
		public static Vector2 PolarToPixel(Vector2 polar, HeadsetModel headset)
		{
			return PolarToPixel(polar.x, polar.y, headset);
		}

		/// <summary>
		/// Pixel (i, j) -> Polar (rho, phi)
		/// </summary>
		public static Vector2 PixelToPolar(int i, int j, HeadsetModel headset)
		{
			var visualAngle = PixelToVisualAngle(i, j, headset);
			return VisualAngleToPolar(visualAngle);
		}

		/// <summary>
		/// Polar (rho, phi) -> Retinal position (um)
		/// </summary>
		public static Vector2 PolarToRetina(float rho, float phi)
		{
			var visualAngle = PolarToVisualAngle(rho, phi);
			return VisualAngleToRetina(visualAngle);
		}

		/// <summary>
		/// Polar (rho, phi) -> Retinal position (um)
		/// </summary>
		public static Vector2 PolarToRetina(Vector2 polar)
		{
			return PolarToRetina(polar.x, polar.y);
		}

		/// <summary>
		/// Retinal position (um) -> Polar (rho, phi)
		/// </summary>
		public static Vector2 RetinaToPolar(Vector2 retina)
		{
			var visualAngle = RetinaToVisualAngle(retina);
			return VisualAngleToPolar(visualAngle);
		}

		/// <summary>
		/// Field of View (deg) -> Retinal radius (um)
		/// </summary>
		public static float FovToRetinalRadius(float fov)
		{
			return VisualAngleToRetina(fov / 2);
		}

		/// <summary>
		/// Field of View (deg) -> Retinal diameter (um)
		/// </summary>
		public static float FovToRetinalDiameter(float fov)
		{
			return FovToRetinalRadius(fov) * 2;
		}
	}
}
