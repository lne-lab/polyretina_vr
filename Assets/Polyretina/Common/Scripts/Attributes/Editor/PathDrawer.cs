using System.IO;
using UnityEngine;
using UnityEditor;

namespace LNE.UI.Attributes
{
	[CustomPropertyDrawer(typeof(PathAttribute))]
	public class PathDrawer : ExtendedAttributeDrawer<PathAttribute>
	{
		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			BeginGUI(rect, property, label);

			var path = attribute.isRelative ? UnityApp.MakeAbsolute(property.stringValue) : property.stringValue;
			var exists = attribute.isFile ? File.Exists(path) : Directory.Exists(path);

			Rect textRect, buttonRect;
			SplitFromRight(28, out textRect, out buttonRect);

			var prevCol = SetColour(exists ? Pastel.Green : Pastel.Red);
			EditorGUI.PropertyField(textRect, property, Label);
			SetColour(prevCol);

			if (GUI.Button(buttonRect, "..."))
			{
				path = exists ? path : UnityApp.DataPath;

				if (attribute.isFile)
				{
					path = EditorUtility.OpenFilePanel("Select file...", path, "");
				}
				else
				{
					path = EditorUtility.OpenFolderPanel("Select folder...", path, "") + "/";
				}

				if (path == "" || path == "/")
				{
					return;
				}

				property.stringValue = attribute.isRelative ? UnityApp.MakeRelative(path, false) : path;
			}

			EndGUI();
		}
	}
}
