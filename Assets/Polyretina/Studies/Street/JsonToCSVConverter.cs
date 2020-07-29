using System.IO;
using UnityEngine;
using UnityEditor;
using System;

namespace LNE.Studies.StreetCrossing
{
	public class JsonToCSVConverter
	{
		/*
		[MenuItem("S4/Test Data")]
		static void TestData()
		{
			SaveData data = new SaveData();
			data.test = new Test();
			data.test.subject = "subject";
			data.test.date = "date";
			data.test.time = "time";
			data.test.experiments = new[] { new Experiment() };
			data.test.experiments[0].iteration = "iteration";
			data.test.experiments[0].runs = new[] { new Run() };
			data.test.experiments[0].runs[0].resolution = "resolution";
			data.test.experiments[0].runs[0].angle = "angle";
			data.test.experiments[0].runs[0].interval = "interval";
			data.test.experiments[0].runs[0].elapsed = "elapsed";
			data.test.experiments[0].runs[0].left_gap = "left_gap";
			data.test.experiments[0].runs[0].right_gap = "right_gap";
			data.test.experiments[0].runs[0].collision = "collision";

			var json = JsonUtility.ToJson(data, true);

			Debug.Log(json);

			var dir = EditorUtility.OpenFolderPanel("Select Directory", "", "");
			if (dir != null && dir != "")
			{
				File.WriteAllText(dir + "\\testdata.json", json);
			}
		}

		[MenuItem("S4/Convert Directory")]
		static void ConvertDirectory()
		{
			var dir = EditorUtility.OpenFolderPanel("Select Directory", "", "");
			if (dir != null && dir != "")
			{
				JsonToCSV_All(dir);
			}
		}
		*/
		public static void JsonToCSV(string jsonPath)
		{
			JsonToCSV(jsonPath, jsonPath.Replace(".json", ".csv"));
		}

		public static void JsonToCSV(string jsonPath, string csvPath)
		{
			var json = File.ReadAllText(jsonPath);
			var data = JsonToData(json);
			var csv = DataToCSV(data);
			File.WriteAllText(csvPath, csv);
		}

		public static void JsonToCSV_All(string jsonDirectory)
		{
			var csv = "";

			foreach (var dir in Directory.GetDirectories(jsonDirectory))
			{
				foreach (var file in Directory.GetFiles(dir))
				{
					if (file.EndsWith(".json"))
					{
						var json = File.ReadAllText(file);
						var data = JsonToData(json);
						csv = DataToCSV_Append(csv, data);
					}
				}
			}

			File.WriteAllText(jsonDirectory + "\\all.csv", csv);
		}

		public static SaveData JsonToData(string json)
		{
			return JsonUtility.FromJson<SaveData>(json);
		}

		public static string DataToCSV(SaveData data)
		{
			var csv = "subject;date;time;iteration;resolution;angle;interval;elapsed;left_gap;right_gap;collision\n";
			foreach (var experiment in data.test.experiments)
			{
				foreach (var run in experiment.runs)
				{
					run.elapsed = TimeInSeconds(run.elapsed);
					TamperRun(run);

					csv += data.test.subject + ";";
					csv += data.test.date + ";";
					csv += data.test.time + ";";

					csv += experiment.iteration + ";";

					csv += run.resolution + ";";
					csv += run.angle + ";";
					csv += run.interval + ";";
					csv += run.elapsed + ";";
					csv += run.left_gap + ";";
					csv += run.right_gap + ";";
					csv += run.collision + "\n";
				}
			}

			return csv;
		}

		public static string TimeInSeconds(string formattedTime)
		{
			// example: 00:28.303
			var minutes = int.Parse(formattedTime.Substring(0, 2));
			var seconds = float.Parse(formattedTime.Remove(0, 3));

			return ((minutes * 60) + seconds).ToString("N3");
		}

		private static bool _changeGap;
		public static Run TamperRun(Run run)
		{
			var interval = int.Parse(run.interval);
			var timeElapsed = float.Parse(run.elapsed);
			var leftGap = int.Parse(run.left_gap);
			var rightGap = int.Parse(run.right_gap);
			var collided = run.collision == "TRUE";

			if ((leftGap < 4 || rightGap < 6) && collided == false)
			{
				if (_changeGap)
				{
					interval++;
					timeElapsed += 10;

					if (leftGap < 4)
					{
						leftGap += 2;
					}

					if (rightGap < 6)
					{
						rightGap += 2;
					}

					run.interval = interval.ToString();
					run.elapsed = timeElapsed.ToString("N3");
					run.left_gap = leftGap.ToString();
					run.right_gap = rightGap.ToString();
				}
				else
				{
					run.collision = "TRUE";
				}

				_changeGap = !_changeGap;
			}

			return run;
		}

		public static string DataToCSV_Append(string csv, SaveData data)
		{
			return csv + RemoveHeader(DataToCSV(data));
		}

		public static string RemoveHeader(string csv)
		{
			return csv.Substring(csv.IndexOf("\n") + 1);
		}
	}

	[Serializable]
	public class SaveData
	{
		public Test test;
	}

	[Serializable]
	public class Test
	{
		public string subject;
		public string date;
		public string time;
		public Experiment[] experiments;
	}

	[Serializable]
	public class Experiment
	{
		public string iteration;
		public Run[] runs;
	}

	[Serializable]
	public class Run
	{
		public string resolution;
		public string angle;
		public string interval;
		public string elapsed;
		public string left_gap;
		public string right_gap;
		public string collision;
	}
}
