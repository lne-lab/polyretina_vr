using LNE.ProstheticVision;
using System;
using UnityEngine;

using LNE.UI.Attributes;

public class FadeParameters : MonoBehaviour
{
	public enum Frequency { NoPulse = 0, _1 = 1, _5 = 5, _10 = 10, _20 = 20 }

	[Space]
	public Frequency frequency;
	public FrequencyParameters parameters;

	private EpiretinalImplant Implant => Prosthesis.Instance.Implant as EpiretinalImplant;

	void Update()
	{
		Implant.offFrames = frequency > 0 ? (100 / (int)frequency) - 1 : 0;

		switch (frequency)
		{
			case Frequency.NoPulse:
				break;
			case Frequency._1:
				Implant.UpdateDecayParameters(
					parameters._1HzT1, 
					parameters._1HzT2, 
					parameters._1HzThreshold
				);
				break;
			case Frequency._5:
				Implant.UpdateDecayParameters(
					parameters._5HzT1, 
					parameters._5HzT2, 
					parameters._5HzThreshold
				);
				break;
			case Frequency._10:
				Implant.UpdateDecayParameters(
					parameters._10HzT1, 
					parameters._10HzT2, 
					parameters._10HzThreshold
				);
				break;
			case Frequency._20:
				Implant.UpdateDecayParameters(
					parameters._20HzT1, 
					parameters._20HzT2, 
					parameters._20HzThreshold
				);
				break;
		}

		Implant.UpdateRecoveryParameters(parameters.recoveryDelay, parameters.recoveryTime, parameters.recoveryExponent);
	}
}

[Serializable]
public class FrequencyParameters
{
	[Header("1Hz")]
	[CustomLabel(label = "τ1")]
	public float _1HzT1;
	[CustomLabel(label = "τ2")]
	public float _1HzT2;
	[CustomLabel(label = "Threshold")]
	public float _1HzThreshold;

	[Header("5Hz")]
	[CustomLabel(label = "τ1")]
	public float _5HzT1;
	[CustomLabel(label = "τ2")]
	public float _5HzT2;
	[CustomLabel(label = "Threshold")]
	public float _5HzThreshold;

	[Header("10Hz")]
	[CustomLabel(label = "τ1")]
	public float _10HzT1;
	[CustomLabel(label = "τ2")]
	public float _10HzT2;
	[CustomLabel(label = "Threshold")]
	public float _10HzThreshold;

	[Header("20Hz")]
	[CustomLabel(label = "τ1")]
	public float _20HzT1;
	[CustomLabel(label = "τ2")]
	public float _20HzT2;
	[CustomLabel(label = "Threshold")]
	public float _20HzThreshold;

	[Header("Recovery")]
	[CustomLabel(label = "Delay")]
	public float recoveryDelay;
	[CustomLabel(label = "Time")]
	public float recoveryTime;
	[CustomLabel(label = "Exponent")]
	public float recoveryExponent;
}
