using UnityEngine;

namespace LNE.ProstheticVision
{
	public static class PhospheneMatrix
	{
		/*
		 * Public methods
		 */

		public static Vector2[,] CalculateMatrix(HeadsetModel headset, ElectrodePattern pattern, ElectrodeLayout layout)
		{
			return CalculateMatrix(headset, pattern.GetElectrodePositions(layout));
		}

		public static Vector2[,] CalculateMatrix(HeadsetModel headset, Vector2[] electrodes)
		{
			var width = headset.GetWidth();
			var height = headset.GetHeight();

			var matrix = new Vector2[width, height];

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					matrix[i, j] = CalculatePoint(i, j, headset, electrodes);
				}
			}

			return matrix;
		}

		public static Vector2 CalculatePoint(int x, int y, HeadsetModel headset, Vector2[] electrodes)
		{
			var retinalPos = CoordinateSystem.PixelToRetina(x, y, headset);
			return FindClosestElectrode(electrodes, retinalPos);
		}

		/*
		 * Private methods
		 */

		private static Vector2 FindClosestElectrode(Vector2[] electrodes, Vector2 retinalPos)
		{
			// asserts
			Debug.Assert(electrodes != null && electrodes.Length > 0);

			// algorithm
			var closestElectrode = electrodes[0];
			var closestDistance = Vector2.Distance(retinalPos, closestElectrode);
			for (int i = 1; i < electrodes.Length; i++)
			{
				var currentDistance = Vector2.Distance(retinalPos, electrodes[i]);
				if (currentDistance < closestDistance)
				{
					closestElectrode = electrodes[i];
					closestDistance = currentDistance;
				}
			}

			return closestElectrode;
		}
	}
}
