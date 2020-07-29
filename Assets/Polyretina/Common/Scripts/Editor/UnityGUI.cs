using UnityEngine;
using UnityEditor;
using Action = System.Action;

namespace LNE.UI
{
	public static class UnityGUI
	{
		/*
		 * Private fields
		 */

		private static Color? guiColour;

		/*
		 * Public properties
		 */

		public static int IndentLevel
		{
			get
			{
				return EditorGUI.indentLevel;
			}

			set
			{
				EditorGUI.indentLevel = value;
			}
		}

		public static bool Enabled
		{
			get
			{
				return GUI.enabled;
			}

			set
			{
				GUI.enabled = value;
			}
		}

		public static GUIOptions BoldLabel { get => new GUIOptions { style = EditorStyles.boldLabel }; }

		/*
		 * Public methods
		 */

		/*
		 * Ease of use
		 */

		public static void Space(int spaces = 1)
		{
			for (int i = 0; i < spaces; i++)
			{
				EditorGUILayout.Space();
			}
		}

		public static void FlexibleSpace()
		{
			GUILayout.FlexibleSpace();
		}

		public static void BeginHorizontal()
		{
			EditorGUILayout.BeginHorizontal();
		}

		public static void EndHorizontal()
		{
			EditorGUILayout.EndHorizontal();
		}

		public static void BeginColour(Color colour)
		{
			guiColour = GUI.color;
			GUI.color = colour;
		}

		public static void EndColour()
		{
			if (guiColour.HasValue)
				GUI.color = guiColour.Value;

			guiColour = null;
		}

		public static void BeginChangeCheck()
		{
			EditorGUI.BeginChangeCheck();
		}

		public static bool EndChangeCheck()
		{
			return EditorGUI.EndChangeCheck();
		}

		/*
		 * Fields
		 */

		public static void Separator()
		{
			Label("_______________".Replace("_", "_______________"));
		}

		public static void Header(string label, bool autoIndent = false)
		{
			if (autoIndent && IndentLevel > 0)
				IndentLevel--;

			Space();
			Label(label, BoldLabel);

			if (autoIndent)
				IndentLevel++;
		}

		public static void Label(string label, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				EditorGUILayout.LabelField(label, options.style, options?.layout);
			}
			else
			{
				EditorGUILayout.LabelField(label, options?.layout);
			}
		}

		public static void Label(string label, string label2, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				EditorGUILayout.LabelField(label, label2, options.style, options?.layout);
			}
			else
			{
				EditorGUILayout.LabelField(label, label2, options?.layout);
			}
		}

		public static void HelpLabel(string label, MessageType messageType)
		{
			EditorGUILayout.HelpBox(label, messageType);
		}

		public static bool Foldout(bool fold, string title, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return EditorGUILayout.Foldout(fold, ToGUIContent(title), true, options.style);
			}
			else
			{
				return EditorGUILayout.Foldout(fold, ToGUIContent(title), true);
			}
		}

		public static bool Button(string title, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return GUILayout.Button(ToGUIContent(title), options.style, options?.layout);
			}
			else
			{
				return GUILayout.Button(ToGUIContent(title), options?.layout);
			}
		}

		public static T Enum<T>(string title, T value, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return (T)(object)EditorGUILayout.EnumPopup(ToGUIContent(title), (System.Enum)(object)value, options.style, options?.layout);
			}
			else
			{
				return (T)(object)EditorGUILayout.EnumPopup(ToGUIContent(title), (System.Enum)(object)value, options?.layout);
			}
		}

		public static float Slider(string title, float value, float min, float max, GUIOptions options = null)
		{
			return EditorGUILayout.Slider(ToGUIContent(title), value, min, max, options?.layout);
		}

		public static bool Toggle(string title, bool value, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return EditorGUILayout.Toggle(ToGUIContent(title), value, options.style, options?.layout);
			}
			else
			{
				return EditorGUILayout.Toggle(ToGUIContent(title), value, options?.layout);
			}
		}

		public static bool ToggleLeft(string title, bool value, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return EditorGUILayout.ToggleLeft(ToGUIContent(title), value, options.style, options?.layout);
			}
			else
			{
				return EditorGUILayout.ToggleLeft(ToGUIContent(title), value, options?.layout);
			}
		}

		public static Vector2 Vector2(string title, Vector2 value, GUIOptions options = null)
		{
			return EditorGUILayout.Vector2Field(ToGUIContent(title), value, options?.layout);
		}

		public static T Object<T>(string title, T value, bool allowSceneObjects, GUIOptions options = null) where T : Object
		{
			return EditorGUILayout.ObjectField(ToGUIContent(title), value, typeof(T), allowSceneObjects, options?.layout) as T;
		}

		public static string TextBox(string title, string text, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return EditorGUILayout.TextField(ToGUIContent(title), text, options.style, options?.layout);
			}
			else
			{
				return EditorGUILayout.TextField(ToGUIContent(title), text, options?.layout);
			}
		}

		public static string TextArea(string text, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return EditorGUILayout.TextArea(text, options.style, options?.layout);
			}
			else
			{
				return EditorGUILayout.TextArea(text, options?.layout);
			}
		}

		public static float Float(string title, float value, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return EditorGUILayout.FloatField(ToGUIContent(title), value, options.style, options?.layout);
			}
			else
			{
				return EditorGUILayout.FloatField(ToGUIContent(title), value, options?.layout);
			}
		}

		public static int DelayedInt(string title, int value, GUIOptions options = null)
		{
			if (options?.style != null)
			{
				return EditorGUILayout.DelayedIntField(ToGUIContent(title), value, options.style, options?.layout);
			}
			else
			{
				return EditorGUILayout.DelayedIntField(ToGUIContent(title), value, options?.layout);
			}
		}

		/*
		 * Misc
		 */

		public static bool OnMouseHoverPrevious(Action toDo = null)
		{
			var hovered = Event.current.type == EventType.Repaint &&
							GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);

			if (hovered)
			{
				toDo?.Invoke();
			}

			return hovered;
		}

		/*
		 * Private methods
		 */

		private static GUIContent ToGUIContent(string title)
		{
			return title != null ? new GUIContent(UnityApp.ToDisplayFormat(title)) : GUIContent.none;
		}
	}
}
