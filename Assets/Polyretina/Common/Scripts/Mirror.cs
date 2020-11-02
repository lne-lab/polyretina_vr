using System.Reflection;
using UnityEngine;

namespace LNE.Reflection
{
	using ArrayExts;
	using ReflectionExts;

	/// <summary>
	/// Reflection helper methods
	/// </summary>
	public static class Mirror
	{
		public static object InvokeMethod(object obj, string methodName, params FieldInfo[] arguments)
		{
			var type = obj.GetType();
			var method = type.GetMethod(methodName,	BindingFlags.Instance |
													BindingFlags.Public |
													BindingFlags.NonPublic);

			if (method != null)
			{
				var parameters = method.GetParameters();
				if (parameters.Length == 0)
				{
					return method.Invoke(obj, null);
				}
				else if (ArgumentsMatch(parameters, arguments))
				{
					return method.Invoke(obj, arguments.Convert((fi) => fi.GetValue(obj)));
				}
				else
				{
					Debug.LogWarning($"{obj}.{methodName} with appropriate parameters not found.");
				}
			}

			return null;
		}

		public static void SetProperty(object obj, string propertyName, FieldInfo argument)
		{
			var type = obj.GetType();
			var property = type.GetProperty(propertyName, BindingFlags.Instance |
															BindingFlags.Public |
															BindingFlags.NonPublic);

			var compatible = property?.SetMethod != null && property.PropertyType == argument.FieldType;
			if (compatible)
			{
				property.SetMethod.Invoke(obj, new[] { argument.GetValue(obj) });
			}
			else
			{
				Debug.LogWarning($"{obj}.{propertyName} property of type {argument.FieldType} not found.");
			}
		}

		public static T GetValue<T>(object obj, string fieldName)
		{
			return obj.GetType()
						.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						.GetValue<T>(obj);
		}

		private static bool ArgumentsMatch(ParameterInfo[] parameters, FieldInfo[] arguments)
		{
			return parameters.Convert((e) => e.ParameterType).EqualsArray(arguments.Convert((e) => e.FieldType));
		}
	}
}
