using UnityEditor;

namespace LNE.SerializedPropertyExts
{
	public static class SerializedPropertyExtensions
	{
		public static object GetValue(this SerializedProperty that)
		{
			switch (that.propertyType)
			{
				case SerializedPropertyType.AnimationCurve:
					return that.animationCurveValue;
				case SerializedPropertyType.Boolean:
					return that.boolValue;
				case SerializedPropertyType.Bounds:
					return that.boundsValue;
				case SerializedPropertyType.BoundsInt:
					return that.boundsIntValue;
				case SerializedPropertyType.Character:
					return that.stringValue;
				case SerializedPropertyType.Color:
					return that.colorValue;
				case SerializedPropertyType.Enum:
					return that.intValue;
				case SerializedPropertyType.Float:
					return that.floatValue;
				case SerializedPropertyType.Integer:
					return that.intValue;
				case SerializedPropertyType.ObjectReference:
					return that.objectReferenceValue;
				case SerializedPropertyType.Quaternion:
					return that.quaternionValue;
				case SerializedPropertyType.Rect:
					return that.rectValue;
				case SerializedPropertyType.RectInt:
					return that.rectIntValue;
				case SerializedPropertyType.String:
					return that.stringValue;
				case SerializedPropertyType.Vector2:
					return that.vector2Value;
				case SerializedPropertyType.Vector3:
					return that.vector3Value;
				case SerializedPropertyType.Vector4:
					return that.vector4Value;
				case SerializedPropertyType.Vector2Int:
					return that.vector2IntValue;
				case SerializedPropertyType.Vector3Int:
					return that.vector3IntValue;
				default:
					throw new System.Exception($"Unsupported type: {that.propertyType}");
			}
		}

		public static T GetValue<T>(this SerializedProperty that)
		{
			return (T)that.GetValue();
		}
	}
}
