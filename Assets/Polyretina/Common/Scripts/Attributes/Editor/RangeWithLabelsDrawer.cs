using UnityEngine;
using UnityEditor;

namespace LNE.UI.Attributes
{
	[CustomPropertyDrawer(typeof(RangeWithLabelsAttribute))]
	public class RangeWithLabelsDrawer : ExtendedAttributeDrawer<RangeWithLabelsAttribute>
	{
		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			BeginGUI(rect, property, label);

			DrawNames(position, attribute.Label1, attribute.Label2, attribute.Label3);

			position.y += 6;
			position.height = 16;
			Slider(attribute.Min, attribute.Max);

			EndGUI();
		}

		public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
		{
			return 6 + 16;
		}

		private void DrawNames(Rect rect, string label1, string label2, string label3)
		{
			rect.height = 12;
			var style = new GUIStyle { fontSize = 8 };

			// label 1
			rect.x = ((rect.width - 5) / 2.225f) - 5;
			EditorGUI.LabelField(rect, label1, style);

			// label 2
			var label2Width = style.CalcSize(new GUIContent(label2)).x;
			rect.x = ((rect.width - 18) / 1.375f) - 18;
			rect.x -= label2Width / 2;
			EditorGUI.LabelField(rect, label2, style);

			// label 3
			var label3Width = style.CalcSize(new GUIContent(label3)).x;
			rect.x = rect.width - (45 + label3Width);
			EditorGUI.LabelField(rect, label3, style);
		}
	}
}
