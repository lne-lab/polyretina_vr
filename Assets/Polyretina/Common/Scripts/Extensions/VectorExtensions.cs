using UnityEngine;

namespace LNE.VectorExts
{
	/// <summary>
	/// Collection of useful methods for vectors
	/// </summary>
	public static class VectorExtensions
	{
		/*
		 * Divide
		 */

		public static Vector3 DivideXYZ(this Vector3 a, Vector3 b)
		{
			return new Vector3(
				a.x / b.x,
				a.y / b.y,
				a.z / b.z
			);
		}

		public static Vector3 DivideXYZ(this Vector3 a, float x, float y, float z)
		{
			return new Vector3(
				a.x / x,
				a.y / y,
				a.z / z
			);
		}

		public static Vector3 DivideXY(this Vector3 a, float x, float y)
		{
			return new Vector3(
				a.x / x,
				a.y / y,
				a.z
			);
		}

		public static Vector3 DivideX(this Vector3 a, float x)
		{
			return new Vector3(
				a.x / x,
				a.y,
				a.z
			);
		}

		public static Vector2 DivideXY(this Vector2 a, float xy)
		{
			return new Vector2(
				a.x / xy,
				a.y / xy
			);
		}

		public static Vector2 DivideXY(this Vector2 a, float x, float y)
		{
			return new Vector2(
				a.x / x,
				a.y / y
			);
		}

		/*
		 * Add
		 */

		public static Vector3 AddZ(this Vector3 a, float z)
		{
			return a + new Vector3(0, 0, z);
		}

		public static Vector2 AddXY(this Vector2 a, float xy)
		{
			return a + new Vector2(xy, xy);
		}

		/*
		 * Subtract
		 */

		public static Vector3 SubstractXYZ(this Vector3 a, float xyz)
		{
			return a - new Vector3(xyz, xyz, xyz);
		}

		public static Vector3 SubtractXY(this Vector3 a, float xy)
		{
			return a - new Vector3(xy, xy, 0);
		}

		public static Vector3 SubtractXY(this Vector3 a, float x, float y)
		{
			return a - new Vector3(x, y, 0);
		}

		public static Vector3 SubtractXY(this Vector3 a, Vector2 b)
		{
			return a - new Vector3(b.x, b.y, 0);
		}

		/*
		 * Set
		 */

		public static Vector3 SetZ(this Vector3 a, float z)
		{
			return new Vector3(a.x, a.y, z);
		}

		/*
		 * Conversions
		 */

		public static Color ToColour(this Vector2 a)
		{
			return new Color(a.x, a.y, 0f, 1f);
		}

		public static Vector2 XY(this Vector3 a)
		{
			return new Vector2(a.x, a.y);
		}
	}
}
