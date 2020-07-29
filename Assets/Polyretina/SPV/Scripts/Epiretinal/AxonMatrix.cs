using UnityEngine;

namespace LNE.ProstheticVision
{
	public static class AxonMatrix
	{
		/*
		 * Public methods
		 */
		 
		public static AxonPoint[,] CalculateMatrix(HeadsetModel headset)
		{
			var width = headset.GetWidth();
			var height = headset.GetHeight();

			var matrix = new AxonPoint[width, height];

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					matrix[i, j] = CalculatePoint(i, j, headset);
				}
			}

			return matrix;
		}
		
		public static AxonPoint CalculatePoint(int x, int y, HeadsetModel headset)
		{
			var polar = CoordinateSystem.PixelToPolar(x, y, headset);
			
			var bestPhi0 = ClosestWholePhi0(polar);

			for (int i = 0; i < 5; i++)
			{
				var deviation = .55f / Mathf.Pow(10, i);

				bestPhi0 = ClosestPhi0(polar, bestPhi0 - deviation, bestPhi0 + deviation, 11);
			}

			return new AxonPoint()
			{
				phi0 = bestPhi0,
				rho = polar.x,
				b = AxonModel.CalculateB(bestPhi0),
				c = AxonModel.CalculateC(bestPhi0)
			};
		}
		
		/*
		 * Private methods
		 */

		private static float ClosestWholePhi0(Vector2 polar)
		{
			return ClosestPhi0(polar, -180, 180, 360);
		}

		private static float ClosestPhi0(Vector2 polar, float from, float to, int resolution)
		{
			var increment = (to - from) / resolution;

			var closestPhi0 = float.MaxValue;
			var closestPhi = float.MaxValue;

			for (int i = 0; i < resolution; i++)
			{
				var estPhi0 = from + increment * i;
				estPhi0 = AuxMath.Overflow(estPhi0, -180, 180);
				
				var estPhi = AxonModel.CalculatePhi(estPhi0, polar.x);
				if (Mathf.Abs(polar.y - estPhi) < Mathf.Abs(polar.y - closestPhi))
				{
					closestPhi = estPhi;
					closestPhi0 = estPhi0;
				}
			}

			return closestPhi0;
		}
	}
}
