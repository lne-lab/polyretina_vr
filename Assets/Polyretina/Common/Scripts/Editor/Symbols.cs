using UnityEngine;
using UnityEditor;

namespace LNE
{
	using ArrayExts;

	public static class Symbols
	{
		public static string[] Defined
		{
			get
			{
				var buildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
				var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget)
											.Split(';');

				return symbols;
			}
		}

		public static void DefineAll(string[] symbols)
		{
			var buildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
			PlayerSettings.SetScriptingDefineSymbolsForGroup(
				buildTarget,
				symbols.Converge("", (a, b) => a + ";" + b)
			);

			Debug.Log("Defined Symbols: " + PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget));
		}

		public static bool IsDefined(string symbol)
		{
			return Defined.Contains(symbol);
		}

		public static void Define(string symbol)
		{
			if (IsDefined(symbol) == false)
			{
				DefineAll(Defined.Combine(symbol));
			}
		}

		public static void Undefine(string symbol)
		{
			if (IsDefined(symbol))
			{
				DefineAll(Defined.Remove(symbol));
			}
		}
	}
}
