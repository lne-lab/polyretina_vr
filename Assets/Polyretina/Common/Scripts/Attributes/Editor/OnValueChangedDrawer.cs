using UnityEditor;

namespace LNE.UI.Attributes
{
	[CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
	public class OnValueChangedDrawer : ExtendedAttributeDrawer<OnValueChangedAttribute>
	{
		// empty == default drawer
	}
}
