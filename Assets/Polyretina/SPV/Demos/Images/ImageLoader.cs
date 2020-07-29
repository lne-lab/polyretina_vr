using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace LNE.Demos.Images
{
	using ArrayExts;
	using ImageExts;
	using ProstheticVision;
	using Threading;
	using UI;

	public class ImageLoader : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

#pragma warning disable 649

		[SerializeField]
		private Image _screen;

		[SerializeField]
		private Dropdown _imageDropdown;

#pragma warning restore 649

		/*
		 * Unity callbacks
		 */

		void Start()
		{
			_imageDropdown.ClearOptions();
			_imageDropdown.AddOptions(new List<string>(GetImageFilenames()));
			_imageDropdown.onValueChanged.AddListener((i) =>
			{
				LoadImage(i);
			});

			LoadImage(0);
		}

		/*
		 * Public methods
		 */

		public void SaveImage()
		{
			Prosthesis.Instance.Camera.Screenshot(UnityApp.ProjectPath + "Images_SPV/" + GetImageFilenames()[_imageDropdown.value] + "_spv.png");
		}

		/*
		 * Private methods
		 */

		private void LoadImage(int i)
		{
			LoadImage(GetImagePath(i));
		}

		private void LoadImage(string path)
		{
			// set image
			var image = new Texture2D(1000, 1000);
			image.LoadImage(File.ReadAllBytes(path));
			_screen.material.SetTexture("_SubTex", image);

			// set image size
			var imageSize = new Vector2(image.width, image.height);
			imageSize *= 1000 / Mathf.Max(imageSize.x, imageSize.y);
			_screen.GetComponent<RectTransform>().sizeDelta = imageSize;
			_screen.GetComponent<ZoomUI>().StartingSize = imageSize;

			// render
			CallbackManager.InvokeOnce(.001f, () =>
			{
				Prosthesis.Instance.Camera.Render();
			});
		}

		private string[] GetImageFilenames()
		{
			return GetImagePaths().Apply((path) => Path.GetFileNameWithoutExtension(path));
		}

		private string GetImagePath(int i)
		{
			return GetImagePaths()[i];
		}

		private string[] GetImagePaths()
		{
			var path = UnityApp.ProjectPath + "Images/";
			return Directory.GetFiles(path);
		}
	}
}
