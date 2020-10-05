#pragma warning disable 649

using System;
using System.Collections;
using UnityEngine;
using LNE.ProstheticVision;
using LNE.ProstheticVision.Fading;
using LNE.UI.Attributes;
using LNE.UI;

public class FadeDebugger : MonoBehaviour
{
	public enum Channel { R = 1, G = 2, B = 4, A = 8 }

	[SerializeField, Space, CustomLabel(label = "Reset")]
	private EditorButton resetButton;

	[Header("Fade Values")]
	[SerializeField, CustomLabel(label = "Material")]
	private Material fadeMaterial;

	[SerializeField]
	Channel channels;

	[Header("Crosshair")]
	[SerializeField, CustomLabel(label = "Material")]
	private Material crosshairMaterial;

	[SerializeField]
	private Vector2 pixel;

	[SerializeField]
	private Vector2 pixelOffset;

	private Texture2D readback;
	private LineGraph graph;

	public Vector2 Pixel
	{
		get => pixel + pixelOffset;
		set => pixel = value;
	}

	private EpiretinalImplant Implant => Prosthesis.Instance.Implant as EpiretinalImplant;

	void Start()
	{
		resetButton = new EditorButton(() => { Implant.ResetFadingParameters(); });

		readback = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
		graph = FindObjectOfType<LineGraph>();

		StartCoroutine(GetPixel());
	}

	void Update()
	{
		fadeMaterial.SetTexture("_SubTex", Implant.FadeRT.Back);
		fadeMaterial.SetFloat("_R", channels.HasFlag(Channel.R) ? 1 : 0);
		fadeMaterial.SetFloat("_G", channels.HasFlag(Channel.G) ? 1 : 0);
		fadeMaterial.SetFloat("_B", channels.HasFlag(Channel.B) ? 1 : 0);
		fadeMaterial.SetFloat("_A", channels.HasFlag(Channel.A) ? 1 : 0);

		if (Input.GetMouseButton(1))
		{
			Pixel = Input.mousePosition;
		}

		crosshairMaterial.SetVector("_target_pixel", Pixel);
	}

	IEnumerator GetPixel()
	{
		while (Application.isPlaying)
		{
			yield return new WaitForEndOfFrame();

			var activeRT = RenderTexture.active;
			RenderTexture.active = Implant.FadeRT.Back;

			readback.ReadPixels(new Rect(Pixel.x, Implant.headset.GetHeight() - Pixel.y, 1, 1), 0, 0);
			readback.Apply();

			if (channels.HasFlag(Channel.R))
			{
				graph.Value = readback.GetPixel(0, 0).r;
			}
			else if (channels.HasFlag(Channel.G))
			{
				graph.Value = readback.GetPixel(0, 0).g;
			}
			else if (channels.HasFlag(Channel.B))
			{
				graph.Value = readback.GetPixel(0, 0).b;
			}
			else if (channels.HasFlag(Channel.A))
			{
				graph.Value = readback.GetPixel(0, 0).a;
			}

			RenderTexture.active = activeRT;
		}
	}
}
