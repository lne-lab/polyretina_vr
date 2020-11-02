using UnityEngine;
using UnityEngine.UI;

namespace LNE
{
	/// <summary>
	/// Displays a chosen webcam on the target image or renderer
	/// </summary>
	public class Webcam : MonoBehaviour
	{
		public enum Target { None, Image, Renderer }

		public const string NO_WEBCAM_FOUND = "No webcam found";

#pragma warning disable 649
		[SerializeField]
		private Target target;

		[SerializeField]
		private string deviceName;
#pragma warning restore 649

		public WebCamTexture Texture { get; private set; }

		void Awake()
		{
			if (Texture == null)
			{
				Texture = (deviceName == NO_WEBCAM_FOUND) ? new WebCamTexture() : new WebCamTexture(deviceName);
				Texture.Play();

				Debug.Log($"Webcam Info: {Texture.deviceName} ({Texture.width}, {Texture.height}).");
			}
		}

		void Start()
		{
			switch (target)
			{
				case Target.Renderer:
					GetComponent<Renderer>().material.mainTexture = Texture;
					break;
				case Target.Image:
					GetComponent<Image>().material.mainTexture = Texture;
					break;
			}
		}

		void OnApplicationQuit()
		{
			if (Texture != null)
			{
				Texture.Stop();
				Destroy(Texture);
				Texture = null;
			}
		}
	}
}
