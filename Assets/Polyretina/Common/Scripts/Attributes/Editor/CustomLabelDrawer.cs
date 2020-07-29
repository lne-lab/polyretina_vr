using UnityEditor;

namespace LNE.UI.Attributes
{
	[CustomPropertyDrawer(typeof(CustomLabelAttribute))]
	public class CustomLabelDrawer : ExtendedAttributeDrawer<CustomLabelAttribute>
	{
		// empty == default drawer
	}
}
