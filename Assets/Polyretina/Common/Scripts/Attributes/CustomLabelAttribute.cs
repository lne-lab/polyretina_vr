using UnityEngine;

namespace LNE.UI.Attributes
{
	/// <summary>
	/// Customizes the editor field's label
	/// </summary>
	public class CustomLabelAttribute : PropertyAttribute
	{
		public bool drawLabel = true;
		public string label = null;

		public GUIContent GUIContent
		{
			get
			{
				if (drawLabel)
				{
					if (label == null)
					{
						return null;
					}
					else if (label == "")
					{
						return new GUIContent(" ");
					}
					else
					{
						return new GUIContent(label);
					}
				}
				else
				{
					return GUIContent.none;
				}
			}
		}

		public CustomLabelAttribute()
		{
			order = -1;
		}
	}
}
