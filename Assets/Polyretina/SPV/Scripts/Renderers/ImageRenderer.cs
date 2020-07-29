using UnityEngine;

namespace LNE.ProstheticVision
{
	public abstract class ImageRenderer : ScriptableObject
	{
		/*
		 * Static
		 */

		public static T Initialise<T>(T value) where T : ImageRenderer
		{
			if (value == null)
			{
				return null;
			}

			var retval = Instantiate(value);
			retval.original = value.original != null ? value.original : value;
			retval.name = value.name;
			retval.Start();

			return retval;
		}

		/*
		 * Instance
		 */

		[HideInInspector]
		public bool on = true;

		[HideInInspector]
		public bool foldout = true;

		[HideInInspector]
		public ImageRenderer original = null;

		public abstract void Start();

		public abstract void Update();

		public abstract void GetDimensions(out int width, out int height);

		public abstract void OnRenderImage(Texture source, RenderTexture destination);
	}
}
