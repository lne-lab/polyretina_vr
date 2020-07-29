using System;
using UnityEngine;
using UnityEditor;

namespace LNE.ProstheticVision.UI
{
	using Threading;
	using LNE.UI;

	using DataType = PreprocessedDataFactory.DataType;

	public class PreprocessedDataFactoryWindow : EditorWindow
	{
		private DataType _dataType;
		private HeadsetModel _headset;
		private ElectrodePattern _pattern;
		private ElectrodeLayout _layout;
		private string _path;
		private bool _gpuAccel;

		private bool cpuStarted;
		private ThreadGroup threadGroup;
		private DateTime startTime;

		[MenuItem("Polyretina/Preprocess Data", priority = WindowPriority.preprocessedDataFactoryWindow)]
		static void ShowWindow()
		{
			GetWindow<PreprocessedDataFactoryWindow>("Preprocess Data").Show();
		}

		void OnEnable()
		{
			_dataType = DataType.Phosphene;
			_headset = HeadsetModel.VivePro;
			_pattern = ElectrodePattern.POLYRETINA;
			_layout = ElectrodeLayout._80x150;
			_path = "Assets/";
			_gpuAccel = true;

			cpuStarted = false;
		}

		void OnGUI()
		{
			UnityGUI.Space();
			_dataType = UnityGUI.Enum("Data Type", _dataType);

			UnityGUI.Space();
			UnityGUI.Label("Parameters", UnityGUI.BoldLabel);

			_headset = UnityGUI.Enum("Headset Model", _headset);
			if (_dataType == DataType.Phosphene)
			{
				_pattern = UnityGUI.Enum("Electrode Pattern", _pattern);
				_layout = UnityGUI.Enum("Electrode Layout", _layout);
			}

			UnityGUI.Space();
			_gpuAccel = UnityGUI.ToggleLeft("GPU Accelerated", _gpuAccel);

			if (UnityGUI.Button(!cpuStarted ? "Start" : "Stop", new GUIOptions { maxWidth = 50 }))
				{
				if (!cpuStarted)
					Start();
				else
					StopCPU();
			}

			if (threadGroup != null)
			{
				UnityGUI.Enabled = cpuStarted || !_gpuAccel;
				UnityGUI.Space();
				UnityGUI.Label("CPU Threads", UnityGUI.BoldLabel);

				for (int i = 0; i < threadGroup.NumThreads; i++)
				{
					UnityGUI.Label("Thread " + i.ToString(), (threadGroup.Progress[i] * 100).ToString("N0") + "%");
				}
			}
		}

		void OnInspectorUpdate()
		{
			if (cpuStarted)
			{
				Repaint();
			}
		}

		void OnDestroy()
		{
			StopCPU();
		}

		private void Start()
		{
			// substring to remove proceeding "_"
			var filename = _dataType == DataType.Phosphene ? UnityApp.ToDisplayFormat(_layout) : "axons";
			_path = EditorUtility.SaveFilePanelInProject("Save data texture...", filename, "asset", "");

			if (_path == "")
			{
				return;
			}

			StartTimer();

			threadGroup = PreprocessedDataFactory.Create(_dataType, _headset, _pattern, _layout, _gpuAccel, _path);
			if (_gpuAccel)
			{
				LogTimer();
			}
			else
			{
				threadGroup.OnAllThreadsFinished += LogTimer;
				threadGroup.OnAllThreadsFinished += () => { cpuStarted = false; };
			}

			cpuStarted = !_gpuAccel;
		}

		private void StopCPU()
		{
			threadGroup?.Abort();
			cpuStarted = false;
		}

		private void StartTimer()
		{
			startTime = DateTime.Now;
		}

		private void LogTimer()
		{
			var deltaTime = DateTime.Now - startTime;
			var timeTaken = deltaTime.TotalSeconds;

			var log = _dataType.ToString() + " | ";

			log += _headset.ToString() + " | ";

			if (_dataType == DataType.Phosphene)
			{
				log += _pattern.ToString() + " | ";
				log += _layout.ToString() + " | ";
			}

			log += timeTaken.ToString("n2") + "s" + (_gpuAccel ? " (gpu)" : " (cpu)");

			Debug.Log(log);
		}
	}
}
