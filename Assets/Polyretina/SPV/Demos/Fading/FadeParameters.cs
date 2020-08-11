using LNE.ProstheticVision;
using System;
using UnityEngine;

public class FadeParameters : MonoBehaviour
{
	public int parameterId;

	[Space]
	public FrequencyParameters[] parameters;

	private EpiretinalImplant Implant => Prosthesis.Instance.Implant as EpiretinalImplant;

	void Update()
	{
		Implant.UpdateFadingParameters_v2
		(
			parameters[parameterId].t1, 
			parameters[parameterId].t2, 
			parameters[parameterId].threshold, 
			parameters[parameterId].recoveryTime, 
			parameters[parameterId].recoveryExponent
		);
	}
}

[Serializable]
public class FrequencyParameters
{
	public string label;

	public float t1;
	public float t2;
	public float threshold;

	public float recoveryTime;
	public float recoveryExponent;
}
