using UnityEngine;
using UnityEditor;

namespace LNE.UI
{
	[CustomPropertyDrawer(typeof(EditorButton))]
	public class EditorButtonDrawer : ExtendedPropertyDrawer
	{
		[InitializeOnLoadMethod]
		static void AddDrawer()
		{
			AddDrawer(typeof(EditorButton), OnGUI, null);
		}

		public static void OnGUI(ExtendedPropertyDrawer drawer)
		{
			var pressed = drawer.Button();
			if (pressed && (Application.isPlaying || drawer.ExecuteInEditMode))
			{
				drawer.GetTarget<EditorButton>().Invoke();
			}
		}
	}
}
