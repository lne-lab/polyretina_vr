#pragma warning disable 649
#pragma warning disable 414

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LNE.ProstheticVision.Fading
{
	using LNE.UI.Attributes;

	public class GlobalFading : MonoBehaviour
	{
		/*
		 * Editor fields
		 */

		[SerializeField]
		private float	fastDecayTime = 0.176f,
						fastDecayRate = 0.0f,
						slowDecayTime = 14.0f,
						slowDecayRate = 0.0f,
						decayThreshold = 0.95f,
						absoluteRefractoryTime = 0.0f, 
						relativeRefractoryTime = 30.0f;

		[Space]

		[SerializeField, RangeWithLabels(0, 1, "Every activation", "", "At 100% recovery"), CustomLabel(label = "Restart Fast Decay")]
		private int _restartFastDecay;

		[SerializeField, RangeWithLabels(0, 1, "Linear", "", "Exponential"), CustomLabel(label = "Decay Rate")]
		private int _decayRate1;

		[SerializeField, RangeWithLabels(0, 1, "Relative to brightness", "", "Uniform"), CustomLabel(label = "Decay Rate")]
		private int _decayRate2;

		[SerializeField]
		private float _decayExponent;

		[Space]

		[SerializeField, RangeWithLabels(0, 1, "Relative to brightness", "", "Uniform"), CustomLabel(label = "Recovery Rate")]
		private int _recoveryRate;

		[SerializeField]
		private float _recoveryExponent;

		[Space]

		[SerializeField, LinkWithProperty]
		private bool _electrodesAreOn = false;

		[Header("Links")]

		[SerializeField]
		private Text onTextbox;

		[SerializeField]
		private Text pulseTextbox;

		[SerializeField]
		private Text stateTextbox;

		[SerializeField]
		private Text onTimeTextbox;

		[SerializeField]
		private Text offTimeTextbox;

		[SerializeField]
		private Text brightnessTextbox;

		/*
		 * Private fields
		 */

		private float onTime = 0;
		private float offTime = 0;
		private float startingBrightness = 1;
		private LineGraph graph;

		private int onFrames = 0;

		/*
		 * Private properties
		 */

		private EpiretinalImplant Implant => Prosthesis.Instance.Implant as EpiretinalImplant;

		private float ActualFastDecayTime => fastDecayTime * ((float)Implant.onFrames / (Implant.onFrames + Implant.offFrames));

		private bool FastDecayEveryActivation => _restartFastDecay == 0;
		private bool FastDecayAtCompleteRecovery => _restartFastDecay == 1;

		private bool RelativeRecoveryRate => _recoveryRate == 0;
		private bool UniformRecoveryRate => _recoveryRate == 1;

		private bool LinearDecayRate => _decayRate1 == 0;
		private bool ExponentialDecayRate => _decayRate1 == 1;

		private bool RelativeDecayRate => _decayRate2 == 0;
		private bool UniformDecayRate => _decayRate2 == 1;


		private bool ElectrodesAreOn
		{
			get
			{
				return _electrodesAreOn;
			}

			set
			{
				_electrodesAreOn = value;

				Implant.brightness = value ? Brightness : 0;
			}
		}

		private float _brightness = 1;
		private float Brightness
		{
			get
			{
				return _brightness;
			}

			set
			{
				_brightness = value;

				Implant.brightness = ElectrodesAreOn ? value : 0;
				graph.Value = value;

				brightnessTextbox.text = value.ToString("N2");
			}
		}

		/*
		 * Unity callbacks
		 */

		void Start()
		{
			Application.targetFrameRate = 90;

			graph = FindObjectOfType<LineGraph>();

			Debug.Log("Actual Fast Decay Time:" + ActualFastDecayTime.ToString());
		}

		void Update()
		{
			UpdateFade();

			onTextbox.text = ElectrodesAreOn.ToString();
			pulseTextbox.text = Implant.Pulse.ToString();
			onTimeTextbox.text = onTime.ToString("N3");
			offTimeTextbox.text = offTime.ToString("N3");
		}

		private void UpdateFade()
		{
			if (FastDecayEveryActivation && ElectrodesAreOn == false)
			{
				ResetOnTime();
			}

			if (ElectrodesAreOn && Implant.Pulse)
			{
				IncreaseOnTime();
				ResetOffTime();

				if (DecayingQuickly())
				{
					Decay(fastDecayRate);
					stateTextbox.text = "Fast decay";
				}
				else
				{
					if (FastDecayAtCompleteRecovery && RecoveryIsAboveThreshold())
					{
						ResetOnTime();
						IncreaseOnTime();
						Decay(fastDecayRate);
						stateTextbox.text = "Fast decay";
					}
					else
					{
						Decay(slowDecayRate);
						stateTextbox.text = "Slow decay";
					}
				}

				UpdateStartingBrightness();
			}
			else
			{
				IncreaseOffTime();
				Recover();
				stateTextbox.text = "Recovery";
			}

			Brightness = Mathf.Clamp(Brightness, 0.001f, 1);
		}

		private bool DecayingQuickly()
		{
			return onFrames <= 3;
			//return onTime < ActualFastDecayTime;
		}

		private bool RecoveryIsAboveThreshold()
		{
			return Brightness >= decayThreshold;
		}

		private void IncreaseOnTime()
		{
			onTime += Time.deltaTime;
			onFrames++;
		}

		private void IncreaseOffTime()
		{
			offTime += Time.deltaTime;
		}

		private void ResetOnTime()
		{
			onTime = 0;
			onFrames = 0;
		}

		private void ResetOffTime()
		{
			offTime = 0;
		}

		private void Decay(float rate)
		{
			if (LinearDecayRate)
			{
				LinearDecay(rate);
			}
			else if (ExponentialDecayRate)
			{
				ExponentialDecay(rate);
			}
		}

		private void LinearDecay(float rate)
		{
			Brightness -= rate;
		}

		private void ExponentialDecay(float rate)
		{
			if (RelativeDecayRate)
			{
				RelativeDecay(rate);
			}
			else if (UniformDecayRate)
			{
				UniformDecay(rate);
			}
		}

		private void RelativeDecay(float rate)
		{
			Brightness -= rate * (1 + ((1 - Brightness) * _decayExponent));
		}

		private void UniformDecay(float rate)
		{
			Brightness -= rate * Mathf.Pow(_decayExponent, onFrames);
		}

		private void UpdateStartingBrightness()
		{
			startingBrightness = Mathf.Clamp(Brightness, .001f, 1);
		}

		private void Recover()
		{
			if (RelativeRecoveryRate)
			{
				RelativeRecovery();
			}
			else if (UniformRecoveryRate)
			{
				UniformRecovery();
			}
		}

		private void RelativeRecovery()
		{
			var scaledStartingBrightness = Mathf.Pow(startingBrightness, 1 / _recoveryExponent);

			// calculate starting x (time) value
			var startingY = scaledStartingBrightness * relativeRefractoryTime;
			var startingX = Pythagoras(relativeRefractoryTime, startingY);

			// calculate y (brightness) value 
			var startTime = startingX;
			var time = offTime - absoluteRefractoryTime;
			var inverseTime = startTime - time; // inverse because we are the top-left quarter of the circle (i.e., going from (-x, 0) -> (0, +y))

			if (inverseTime > 0)
			{
				var y = Pythagoras(relativeRefractoryTime, inverseTime);
				y /= relativeRefractoryTime;
				y = Mathf.Pow(y, _recoveryExponent);

				Brightness = y;
			}
		}

		private void UniformRecovery()
		{
			// calculate starting x (time) value
			var startingY = 0;
			var startingX = Pythagoras(relativeRefractoryTime, startingY);

			// calculate y (brightness) value 
			var startTime = startingX;
			var time = offTime - absoluteRefractoryTime;
			var inverseTime = startTime - time; // inverse because we are the top-left quarter of the circle (i.e., going from (-x, 0) -> (0, +y))

			if (inverseTime > 0)
			{
				var y = Pythagoras(relativeRefractoryTime, inverseTime);
				y /= relativeRefractoryTime;
				y = Mathf.Pow(y, _recoveryExponent);

				Brightness = startingBrightness + y;
			}
		}

		private float Pythagoras(float r, float x)
		{
			return Mathf.Sqrt((r * r) - (x * x));
		}

		[ContextMenu("Set Relative Recovery")]
		private void SetRelativeRecovery_Menu()
		{
			fastDecayRate = .175f;
			slowDecayRate = .00167f;
			_decayRate1 = 1;

			_recoveryRate = 0;
			_recoveryExponent = 1.667f;
		}

		[ContextMenu("Set Uniform Recovery")]
		private void SetUniformRecovery_Menu()
		{
			fastDecayRate = .3f;
			slowDecayRate = .05575f;
			_decayRate1 = 0;

			_recoveryRate = 1;
			_recoveryExponent = 1;
		}
	}
}
