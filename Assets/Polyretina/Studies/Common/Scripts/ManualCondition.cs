#pragma warning disable 649

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LNE.Studies
{
	using ProstheticVision;
	
	/// <summary>
	/// Manually sets a studies independent variables
	/// </summary>
	public class ManualCondition : MonoBehaviour
	{
		[SerializeField]
		private Study _study;

		[SerializeField]
		private ElectrodeLayout _resolution;

		[SerializeField]
		private float _visualAngle;

		[SerializeField]
		private float _tailLength;
		
		public void SetResolution()
		{
			_study.SetElectrodeLayout(_resolution);
		}

		public void SetVisualAngle()
		{
			_study.SetVisualAngle(_visualAngle);
		}

		public void SetTailLength()
		{
			_study.SetTailLength(_tailLength);
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ManualCondition))]
	public class ManualConditionEditor : Editor
	{
		private SerializedProperty study;
		private SerializedProperty resolution;
		private SerializedProperty visualAngle;
		private SerializedProperty tailLength;

		void OnEnable()
		{
			study = serializedObject.FindProperty("_study");
			resolution = serializedObject.FindProperty("_resolution");
			visualAngle = serializedObject.FindProperty("_visualAngle");
			tailLength = serializedObject.FindProperty("_tailLength");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as ManualCondition), typeof(ManualCondition), false);
			GUI.enabled = true;

			EditorGUILayout.PropertyField(study);

			DrawProperty(resolution, (target as ManualCondition).SetResolution);
			DrawProperty(visualAngle, (target as ManualCondition).SetVisualAngle);
			DrawProperty(tailLength, (target as ManualCondition).SetTailLength);
			
			serializedObject.ApplyModifiedProperties();
		}

		private void DrawProperty(SerializedProperty property, Action action)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(property);
			if (GUILayout.Button("Set", GUILayout.Width(45), GUILayout.Height(14)))
			{
				action();
			}
			EditorGUILayout.EndHorizontal();

		}
	}
#endif
}
