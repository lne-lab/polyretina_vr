using UnityEditor;
using UnityEngine;

namespace LNE.UI
{
	using SerializedPropertyExts;

	public class ExtendedEditor<T> : Editor where T : MonoBehaviour
	{
		protected new T target;

		public void BeginInspectorGUI()
		{
			serializedObject.Update();

			target = base.target as T;

			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target), typeof(T), false);
			GUI.enabled = true;
		}

		public void EndInspectorGUI()
		{
			serializedObject.ApplyModifiedProperties();
		}

		protected SerializedProperty Field(bool showLabel, params string[] path)
		{
			return Field(showLabel, FindProperty(path));
		}

		protected SerializedProperty Field(string customLabel, params string[] path)
		{
			return Field(customLabel, FindProperty(path));
		}

		protected T1 Field<T1>(bool showLabel, params string[] path)
		{
			return Field<T1>(showLabel, FindProperty(path));
		}

		protected T1 Field<T1>(string customLabel, params string[] path)
		{
			return Field<T1>(customLabel, FindProperty(path));
		}

		protected SerializedProperty Field(bool showLabel, SerializedProperty property, params string[] path)
		{
			property = FindProperty(property, path);

			if (showLabel)
			{
				EditorGUILayout.PropertyField(property);
			}
			else
			{
				EditorGUILayout.PropertyField(property, GUIContent.none);
			}

			return property;
		}

		protected SerializedProperty Field(string customLabel, SerializedProperty property, params string[] path)
		{

			property = FindProperty(property, path);

			EditorGUILayout.PropertyField(property, new GUIContent(UnityApp.ToDisplayFormat(customLabel)));
			return property;
		}

		protected T1 Field<T1>(bool showLabel, SerializedProperty property, params string[] path)
		{

			property = FindProperty(property, path);

			if (showLabel)
			{
				EditorGUILayout.PropertyField(property);
			}
			else
			{
				EditorGUILayout.PropertyField(property, GUIContent.none);
			}

			return property.GetValue<T1>();
		}

		protected T1 Field<T1>(string customLabel, SerializedProperty property, params string[] path)
		{

			property = FindProperty(property, path);

			EditorGUILayout.PropertyField(property, new GUIContent(UnityApp.ToDisplayFormat(customLabel)));
			return property.GetValue<T1>();
		}

		protected bool Foldout(string customLabel, params string[] path)
		{
			return Foldout(customLabel, FindProperty(path));
		}

		protected bool Foldout(string customLabel, SerializedProperty property, GUIOptions options = null)
		{
			property.boolValue = UnityGUI.Foldout(property.boolValue, customLabel, options);
			return property.boolValue;
		}

		protected bool Foldout(string customLabel, SerializedProperty property, string path, GUIOptions options = null)
		{
			return Foldout(customLabel, FindProperty(property, path), options);
		}

		protected bool Foldout(string customLabel, SerializedProperty property, string[] path, GUIOptions options = null)
		{
			return Foldout(customLabel, FindProperty(property, path), options);
		}

		protected SerializedProperty FindProperty(params string[] path)
		{
			Debug.Assert(path != null && path.Length > 0);

			var property = serializedObject.FindProperty(path[0]);
			for (int i = 1; i < path.Length; i++)
			{
				property.FindPropertyRelative(path[i]);
			}

			return property;
		}

		protected SerializedProperty FindProperty(SerializedProperty property, params string[] path)
		{
			Debug.Assert(path != null);

			foreach (var fieldName in path)
			{
				property = property.FindPropertyRelative(fieldName);
			}

			return property;
		}
	}
}
