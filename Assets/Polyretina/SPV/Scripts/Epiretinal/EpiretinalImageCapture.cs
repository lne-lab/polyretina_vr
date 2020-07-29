#pragma warning disable 649

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LNE.ProstheticVision
{
	using ImageExts;
	using LNE.UI.Attributes;

	/// <summary>
	/// Screenshot output of Retinal Prosthesis under different conditions
	/// </summary>
	public class EpiretinalImageCapture : Singleton<EpiretinalImageCapture>
	{
		/*
		 * Editor fields
		 */

		[SerializeField, Path]
		private string _path;

		[Space]

		[SerializeField]
		private HeadsetModel[] _headsets;

		[SerializeField, Range(0, 1)]
		private float[] _brightness;

		[SerializeField]
		private ElectrodePattern[] _patterns;

		[SerializeField]
		private ElectrodeLayout[] _layouts;

		[SerializeField]
		private float[] _fieldOfViews;

		[SerializeField, Range(0, 3)]
		private float[] _decayConstants;

		[Header("Optional")]

		[SerializeField]
		private int[] _fontSizes;

		[SerializeField]
		private Text _textBox;

		/*
		 * Private properties
		 */

		private EpiretinalImplant implant => Prosthesis.Instance.Implant as EpiretinalImplant;

		/*
		 * Unity callbacks
		 */

		void Start()
		{
			if (_headsets.Length == 0)
				_headsets = new[] { implant.headset };

			if (_brightness.Length == 0)
				_brightness = new[] { implant.brightness };

			if (_patterns.Length == 0)
				_patterns = new[] { implant.pattern };

			if (_layouts.Length == 0)
				_layouts = new[] { implant.layout };

			if (_fieldOfViews.Length == 0)
				_fieldOfViews = new[] { implant.fieldOfView };

			if (_decayConstants.Length == 0)
				_decayConstants = new[] { implant.tailLength };

			if (_fontSizes.Length == 0)
				_fontSizes = new[] { _textBox != null ? _textBox.fontSize : 0 };
		}

		/*
		 * Public methods
		 */

		public void TakeScreenshots()
		{
			StartCoroutine(TakeScreenshots_Coroutine());
		}

		public void TakeScreenshot(string screenshotName)
		{
			Prosthesis.Instance.Camera.Screenshot(_path + screenshotName + ".png");
		}

		public string GenerateName()
		{
			var name = "";

			name += GenerateFactorName(_headsets, implant.headset);
			name += GenerateFactorName(_brightness, implant.brightness);
			name += GenerateFactorName(_patterns, implant.pattern);
			name += GenerateFactorName(_layouts, implant.layout);
			name += GenerateFactorName(_fieldOfViews, implant.fieldOfView);
			name += GenerateFactorName(_decayConstants, implant.tailLength);
			name += GenerateFactorName(_fontSizes, _textBox != null ? _textBox.fontSize : 0);

			return name;
		}

		public string GenerateFactorName<T>(T[] arr, T val)
		{
			return arr.Length > 1 ? val.ToString() + "_" : "";
		}

		/*
		 * Coroutines
		 */

		IEnumerator TakeScreenshots_Coroutine()
		{
			foreach (var headset in _headsets)
			{
				foreach (var brightness in _brightness)
				{
					foreach (var pattern in _patterns)
					{
						foreach (var layout in _layouts)
						{
							foreach (var fieldOfView in _fieldOfViews)
							{
								foreach (var decayConstant in _decayConstants)
								{
									foreach (var fontSize in _fontSizes)
									{
										implant.headset = headset;
										implant.brightness = brightness;
										implant.pattern = pattern;
										implant.layout = layout;
										implant.fieldOfView = fieldOfView;
										implant.tailLength = decayConstant;

										if (_textBox != null)
											_textBox.fontSize = fontSize;

										yield return null;

										TakeScreenshot(GenerateName());

										yield return null;
									}
								}
							}
						}
					}
				}
			}

			Prosthesis.Instance.enabled = false;

			yield return null;

			TakeScreenshot("Natural");

			yield return null;

			Prosthesis.Instance.enabled = true;
		}
	}
}
