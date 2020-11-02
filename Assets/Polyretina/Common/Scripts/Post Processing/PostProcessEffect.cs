#pragma warning disable 649

using UnityEngine;

namespace LNE.PostProcessing
{
	/// <summary>
	/// Applyies a post process effect to a camera
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class PostProcessEffect : MonoBehaviour
	{
		public enum Source { Shader, Material }

		/*
		 * Editor fields
		 */

		[SerializeField]
		public Source _source;

		[SerializeField]
		private Shader _shader;

		[SerializeField]
		private Material _material;

		/*
		 * Public properties
		 */

		public Shader Shader
		{
			get => _shader;
			set
			{
				_shader = value;
				if (_shader != null)
				{
					_material = new Material(_shader);
				}

				_source = Source.Shader;
			}
		}

		public Material Material
		{
			get => _material;
			set
			{
				_material = value;
				_shader = _material?.shader;

				_source = Source.Material;
			}
		}

		/*
		 * Unity callbacks
		 */

		void Awake()
		{
			if (_shader != null && _material == null)
			{
				_material = new Material(_shader);
			}

			if (_material != null && _shader == null)
			{
				_shader = _material.shader;
			}
		}

		void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, Material);
		}

		/*
		 * Public methods
		 */

		public void SetFloat(int propertyId, float value)
		{
			Material.SetFloat(propertyId, value);
		}

		public void SetInt(int propertyId, int value)
		{
			Material.SetInt(propertyId, value);
		}

		public void SetTexture(int propertyId, Texture value)
		{
			Material.SetTexture(propertyId, value);
		}

		public void SetVector(int propertyId, Vector4 value)
		{
			Material.SetVector(propertyId, value);
		}
	}
}
