#pragma warning disable 649

using System.Collections;
using UnityEngine;
using LNE.ProstheticVision;
using LNE.ProstheticVision.Fading;

public class FadeDebugger : MonoBehaviour
{
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
		readback = new Texture2D(1, 1);
		graph = FindObjectOfType<LineGraph>();

		StartCoroutine(GetPixel());
	}

	void Update()
	{
		if (Input.GetMouseButton(1))
		{
			Pixel = Input.mousePosition;
		}
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

			graph.Value = readback.GetPixel(0, 0).r;

			RenderTexture.active = activeRT;
		}
	}
}
