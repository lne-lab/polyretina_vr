using UnityEditor;

namespace LNE.UI.Attributes
{
	[CustomPropertyDrawer(typeof(LinkWithPropertyAttribute))]
	public class LinkWithPropertyDrawer : ExtendedAttributeDrawer<LinkWithPropertyAttribute>
	{
		// empty == default drawer
	}
}
