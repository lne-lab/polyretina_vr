using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LNE.IO;
using LNE.ProstheticVision;
using LNE.StringExts;
using LNE.UI.Attributes;

using static LNE.ArrayExts.ArrayExtensions;

public class FadingStudy2 : MonoBehaviour
{
	[Path]
	public string path;
	public string participant;

	public int startingTrial;
	public int trialsPerCondition;

	//[Space]

	//public Image webcam;

	[Space]

	public AudioSource speakers;
	public AudioClip startSound;
	public AudioClip successSound;
	public AudioClip failedSound;

	[Space]

	public TextAsset _words;
	public int wordLength;
	public int rows;
	public int columns;

	private List<TrialData_Conditions> _trials;
	private TrialData_Conditions _trial;
	private int _seed;
	private int _trialId;
	private bool _trialStarted;
	private Strategy[] _strategies;

	private CSV _csv;

	public int trialId => _trialStarted ? _trialId : -1;

	private string[] randomWords
	{
		get
		{
			var allWords = _words
							.text
							.Split('\n')
							.Apply((word) => word.Trim('\r'))
							.Where((word) => word.Length == wordLength)
							.Randomise(_seed - _trialId);

			return allWords.Subarray(0, 8);
		}
	}

	void Awake()
	{
		_trials = new List<TrialData_Conditions>();
		_seed = participant.AsUid();
		_trialId = startingTrial;
		_trialStarted = false;

		var strategyCombos = new[] 
		{
			Strategy.None,
			Strategy.Saccade,
			Strategy.Random,
			Strategy.Interrupt,
			Strategy.Saccade | Strategy.Random,
			Strategy.Saccade | Strategy.Interrupt,
			Strategy.Random | Strategy.Interrupt,
			Strategy.Saccade | Strategy.Random | Strategy.Interrupt,
			Strategy.None | Strategy.EyeTracking,
			Strategy.None | Strategy.NoFading
		};

		_strategies = CreateArray(strategyCombos, trialsPerCondition, (s) => s).Randomise(_seed);

		_csv = new CSV();
		_csv.AppendRow("participant", "trial", "strategy", "start time", "end time", "success");

		//webcam.enabled = false;
		HideWords();
	}

	void OnApplicationQuit()
	{
		//SaveAsJson(fileId);
		SaveAsCsv();
	}

	//private void SaveAsJson(long id)
	//{
	//	var data = new TestData_Conditions { trials = _trials.ToArray() };
	//	var json = JsonUtility.ToJson(data, true);

	//	File.WriteAllText(
	//		path + $"Fading_Conditions_{participant}_json_{id}.json", json
	//	);
	//}

	private void SaveAsCsv()
	{
		var csv = new CSV();
		csv.AppendRow("participant", "trial", "strategy", "start time", "end time", "success");

		foreach (var trial in _trials)
		{
			csv.AppendRow(
				trial.participant,
				trial.trialId,
				trial.strategy,
				trial.startTime,
				trial.endtime,
				trial.numCorrect
			);
		}

		csv.SaveWStream(path + $"Fad_Cnd_Res_All_{participant}.csv");
	}

	public void StartTrial()
	{
		// safety check
		if (_trialStarted)
			return;

		_trialStarted = true;

		// create trial
		_trial = new TrialData_Conditions
		{
			participant = participant,
			trialId = _trialId,
			strategy = _strategies[_trialId],
			startTime = Time.time
		};

		// misc stuff
		SetStrategyParameters();
		//webcam.enabled = true;
		ShowWords();
		speakers.PlayOneShot(startSound);

		Debug.Log($"Trial {_trialId} starting. Strategy: {_trial.strategy}");
	}

	public void EndTrial(int numCorrect)
	{
		// safety check
		if (_trialStarted == false)
			return;

		_trialStarted = false;

		// add trial
		_trial.endtime = Time.time;
		_trial.numCorrect = numCorrect;
		_trials.Add(_trial);

		// misc stuff
		//speakers.PlayOneShot(success ? successSound : failedSound);
		speakers.PlayOneShot(startSound);
		//webcam.enabled = false;
		HideWords();

		// save data
		_csv.AppendRow(_trial.participant, _trial.trialId, _trial.strategy, _trial.startTime, _trial.endtime, _trial.numCorrect);
		_csv.SaveWStream(path + $"Fad_Cnd_Res_{_trialId}_{participant}.csv");
		FindObjectOfType<RotationRecorder>().Save(_trialId);

		Debug.Log($"Trial {_trialId} ending. Participant got {numCorrect} out of 8 correct.");

		// incr
		_trialId++;

		if (_trialId >= _strategies.Length)
		{
			Application.Quit();
		}
	}

	private void SetStrategyParameters()
	{
		var simulator = Prosthesis.Instance.ExternalProcessor as SaccadeSimulator;

		simulator.saccadeType	= _trial.strategy.HasFlag(Strategy.Saccade) 
								? SaccadeSimulator.SaccadeType.Left 
								: SaccadeSimulator.SaccadeType.None;

		simulator.offTime	= _trial.strategy.HasFlag(Strategy.Interrupt)
							? 0.6f
							: 0.0f;

		var fadeParams = FindObjectOfType<FadeParameters>();
		switch (_trial.strategy)
		{
			case Strategy.None:
				fadeParams.parameters._5HzT1 = 0.1f;
				fadeParams.parameters._5HzT2 = 0.3f;
				break;
			case Strategy.Saccade:
				fadeParams.parameters._5HzT1 = 0.175f;
				fadeParams.parameters._5HzT2 = 0.525f;
				break;
			case Strategy.Random:
				fadeParams.parameters._5HzT1 = 0.2f;
				fadeParams.parameters._5HzT2 = 0.6f;
				break;
			case Strategy.Interrupt:
				fadeParams.parameters._5HzT1 = 0.1f;
				fadeParams.parameters._5HzT2 = 0.3f;
				break;
			case Strategy.Saccade | Strategy.Random:
				fadeParams.parameters._5HzT1 = 0.175f;
				fadeParams.parameters._5HzT2 = 0.525f;
				break;
			case Strategy.Saccade | Strategy.Interrupt:
				fadeParams.parameters._5HzT1 = 0.275f;
				fadeParams.parameters._5HzT2 = 0.825f;
				break;
			case Strategy.Random | Strategy.Interrupt:
				fadeParams.parameters._5HzT1 = 0.8f;
				fadeParams.parameters._5HzT2 = 2.4f;
				break;
			case Strategy.Saccade | Strategy.Random | Strategy.Interrupt:
				fadeParams.parameters._5HzT1 = 0.55f;
				fadeParams.parameters._5HzT2 = 0.165f;
				break;
		}

		// eye tracking
		var implant = Prosthesis.Instance.Implant as EpiretinalImplant;
		implant.eyeGazeSource = _trial.strategy.HasFlag(Strategy.EyeTracking) ?
								EyeGaze.Source.EyeTracking :
								EyeGaze.Source.None;

		// no fading
		implant.useFading = !_trial.strategy.HasFlag(Strategy.NoFading);
	}

	private void ShowWords()
	{
		var words = randomWords;
		var text = "";

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				text += words[(i * columns) + j] + " ";
			}

			text += "\n";
		}

		FindObjectOfType<Text>().text = text;

		Debug.Log("<b>" + text.Replace("\n", "") + "</b>");
	}

	private void HideWords()
	{
		FindObjectOfType<Text>().text = "";
	}
}
