using UnityEngine;
using UnityEditor;

namespace LNE.ProstheticVision.UI
{
	using LNE.UI;

	public class CalculatorWindow : EditorWindow
	{
		private HeadsetModel headset;

		private Vector2 retina;
		private Vector2 angle;
		private Vector2 pixel;
		private Vector2 polar;

		private ElectrodePattern pattern = ElectrodePattern.POLYRETINA;
		private ElectrodeLayout layout = ElectrodeLayout._100x150;
		private float fieldOfView = 46.3f;
		private int numElectrodes = 7738;

		[MenuItem("Polyretina/Calculator", priority = WindowPriority.calculatorWindow)]
		static void ShowWindow()
		{
			var window = GetWindow<CalculatorWindow>("Calculator");
			window.minSize = new Vector2(300, 400);
			window.maxSize = new Vector2(300, 400);
			window.Show();
		}

		void OnGUI()
		{
			UnityGUI.Header("Coordinates");
			headset = UnityGUI.Enum("Headset", headset);

			UnityGUI.Space();

			UnityGUI.BeginHorizontal();
			UnityGUI.Label(" ", "X", UnityGUI.BoldLabel);
			UnityGUI.Label("Y", UnityGUI.BoldLabel);
			UnityGUI.EndHorizontal();

			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Resolution", headset.GetWidth().ToString());
			UnityGUI.Label(headset.GetHeight().ToString());
			UnityGUI.EndHorizontal();

			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Field of View", ((int)headset.GetFieldOfView(Axis.Horizontal)).ToString());
			UnityGUI.Label(((int)headset.GetFieldOfView(Axis.Vertical)).ToString());
			UnityGUI.EndHorizontal();

			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Diameter (um)", ((int)headset.GetRetinalDiameter(Axis.Horizontal)).ToString());
			UnityGUI.Label(((int)headset.GetRetinalDiameter(Axis.Vertical)).ToString());
			UnityGUI.EndHorizontal();

			UnityGUI.Space();

			Draw("Pixel", ref pixel);
			Draw("Visual Angle", ref angle);
			Draw("Retina (um)", ref retina);
			Draw("Polar", ref polar);

			UnityGUI.Separator();
			UnityGUI.Header("Electrode Count");

			UnityGUI.BeginChangeCheck();
			pattern = UnityGUI.Enum("Pattern", pattern);
			layout = UnityGUI.Enum("Layout", layout);
			fieldOfView = UnityGUI.Float("Field of View", fieldOfView);
			var changed = UnityGUI.EndChangeCheck();
			if (changed)
			{
				numElectrodes = pattern.GetElectrodePositions(layout, fieldOfView).Length;
			}

			UnityGUI.Space();
			UnityGUI.Label("Electode Count", numElectrodes.ToString());
		}

		private void Draw(string label, ref Vector2 value)
		{
			UnityGUI.BeginChangeCheck();
			value = UnityGUI.Vector2(label, value);
			var changed = UnityGUI.EndChangeCheck();
			if (changed)
			{
				UpdateValues(label);
			}
		}

		private void UpdateValues(string from)
		{
			switch (from)
			{
				case "Retina (um)":
					angle = CoordinateSystem.RetinaToVisualAngle(retina);
					pixel = CoordinateSystem.RetinaToPixel(retina, headset);
					polar = CoordinateSystem.RetinaToPolar(retina);
					break;

				case "Visual Angle":
					retina = CoordinateSystem.VisualAngleToRetina(angle);
					pixel = CoordinateSystem.VisualAngleToPixel(angle, headset);
					polar = CoordinateSystem.VisualAngleToPolar(angle);
					break;

				case "Pixel":
					retina = CoordinateSystem.PixelToRetina((int)pixel.x, (int)pixel.y, headset);
					angle = CoordinateSystem.PixelToVisualAngle((int)pixel.x, (int)pixel.y, headset);
					polar = CoordinateSystem.PixelToPolar((int)pixel.x, (int)pixel.y, headset);
					break;

				case "Polar":
					retina = CoordinateSystem.PolarToRetina(polar);
					angle = CoordinateSystem.PolarToVisualAngle(polar);
					pixel = CoordinateSystem.PolarToPixel(polar, headset);
					break;
			}
		}
	}
}
