using System.Reflection;
using UnityEngine;

namespace LNE.ObjectExts
{
	/// <summary>
	/// Collection of useful methods for all objects
	/// </summary>
	public static class ObjectExtensions
	{
		public static void CopyFrom(this object to, object from)
		{
			Debug.Assert(to != null && from != null);
			Debug.Assert(from.GetType() == to.GetType());

			var type = from.GetType();

			foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				field.SetValue(to, field.GetValue(from));
			}
		}

		public static void CopyTo(this object from, object to)
		{
			to.CopyFrom(from);
		}
	}
}
