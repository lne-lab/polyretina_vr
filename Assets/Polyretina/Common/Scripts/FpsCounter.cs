#pragma warning disable 649

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LNE.Analysis
{
	using UI.Attributes;
	using static ArrayExts.ArrayExtensions;

	/// <summary>
	/// Displays an FPS counter on the screen saves the data to a file
	/// </summary>
	public class FpsCounter : MonoBehaviour
	{
		/*
		 * Inspector fields
		 */

		[Space]

		[SerializeField, CustomLabel(label = "Refresh Rate (Hz)")]
		private float _refreshRate = 1;

		[SerializeField, Path]
		private string _path;

		[SerializeField]
		private string _filename;

		[Header("Display")]

		[SerializeField]
		private Vector2 _textPosition = new Vector2(.7f, .95f);

		[SerializeField]
		private int _textSize = 100;

		[SerializeField]
		private Color _textColour = Color.yellow;

		/*
		 * Private fields
		 */

		private float refreshTime;
		private int frameCount;

		private List<int> frameCounts = new List<int>();

		private string text;

		/*
		 * Unity callbacks
		 */

		void Start()
		{
			refreshTime = Time.time;
		}

		void Update()
		{
			frameCount++;

			if (Time.time - refreshTime >= (1 / _refreshRate))
			{
				text = $"{frameCount}Hz ({(int)(_refreshRate / frameCount * 1000)}ms)";

				frameCounts.Add(frameCount);

				frameCount = 0;
				refreshTime = Time.time;
			}
		}

		void OnGUI()
		{
			var position = new Rect(_textPosition * new Vector2(Screen.width, Screen.height), new Vector2(1000, _textSize));
			var style = new GUIStyle(GUI.skin.label) { fontSize = _textSize };

			GUI.color = _textColour;
			GUI.Label(position, text, style);
		}

		void OnApplicationQuit()
		{
			if (Directory.Exists(_path))
			{
				var csv = ToCSV(true, ";", frameCounts.ToArray());
				File.WriteAllText(_path + _filename + ".csv", csv);
			}
		}
	}
}
