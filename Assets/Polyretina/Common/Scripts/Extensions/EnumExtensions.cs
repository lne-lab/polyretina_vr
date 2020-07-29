using System;

namespace LNE.EnumExts
{
	using ArrayExts;

	/// <summary>
	/// Collection of useful methods for enums
	/// </summary>
	public static class EnumExtensions
	{
		public static int ToInt(this Enum that)
		{
			return (int)(object)that;
		}

		public static int GetIndex(this Enum that)
		{
			return Enum.GetNames(that.GetType()).IndexOf(that.ToString());
		}

		public static int Count<T>() where T : struct, IConvertible
		{
			if (typeof(T).IsEnum == false)
			{
				throw new Exception("T is not an enum.");
			}

			return Enum.GetValues(typeof(T)).Length;
		}

		public static T[] Enumerate<T>()
		{
			return Enum.GetValues(typeof(T)) as T[];
		}
	}
}
