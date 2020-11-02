using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LNE.UI.Attributes;

public class ConditionReconstruction : MonoBehaviour
{
	[SerializeField, Path(isFile = true)]
	private string path;

	private TrialData_Conditions[] trials;
	private TrialData_Conditions trial;

	void Start()
	{
		var json = File.ReadAllText(path);
		trials = JsonUtility.FromJson<TestData_Conditions>(json).trials;
	}

	void OnGUI()
	{
		GUILayout.Space(25);
		//GUILayout.Label("        / " + (trial.endtime - trial.startTime).ToString("N2"));

		GUILayout.Space(25);
		GUILayout.Label(trial.strategy.ToString());
	}

	public void SetCondition(int trialId)
	{
		trial = trials[trialId];
	}
}
