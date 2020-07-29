using System;
using System.IO;
using UnityEngine;

namespace LNE.ImageExts
{
	using static ArrayExts.ArrayExtensions;

	/// <summary>
	/// Collection of useful methods related to images
	/// </summary>
	public static class ImageExtensions
	{
		public static void Apply(this Texture2D that, Func<Color, Color> func)
		{
			that.SetPixels(that.GetPixels().Apply(func));
			that.Apply();
		}

		public static void Apply_Parallelised(this Texture2D that, Func<Color, Color> func)
		{
			that.SetPixels(that.GetPixels().Apply_Parallelised(func));
			that.Apply();
		}

		public static void SaveAsPNG(this Texture2D that, string path)
		{
			that.Apply();
			File.WriteAllBytes(path, that.EncodeToPNG());
		}

		public static Texture2D ToTexture2D(this RenderTexture that, TextureFormat format = TextureFormat.ARGB32, bool linear = false)
		{
			var output = new Texture2D(that.width, that.height, format, false, linear);

			var active = RenderTexture.active;
			RenderTexture.active = that;

			output.ReadPixels(new Rect(0, 0, that.width, that.height), 0, 0);
			output.Apply();

			RenderTexture.active = active;

			return output;
		}

		public static Texture2D Screenshot(this Camera that)
		{
			var prevTarget = that.targetTexture;
			var currTarget = new RenderTexture(that.pixelWidth, that.pixelHeight, 24);
			currTarget.Create();

			that.targetTexture = currTarget;
			that.Render();

			var screenshot = currTarget.ToTexture2D();

			that.targetTexture = prevTarget;
			RenderTexture.Destroy(currTarget);

			return screenshot;
		}

		public static void Screenshot(this Camera that, string path)
		{
			that.Screenshot().SaveAsPNG(path);
		}

		public static Texture2D Blit(this Texture2D that, Material material, int width, int height)
		{
			var tempRT = RenderTexture.GetTemporary(width, height);
			Graphics.Blit(that, tempRT, material);
			var result = tempRT.ToTexture2D();
			RenderTexture.ReleaseTemporary(tempRT);
			return result;
		}

		public static Texture2D Blit(this Texture2D that, Material material)
		{
			return that.Blit(material, that.width, that.height);
		}

		public static Texture2D Blit_Editor(this Texture2D that, Material material)
		{
			that.SaveAsPNG(UnityApp.ProjectPath + $"test_0.png");

			for (int i = 0; i < material.passCount; i++)
			{
				for (int j = 0; j < material.passCount; j++)
				{
					var passName = material.GetPassName(j);
					material.SetShaderPassEnabled(passName, i == j);
				}

				that = that.Blit(material);
				that.SaveAsPNG(UnityApp.ProjectPath + $"test_{material.GetPassName(i)}.png");
			}

			return that;
		}

		public static Texture2D Scale(this Texture2D that, int width, int height, ScaleMode mode)
		{
			var material = new Material(Shader.Find("LNE/Scale"));	// this wont work in play mode
			material.SetVector("_src_res", new Vector2(that.width, that.height));
			material.SetVector("_dst_res", new Vector2(width, height));
			material.SetInt("_mode", (int)mode);

			return that.Blit(material, width, height);
		}
	}
}
