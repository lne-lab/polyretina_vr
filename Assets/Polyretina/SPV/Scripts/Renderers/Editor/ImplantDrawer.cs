using UnityEditor;

namespace LNE.ProstheticVision.UI
{
	using LNE.UI;

	[CustomPropertyDrawer(typeof(Implant))]
	public class ImplantDrawer : ExtendedPropertyDrawer
	{
		[InitializeOnLoadMethod]
		static void AddDrawer()
		{
			AddDrawer(typeof(Implant), ImageRendererDrawer.OnGUI, ImageRendererDrawer.GetPropertyHeight);
		}
	}
}
