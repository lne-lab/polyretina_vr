using System;
using System.Threading;
using UnityEngine;
using UnityEditor;

namespace LNE.UI
{
	public class SettingsWindow : EditorWindow
	{
		private string helpMessage;

		[MenuItem("Polyretina/Settings", priority = WindowPriority.settingsWindow)]
		static void ShowWindow()
		{
			var window = GetWindow<SettingsWindow>("Settings");
			window.minSize = new Vector2(300, 400);
			window.maxSize = new Vector2(300, 400);
			window.Show();
		}

		void OnGUI()
		{
			helpMessage = "";

			UnityGUI.Header("Paths", true);
			Settings.QuitForInvalidPaths = UnityGUI.Toggle("Quit For Invalid Paths", Settings.QuitForInvalidPaths);
			if (UnityGUI.OnMouseHoverPrevious())
			{
				helpMessage = "Application will quit on initialisation if any fields that are using the Path Attribute have invalid paths.";
			}

			UnityGUI.Header("VR", true);
			UnityGUI.IndentLevel--;
			UnityGUI.Label("Eye Tracking SDKs");
			UnityGUI.IndentLevel++;
			Settings.FoveSupport = UnityGUI.Toggle("FOVE", Settings.FoveSupport);
			if (UnityGUI.OnMouseHoverPrevious())
			{
				helpMessage = "Enable FOVE eye tracking. Will produce errors if the FOVE Unity Package has not been imported.";
			}

			Settings.ViveProEyeSupport = UnityGUI.Toggle("Vive Pro Eye", Settings.ViveProEyeSupport);
			if (UnityGUI.OnMouseHoverPrevious())
			{
				helpMessage = "Enable Vive Pro Eye eye tracking. Will produce errors if the SRanipal Unity Package has not been imported.";
			}

			UnityGUI.Space();

			Settings.VRInputSupport = UnityGUI.Toggle("VRInput Support", Settings.VRInputSupport);
			if (UnityGUI.OnMouseHoverPrevious())
			{
				helpMessage = "Add VRInput support to the Input Manager. Recommended if using the Vive Pro Eye.";
			}

			UnityGUI.BeginHorizontal();
			Settings.DelayPlay = UnityGUI.Float("Delay", Settings.DelayPlay);
			if (UnityGUI.OnMouseHoverPrevious())
			{
				helpMessage = "Delay play for some seconds after clicking the play button. Useful when testing in VR.";
			}

			if (UnityGUI.Button("Play"))
			{
				Thread.Sleep(TimeSpan.FromSeconds(Settings.DelayPlay));
				EditorApplication.isPlaying = true;
			}
			UnityGUI.EndHorizontal();

			UnityGUI.Header("Prosthetic Vision", true);
			Settings.SaveRuntimeChangesAutomatically = UnityGUI.Toggle("Save Runtime Changes", Settings.SaveRuntimeChangesAutomatically);
			if (UnityGUI.OnMouseHoverPrevious())
			{
				helpMessage = "Save runtime changes made to ImageRenderers automatically.";
			}

			UnityGUI.IndentLevel--;
			UnityGUI.FlexibleSpace();
			UnityGUI.HelpLabel(helpMessage, MessageType.Info);
		}

		void OnInspectorUpdate()
		{
			Repaint();
		}
	}
}
