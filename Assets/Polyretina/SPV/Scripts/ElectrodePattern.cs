using System.Collections.Generic;
using UnityEngine;

namespace LNE.ProstheticVision
{
	public enum ElectrodePattern
	{
		POLYRETINA
	}

	public static class ElectrodePatternExtensions
	{
		public static Vector2[] GetElectrodePositions(this ElectrodePattern that, ElectrodeLayout layout, float fov = 120)
		{
			switch (that)
			{
				case ElectrodePattern.POLYRETINA:	return Polyretina(layout, fov);
				default:							return null;
			}
		}
		
		private static Vector2[] Polyretina(ElectrodeLayout layout, float fov)
		{
			var pattern = new List<Vector2>();
			
			var retinalRadius = CoordinateSystem.FovToRetinalRadius(fov);

			var verticalSpacing = layout.GetSpacing(LayoutUsage.Anatomical);
			var horizontalSpacing = verticalSpacing * Mathf.Sqrt(3) / 2f;

			var offset = 0f;
			for (var x = -retinalRadius; x < retinalRadius; x += horizontalSpacing)
			{
				offset = offset == 0f ? verticalSpacing / 2f : 0f;
				for (var y = -retinalRadius + offset; y < retinalRadius; y += verticalSpacing)
				{
					var position = new Vector2(x, y);
					if (position.magnitude <= retinalRadius)
					{
						pattern.Add(position);
					}
				}
			}

			return pattern.ToArray();
		}
	}
}
