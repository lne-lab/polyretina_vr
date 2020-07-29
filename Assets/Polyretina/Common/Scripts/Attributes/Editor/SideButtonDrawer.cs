using UnityEngine;
using UnityEditor;

namespace LNE.UI.Attributes
{
	using Reflection;

	[CustomPropertyDrawer(typeof(SideButtonAttribute))]
	public class SideButtonDrawer : ExtendedAttributeDrawer<SideButtonAttribute>
	{
		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			BeginGUI(rect, property, label);
			SplitFromRight(28, out var propertyRect, out var buttonRect);

			EditorGUI.PropertyField(propertyRect, property);
			var pressed = GUI.Button(buttonRect, attribute.buttonLabel);
			if (pressed && (Application.isPlaying || ExecuteInEditMode || attribute.executeInEditMode))
			{
				Mirror.InvokeMethod(
					property.serializedObject.targetObject, 
					attribute.MethodName, 
					fieldInfo
				);
			}

			EndGUI();
		}
	}
}
