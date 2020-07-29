#pragma warning disable 649

using UnityEngine;

public class Crosshair : MonoBehaviour
{
	[SerializeField]
	private Shader shader;

	private Material material;
	private FadeDebugger debugger;

	void Start()
	{
		material = new Material(shader);
		debugger = FindObjectOfType<FadeDebugger>();
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetVector("_target_pixel", debugger.Pixel);
		Graphics.Blit(source, destination, material);
	}
}
