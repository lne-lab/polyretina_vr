using System;
using UnityEngine;

namespace LNE
{
	/// <summary>
	/// Math-related functions
	/// </summary>
	public static class AuxMath
	{
		/// <summary>
		/// Floating-point hyperbolic tangent
		/// </summary>
		public static float Tanh(float f)
		{
			return (float)Math.Tanh(f);
		}

		/// <summary>
		/// Floor x to a multiple of nearest
		/// </summary>
		public static int FloorToNearest(int x, int nearest)
		{
			return x - x % nearest;
		}

		/// <summary>
		/// Overflow
		/// </summary>
		public static float Overflow(float val, float min, float max)
		{
			var size = max - min;
			if (val < min)
				val += size;
			else if (val > max)
				val -= size;

			return val;
		}
		
		/// <summary>
		/// Returns point at intersection between a plane and a ray
		/// </summary>
		public static Vector3 Intersection(Plane plane, Ray ray)
		{
			plane.Raycast(ray, out var distance);

			return ray.GetPoint(distance);
		}

		/// <summary>
		/// Get horizontal field of view
		/// </summary>
		public static float HorizontalFoV(float vertical, float aspectRatio)
		{
			var radVertical = vertical * Mathf.Deg2Rad;
			var radHorizontal = 2 * Math.Atan(Mathf.Tan(radVertical / 2) * aspectRatio);
			var horizontal = Mathf.Rad2Deg * radHorizontal;

			return (float)horizontal;
		}

		/// <summary>
		/// Normalise value (min = 0, max = 1)
		/// </summary>
		public static float Normalise(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}

		/// <summary>
		/// Normalise value (-bound = 0, +bound = 1)
		/// </summary>
		public static float Normalise(float value, float bound)
		{
			return Normalise(value, -bound, bound);
		}

		/// <summary>
		/// Formalise value
		/// </summary>
		public static float Formalise(float value, float min, float max)
		{
			return value * (max - min) + min;
		}

		/// <summary>
		/// Formalise value
		/// </summary>
		public static float Formalise(float value, float bound)
		{
			return Formalise(value, -bound, bound);
		}

		/// <summary>
		/// Cantor pairing functions. Produces a unique number from two numbers
		/// </summary>
		public static float CantorPair(float a, float b)
		{
			return .5f * (a + b) * (a + b + 1) + b;
		}

		/// <summary>
		/// Element-wise min of two vectors
		/// </summary>
		public static Vector2 Min(Vector2 a, Vector2 b)
		{
			return new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
		}

		/// <summary>
		/// Element-wise max of two vectors
		/// </summary>
		public static Vector2 Max(Vector2 a, Vector2 b)
		{
			return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		/// <summary>
		/// Element-wise clamp of vector
		/// </summary>
		public static Vector2 Clamp(Vector2 a, Vector2 min, Vector2 max)
		{
			return new Vector2(Mathf.Clamp(a.x, min.x, max.x), Mathf.Clamp(a.y, min.y, max.y));
		}
	}
}
