using System.IO;
using UnityEngine;
using UnityEditor;

namespace LNE.Studies
{
	public class ResultsConverterWindow : EditorWindow
	{
		/*
		[MenuItem("Results/Convert", priority = 0)]
		static void Convert()
		{
			JsonToCsv();
		}

		[MenuItem("Results/Convert All", priority = 0)]
		static void ConvertAll()
		{
			var path = EditorUtility.OpenFolderPanel("Select folder...", Application.dataPath, "");
			var files = Directory.GetFiles(path);
			foreach (var file in files)
			{
				if (file.EndsWith(".json"))
				{
					JsonToCsv(file, file.Replace(".json", ".csv"));
				}
			}
		}
		*/
		private static void JsonToCsv()
		{
			var loadPath = EditorUtility.OpenFilePanel("Select Json data file...", Application.dataPath, "json");
			var savePath = EditorUtility.SaveFilePanel("Save csv data file...", Application.dataPath, "data", "csv");

			JsonToCsv(loadPath, savePath);
		}
		
		private static void JsonToCsv(string loadPath, string savePath)
		{
			var json = File.ReadAllText(loadPath);
			var data = JsonToStudyData(json);
			var csv = StudyDataToCsv(data);

			File.WriteAllText(savePath, csv);
		}

		private static StudyData JsonToStudyData(string json)
		{
			return JsonUtility.FromJson<StudyData>(json);
		}

		private static string StudyDataToCsv(StudyData data)
		{
			var width = 11;
			var height = 1 + data.exposures.Length;
			var matrix = new string[width, height];

			// header
			matrix[0, 0] = "participant";
			matrix[1, 0] = "date";
			matrix[2, 0] = "startTime";
			matrix[4, 0] = "electrodeLayout";
			matrix[5, 0] = "fieldOfView";
			matrix[6, 0] = "visualAngle";
			matrix[7, 0] = "tailLength";
			matrix[8, 0] = "itemName";
			matrix[9, 0] = "result";
			matrix[10, 0] = "elapsedTime";

			// data
			for (int i = 0; i < data.exposures.Length; i++)
			{
				matrix[0, 1 + i] = data.identifier.ToString();
				matrix[1, 1 + i] = data.date;
				matrix[2, 1 + i] = data.startTime;
				matrix[4, 1 + i] = data.exposures[i].electrodeLayout;
				matrix[5, 1 + i] = data.exposures[i].fieldOfView.ToString();
				matrix[6, 1 + i] = data.exposures[i].visualAngle.ToString();
				matrix[7, 1 + i] = data.exposures[i].tailLength.ToString();
				matrix[8, 1 + i] = data.exposures[i].itemName;
				matrix[9, 1 + i] = data.exposures[i].result.ToString();
				matrix[10, 1 + i] = data.exposures[i].elapsedTime.ToString();
			}

			// to csv
			var csv = "";
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					csv += matrix[j, i] + ";";
				}

				csv += "eof\n";
			}

			return csv;
		}
	}
}
