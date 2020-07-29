using UnityEngine;

namespace LNE.TransformExts
{
	/// <summary>
	/// Collection of useful methods for transforms
	/// </summary>
	public static class TransformExtensions
	{
		public static void SetX(this Transform that, float x, Space space = Space.World)
		{
			switch (space)
			{
				case Space.Self:
					that.localPosition = new Vector3(x, that.localPosition.y, that.localPosition.z);
					break;
				case Space.World:
					that.position = new Vector3(x, that.position.y, that.position.z);
					break;
			}
		}

		public static void SetY(this Transform that, float y, Space space = Space.World)
		{
			switch (space)
			{
				case Space.Self:
					that.localPosition = new Vector3(that.localPosition.x, y, that.localPosition.z);
					break;
				case Space.World:
					that.position = new Vector3(that.position.x, y, that.position.z);
					break;
			}
		}

		public static void SetZ(this Transform that, float z, Space space = Space.World)
		{
			switch (space)
			{
				case Space.Self:
					that.localPosition = new Vector3(that.localPosition.x, that.localPosition.y, z);
					break;
				case Space.World:
					that.position = new Vector3(that.position.x, that.position.y, z);
					break;
			}
		}

		public static void SetScale(this Transform that, float value)
		{
			that.localScale = new Vector3(value, value, value);
		}
	}
}
