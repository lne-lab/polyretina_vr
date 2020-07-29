using System;
using UnityEngine;
using UnityEditor;

namespace LNE.UI
{
	using LNE.ArrayExts;
	using LNE.ImageExts;

	public class ScreenshotShortcut
	{
		[MenuItem("Polyretina/Screenshot &p", priority = WindowPriority.screenshot)]
		static void Screenshot()
		{
			if (Application.isPlaying == false)
				return;

			var cameras = GameObject.FindObjectsOfType<Camera>().Where((cam) =>
				cam.targetTexture == null
			);

			if (cameras.Length != 1)
			{
				Debug.LogError($"Screenshot failed! Number of cameras found: {cameras.Length}.");
				return;
			}

			cameras[0].Screenshot(UnityApp.DataPath + "../" + DateTime.Now.ToString("dd-MM-yyy_hh-mm-ss") + ".png");
		}
	}
}
