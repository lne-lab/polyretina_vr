using UnityEditor;

namespace LNE.UI
{
	/// <summary>
	/// Puts braces surrounding the custom menu items
	/// </summary>
	public class MenuItemBraces
	{
		[MenuItem("(/", priority = int.MinValue)]
		static void OpenBrace()
		{

		}

		[MenuItem(")/", priority = int.MaxValue)]
		static void CloseBrace()
		{

		}
	}
}
