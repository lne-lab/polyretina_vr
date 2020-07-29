using System.Reflection;
using UnityEngine;

namespace LNE.UI.Attributes
{
	using Reflection;

	/// <summary>
	/// Invokes callback method when value is changed in the editor
	/// </summary>
	public class OnValueChangedAttribute : PropertyAttribute
	{
		public bool executeInEditMode;

		public string MethodName { get; private set; }

		public OnValueChangedAttribute(string methodName)
		{
			MethodName = methodName;
			order = -1;
		}

		public virtual void Invoke(MonoBehaviour component, FieldInfo field)
		{
			Mirror.InvokeMethod(component, MethodName, field);
		}
	}
}
