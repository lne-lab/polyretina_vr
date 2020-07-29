using UnityEngine;

namespace LNE.UI.Attributes
{
	/// <summary>
	/// Gives the options of selecting a path using a Windows Explorer panel
	/// </summary>
	public class PathAttribute : PropertyAttribute
	{
		public bool isFile;
		public bool isRelative;
	}
}
