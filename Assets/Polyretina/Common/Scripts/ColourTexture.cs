using System.Collections.Generic;
using UnityEngine;

namespace LNE.UI
{
	using static ArrayExts.ArrayExtensions;

	/// <summary>
	/// Creates uniformly coloured textures. Caches for efficiency
	/// </summary>
	public static class ColourTexture
	{
		private static Dictionary<(byte, byte, byte, byte, int, int), Texture2D> cache
			= new Dictionary<(byte, byte, byte, byte, int, int), Texture2D>();

		public static Texture2D Get(byte r, byte g, byte b, byte a, int width = 64, int height = 64)
		{
			return Get(new Color32(r, g, b, a), width, height);
		}

		public static Texture2D Get(Color32 colour, int width = 64, int height = 64)
		{
			var key = (colour.r, colour.g, colour.b, colour.a, width, height);

			var found = cache.TryGetValue(key, out var retval);
			if (found == false || retval == null)
			{
				retval = new Texture2D(width, height);
				retval.SetPixels32(CreateArray(width * height, () => colour));
				retval.Apply();

				cache[key] = retval;
			}

			return retval;
		}
	}
}
