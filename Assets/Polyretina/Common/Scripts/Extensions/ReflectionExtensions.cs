using System;
using System.Reflection;

namespace LNE.ReflectionExts
{
	/// <summary>
	/// Collection of useful reflection methods
	/// </summary>
	public static class ReflectionExtensions
	{
		public static T GetValue<T>(this FieldInfo field, object obj)
		{
			return (T)field.GetValue(obj);
		}

		public static bool HasCustomAttribute<T>(this MemberInfo member) where T : Attribute
		{
			return member.GetCustomAttribute<T>() != null;
		}
	}
}
