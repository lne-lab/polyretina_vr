using UnityEngine;

namespace LNE.UI
{
	public class ExtendedAttributeDrawer<T> : ExtendedPropertyDrawer where T : PropertyAttribute
	{
		public new T attribute => base.attribute as T;
	}
}
