using System;
using System.Reflection;
using UnityEditor;

namespace LNE.UI
{
	using ArrayExts;

	[CustomEditor(typeof(EnumDropdown))]
	public class EnumDropdownEditor : ExtendedEditor<EnumDropdown>
	{
		private SerializedProperty dropdownProp;
		private SerializedProperty enumNameProp;
		private SerializedProperty startingProp;

		void OnEnable()
		{
			dropdownProp = serializedObject.FindProperty("_dropdown");
			enumNameProp = serializedObject.FindProperty("_enumName");
			startingProp = serializedObject.FindProperty("_startingValue");
		}

		public override void OnInspectorGUI()
		{
			BeginInspectorGUI();

			EditorGUILayout.PropertyField(dropdownProp);

			var enumTypes = GetEnumFullNames();
			var currType = enumNameProp.stringValue;

			var typeIndex = EditorGUILayout.Popup("Enum Type", enumTypes.IndexOfOrZero(currType), GetEnumNames());
			enumNameProp.stringValue = enumTypes[typeIndex];

			var myEnumType = GetEnumTypes()[typeIndex];
			var myEnum = (Enum)Enum.ToObject(myEnumType, startingProp.intValue);
			startingProp.intValue = (int)(object)EditorGUILayout.EnumPopup("Starting Value", myEnum);

			EndInspectorGUI();
		}

		private string[] GetEnumNames()
		{
			return GetEnumTypes().Convert((t) => t.Name);
		}

		private string[] GetEnumFullNames()
		{
			return GetEnumTypes().Convert((t) => t.FullName);
		}

		private Type[] GetEnumTypes()
		{
			var enumTypes = Assembly.GetAssembly(typeof(EnumDropdown))
									.GetTypes()
									.Where((t) => t.IsEnum);

			return enumTypes;
		}
	}
}
