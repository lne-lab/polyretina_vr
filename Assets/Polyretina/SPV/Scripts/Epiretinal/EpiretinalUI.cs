using UnityEngine;

namespace LNE.ProstheticVision.UI
{
	using Threading;

	public class EpiretinalUI : MonoBehaviour
	{
		public bool AlwaysRender { get; set; }

		private EdgeDetector edgeDetector => Prosthesis.Instance.ExternalProcessor as EdgeDetector;
		private EpiretinalImplant implant => Prosthesis.Instance.Implant as EpiretinalImplant;

		void Update()
		{
			// render when told to or whenever the mouse does something
			if (AlwaysRender ||
				Input.GetMouseButton(0) ||
				Input.GetMouseButton(1) ||
				Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				UpdatePolyretinaNow();
			}
		}

		public void SetEdgeDetectorOn(bool on)
		{
			edgeDetector.on = on;
			UpdatePolyretina();
		}

		public void SetSensitivity(int value)
		{
			edgeDetector.sensitivity = (Strength)value;
			UpdatePolyretina();
		}

		public void SetContrast(float value)
		{
			edgeDetector.contrast = value;
			UpdatePolyretina();
		}

		public void SetBrightness(float value)
		{
			edgeDetector.brightness = value;
			UpdatePolyretina();
		}

		public void SetSaturation(float value)
		{
			edgeDetector.saturation = value;
			UpdatePolyretina();
		}

		public void SetThickness(float value)
		{
			edgeDetector.thickness = (int)value;
			UpdatePolyretina();
		}

		public void SetThreshold(float value)
		{
			edgeDetector.threshold = value;
			UpdatePolyretina();
		}

		public void SetImplantOn(bool on)
		{
			implant.on = on;
			UpdatePolyretina();
		}

		public void SetPattern(int value)
		{
			implant.pattern = (ElectrodePattern)value;
			UpdatePolyretina();
		}

		public void SetLayout(int value)
		{
			implant.layout = (ElectrodeLayout)value;
			UpdatePolyretina();
		}

		public void SetFieldOfView(float value)
		{
			implant.fieldOfView = value;
			UpdatePolyretina();
		}

		public void SetLuminanceLevels(float value)
		{
			implant.luminanceLevels = (int)value;
			UpdatePolyretina();
		}

		public void SetImplantBrightness(float value)
		{
			implant.brightness = value;
			UpdatePolyretina();
		}

		public void SetBrokenChance(float value)
		{
			implant.brokenChance = value;
			UpdatePolyretina();
		}

		public void SetSizeVariance(float value)
		{
			implant.sizeVariance = value;
			UpdatePolyretina();
		}

		public void SetIntensityVariance(float value)
		{
			implant.intensityVariance = value;
			UpdatePolyretina();
		}

		public void SetTailLength(float value)
		{
			implant.tailLength = value;
			UpdatePolyretina();
		}

		public void SetOutline(bool value)
		{
			implant.outlineDevice = value;
			UpdatePolyretina();
		}

		public void UpdatePolyretina()
		{
			CallbackManager.InvokeOnce(.001f, () =>
			{
				UpdatePolyretinaNow();
			});
		}

		public void UpdatePolyretinaNow()
		{
			Prosthesis.Instance.Camera.Render();
		}
	}
}
