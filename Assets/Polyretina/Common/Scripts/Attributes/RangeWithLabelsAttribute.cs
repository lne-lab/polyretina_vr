using UnityEngine;

namespace LNE.UI.Attributes
{
	/// <summary>
	/// Displays the float or int as a range with labels above the min, median and max points
	/// </summary>
	public class RangeWithLabelsAttribute : PropertyAttribute
	{
		public float Min { get; private set; }
		public float Max { get; private set; }
		public string Label1 { get; private set; }
		public string Label2 { get; private set; }
		public string Label3 { get; private set; }

		public RangeWithLabelsAttribute(float min, float max, string label1, string label2, string label3)
		{
			Min = min;
			Max = max;
			Label1 = label1;
			Label2 = label2;
			Label3 = label3;
		}
	}
}
