using System.Reflection;
using UnityEngine;

namespace LNE.UI.Attributes
{
	using Reflection;
	using StringExts;

	/// <summary>
	/// Invokes set method of linked property when value is changed in the editor.
	/// Property is linked by name, removing a preceeding _ and capitalizing
	/// </summary>
	public class LinkWithPropertyAttribute : OnValueChangedAttribute
	{
		public LinkWithPropertyAttribute() : base(null)
		{

		}

		public override void Invoke(MonoBehaviour component, FieldInfo field)
		{
			Mirror.SetProperty(component, FieldToProperty(field.Name), field);
		}

		private string FieldToProperty(string fieldName)
		{
			// e.g., _name -> Name
			return fieldName.Substring(1).FirstCharToUpper();
		}
	}
}
