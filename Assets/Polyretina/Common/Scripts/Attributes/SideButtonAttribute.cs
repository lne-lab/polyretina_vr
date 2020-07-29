using UnityEngine;

namespace LNE.UI.Attributes
{
	/// <summary>
	/// Displays a button next to field which invokes a callback method (e.g., a setter method for values that should only be updated manually)
	/// </summary>
	public class SideButtonAttribute : PropertyAttribute
	{
		public string buttonLabel;
		public bool executeInEditMode;

		public string MethodName { get; private set; }

		public SideButtonAttribute(string methodName)
		{
			MethodName = methodName;
		}
	}
}
