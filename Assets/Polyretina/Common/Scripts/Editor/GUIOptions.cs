using System.Collections.Generic;
using UnityEngine;

namespace LNE.UI
{
	public class GUIOptions
	{
		public GUIStyle style = null;
		public int width = -1;
		public int height = -1;
		public int maxWidth = -1;
		public int maxHeight = -1;

		public GUILayoutOption[] layout
		{
			get
			{
				var options = new List<GUILayoutOption>();

				if (width >= 0)
				{
					options.Add(GUILayout.Width(width));
				}

				if (height >= 0)
				{
					options.Add(GUILayout.Height(height));
				}

				if (maxWidth >= 0)
				{
					options.Add(GUILayout.Width(maxWidth));
				}

				if (maxHeight >= 0)
				{
					options.Add(GUILayout.Height(maxHeight));
				}

				return options.ToArray();
			}
		}
	}
}
