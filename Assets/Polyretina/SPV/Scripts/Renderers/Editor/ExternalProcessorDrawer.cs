using UnityEditor;

namespace LNE.ProstheticVision.UI
{
	using LNE.UI;

	[CustomPropertyDrawer(typeof(ExternalProcessor))]
	public class ExternalProcessorDrawer : ExtendedPropertyDrawer
	{
		[InitializeOnLoadMethod]
		static void AddDrawer()
		{
			AddDrawer(typeof(ExternalProcessor), ImageRendererDrawer.OnGUI, ImageRendererDrawer.GetPropertyHeight);
		}
	}
}
