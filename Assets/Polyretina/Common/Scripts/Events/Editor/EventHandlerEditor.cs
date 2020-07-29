using UnityEngine;
using UnityEditor;

namespace LNE.Events.UI
{
	using LNE.UI;

	public class EventHandlerEditor<T> : ExtendedEditor<T> where T : MonoBehaviour
	{
		/*
		 * Virtual interface
		 */

		protected virtual string TriggerName { get => "Trigger"; }

		/*
		 * Sealed body
		 */

		private SerializedProperty events;

		private GUIOptions FoldoutOptions
		{
			get
			{
				return new GUIOptions()
				{
					style = new GUIStyle(EditorStyles.foldout)
					{
						fixedWidth = 1
					}
				};
			}
		}

		private GUIOptions RemoveButtonOptions
		{
			get
			{
				return new GUIOptions()
				{
					style = new GUIStyle(GUI.skin.button)
					{
						alignment = TextAnchor.MiddleLeft,
						fontSize = 8,
					},
					width = 18,
					height = 14
				};
			}
		}

		private GUIOptions AddButtonOptions
		{
			get
			{
				return new GUIOptions()
				{
					style = new GUIStyle(GUI.skin.button)
					{
						alignment = TextAnchor.MiddleLeft,
						fontSize = 8,
						fontStyle = FontStyle.Bold
					},
					width = 18
				};
			}
		}

		void OnEnable()
		{
			events = serializedObject.FindProperty("_events");
		}

		public override void OnInspectorGUI()
		{
			BeginInspectorGUI();

			if (events.arraySize > 0)
			{
				UnityGUI.Label("Events");
			}

			UnityGUI.IndentLevel++;

			for (int i = 0; i < events.arraySize; i++)
			{
				var deleteIndex = DrawEvent(i);
				if (deleteIndex >= 0)
				{
					events.DeleteArrayElementAtIndex(deleteIndex);
					i--;
				}
			}

			UnityGUI.IndentLevel--;

			DrawAddEventButton();
			EndInspectorGUI();
		}

		private int DrawEvent(int index)
		{
			var event_ = events.GetArrayElementAtIndex(index);

			UnityGUI.BeginHorizontal();

			var foldout = Foldout(TriggerName, event_, "foldout", FoldoutOptions);

			DrawTriggerSpace(event_);

			var deleteIndex = -1;
			if (UnityGUI.Button("X", RemoveButtonOptions))
			{
				deleteIndex = index;
			}

			UnityGUI.EndHorizontal();

			if (foldout)
			{
				Field(true, event_, "unityEvent");
			}

			return deleteIndex;
		}

		private void DrawTriggerSpace(SerializedProperty event_)
		{
			UnityGUI.FlexibleSpace();
			UnityGUI.IndentLevel -= 4;
			Field(false, event_, "trigger");
			UnityGUI.IndentLevel += 4;
		}

		private void DrawAddEventButton()
		{
			UnityGUI.BeginHorizontal();
			UnityGUI.FlexibleSpace();
			if (UnityGUI.Button("+", AddButtonOptions))
			{
				events.arraySize++;
			}

			UnityGUI.EndHorizontal();
		}
	}
}
