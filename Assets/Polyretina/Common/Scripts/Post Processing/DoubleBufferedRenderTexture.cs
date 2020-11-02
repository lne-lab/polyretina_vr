using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace LNE.PostProcessing
{
	/// <summary>
	/// Double-buffered render texture
	/// </summary>
	public class DoubleBufferedRenderTexture
	{
		/*
		 * Private fields
		 */

		private RenderTexture first;
		private RenderTexture second;

		private bool swapped;

		/*
		 * Public properties
		 */

		public RenderTexture Front => swapped ? second : first;

		public RenderTexture Back => swapped ? first : second;

		/*
		 * Constructor / Destructor
		 */

		public DoubleBufferedRenderTexture(int width, int height)
		{
			first = new RenderTexture(width, height, 0, GraphicsFormat.R32G32B32A32_SFloat, 0);
			first.antiAliasing = 1;
			first.filterMode = FilterMode.Point;
			first.anisoLevel = 0;
			first.enableRandomWrite = true;
			first.Create();

			second = new RenderTexture(width, height, 0, GraphicsFormat.R32G32B32A32_SFloat, 0);
			second.antiAliasing = 1;
			second.filterMode = FilterMode.Point;
			second.anisoLevel = 0;
			second.enableRandomWrite = true;
			second.Create();
		}

		public DoubleBufferedRenderTexture(RenderTexture first, RenderTexture second)
		{
			this.first = first;
			this.second = second;
		}

		~DoubleBufferedRenderTexture()
		{
			first.Release();
			Object.Destroy(first);
			first = null;

			first.Release();
			Object.Destroy(second);
			second = null;
		}

		/*
		 * Public methods
		 */

		public void Initialise(Material material)
		{
			Graphics.Blit(null, first, material);
			Graphics.Blit(null, second, material);
		}

		public void Initialise(Shader shader)
		{
			Initialise(new Material(shader));
		}

		public void Initialise(Color colour)
		{
			var material = new Material(Shader.Find("Unlit/Color"));
			material.color = colour;

			Initialise(material);
		}

		public void Initialise(Texture texture)
		{
			var material = new Material(Shader.Find("Unlit/Texture"));
			material.mainTexture = texture;

			Initialise(material);
		}

		public void Swap()
		{
			swapped = !swapped;
		}
	}
}
