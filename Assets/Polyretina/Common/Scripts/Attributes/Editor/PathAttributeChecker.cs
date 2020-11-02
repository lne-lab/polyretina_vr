using System.IO;
using System.Reflection;
using UnityEngine;

namespace LNE.UI.Attributes
{
	using ArrayExts;
	using ReflectionExts;

	public class PathAttributeChecker
	{
		private const BindingFlags BINDING_FLAGS = BindingFlags.Instance |
													BindingFlags.DeclaredOnly |
													BindingFlags.Public |
													BindingFlags.NonPublic;

		[RuntimeInitializeOnLoadMethod]
		static void CheckPaths()
		{
			var monoBehaviours = UnityApp.FindAllObjectsOfType<MonoBehaviour>();
			foreach (var behaviour in monoBehaviours)
			{
				if (behaviour == null)
					continue;

				var fields = behaviour.GetType()
										.GetFields(BINDING_FLAGS)
										.Where((f) => f.HasCustomAttribute<PathAttribute>());

				foreach (var field in fields)
				{
					CheckPathField(behaviour, field);
				}
			}
		}

		private static void CheckPathField(MonoBehaviour behaviour, FieldInfo field)
		{
			var attribute = field.GetCustomAttribute<PathAttribute>();
			var value = field.GetValue<string>(behaviour);
			var path = attribute.isRelative ? UnityApp.MakeAbsolute(value) : value;
			var exists = attribute.isFile ? File.Exists(path) : Directory.Exists(path);
			if (exists == false && Settings.QuitForInvalidPaths)
			{
				Debug.LogError($"Invalid path \"{path}\" on the GameObject ({behaviour.name}).");
				UnityApp.Quit();
			}
		}
	}
}
