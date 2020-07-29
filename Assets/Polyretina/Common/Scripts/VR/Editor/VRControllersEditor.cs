using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace LNE.VR.UI
{
	using LNE.UI;

	[CustomEditor(typeof(VRControllers))]
	public class VRControllersEditor : ExtendedEditor<VRControllers>
	{
		public override void OnInspectorGUI()
		{
			BeginInspectorGUI();

			Field(true, "leftHand");
			Field(true, "rightHand");

			UnityGUI.Space();
			Field(true, "applyOffset");

			if (target.applyOffset)
			{
				Field(true, "leftPositionOffset");
				Field(true, "leftRotationOffset");
				Field(true, "editLeftOffset");

				UnityGUI.Space();
				Field(true, "rightPositionOffset");
				Field(true, "rightRotationOffset");
				Field(true, "editRightOffset");
			}

			UnityGUI.Space(2);

			if (UnityGUI.Foldout(Application.isPlaying, "Controller State"))
			{
				if (XRDevice.model == "Oculus Rift CV1")
				{
					DrawOculus();
				}
                else if (XRDevice.model == "Vive MV" || XRDevice.model == "VIVE_Pro MV")
                {
					DrawVive();
				}

				Repaint();
			}

			EndInspectorGUI();
		}

		private void DrawOculus()
		{
			UnityGUI.Space();
			UnityGUI.Label("Rift_LeftController");
			UnityGUI.IndentLevel++;

			var pos = VRInput.LeftHandPosition;
			var rot = VRInput.LeftHandRotation.eulerAngles;
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position X: " + pos.x.ToString("0.000"));
			UnityGUI.Label("Rotation X: " + rot.x.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Y: " + pos.y.ToString("0.000"));
			UnityGUI.Label("Rotation Y: " + rot.y.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Z: " + pos.z.ToString("0.000"));
			UnityGUI.Label("Rotation Z: " + rot.z.ToString("0.000"));
			UnityGUI.EndHorizontal();

			ButtonStateLabel(VRButton.Rift_X);
			ButtonStateLabel(VRButton.Rift_Y);
			ButtonStateLabel(VRButton.Rift_Start);
			ButtonStateLabel(VRButton.Rift_LeftThumbstick);
			ButtonStateLabel(VRButton.Rift_LeftThumbRest);
			ButtonStateLabel(VRButton.Rift_LeftIndexTrigger);
			AxisStateLabel(VRButton.Rift_LeftIndexTrigger);
			AxisStateLabel(VRButton.Rift_LeftMiddleTrigger);
			AxesStateLabel(VRButton.Rift_LeftThumbstick);
			UnityGUI.IndentLevel--;

			UnityGUI.Space();
			UnityGUI.Label("Rift_RightController");
			UnityGUI.IndentLevel++;

			pos = VRInput.RightHandPosition;
			rot = VRInput.RightHandRotation.eulerAngles;
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position X: " + pos.x.ToString("0.000"));
			UnityGUI.Label("Rotation X: " + rot.x.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Y: " + pos.y.ToString("0.000"));
			UnityGUI.Label("Rotation Y: " + rot.y.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Z: " + pos.z.ToString("0.000"));
			UnityGUI.Label("Rotation Z: " + rot.z.ToString("0.000"));
			UnityGUI.EndHorizontal();

			ButtonStateLabel(VRButton.Rift_A);
			ButtonStateLabel(VRButton.Rift_B);
			ButtonStateLabel(VRButton.Rift_RightThumbstick);
			ButtonStateLabel(VRButton.Rift_RightThumbRest);
			ButtonStateLabel(VRButton.Rift_RightIndexTrigger);
			AxisStateLabel(VRButton.Rift_RightIndexTrigger);
			AxisStateLabel(VRButton.Rift_RightMiddleTrigger);
			AxesStateLabel(VRButton.Rift_RightThumbstick);
			UnityGUI.IndentLevel--;
		}

		private void DrawVive()
		{
			UnityGUI.Space();
			UnityGUI.Label("Vive_LeftController");
			UnityGUI.IndentLevel++;

			var pos = VRInput.LeftHandPosition;
			var rot = VRInput.LeftHandRotation.eulerAngles;
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position X: " + pos.x.ToString("0.000"));
			UnityGUI.Label("Rotation X: " + rot.x.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Y: " + pos.y.ToString("0.000"));
			UnityGUI.Label("Rotation Y: " + rot.y.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Z: " + pos.z.ToString("0.000"));
			UnityGUI.Label("Rotation Z: " + rot.z.ToString("0.000"));
			UnityGUI.EndHorizontal();

			ButtonStateLabel(VRButton.Vive_LeftMenu);
			ButtonStateLabel(VRButton.Vive_LeftTrackpad);
			ButtonStateLabel(VRButton.Vive_LeftTrigger);
			AxisStateLabel(VRButton.Vive_LeftTrigger);
			AxisStateLabel(VRButton.Vive_LeftGrip);
			AxesStateLabel(VRButton.Vive_LeftTrackpad);
			UnityGUI.IndentLevel--;

			UnityGUI.Space();
			UnityGUI.Label("Vive_RightController");
			UnityGUI.IndentLevel++;

			pos = VRInput.RightHandPosition;
			rot = VRInput.RightHandRotation.eulerAngles;
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position X: " + pos.x.ToString("0.000"));
			UnityGUI.Label("Rotation X: " + rot.x.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Y: " + pos.y.ToString("0.000"));
			UnityGUI.Label("Rotation Y: " + rot.y.ToString("0.000"));
			UnityGUI.EndHorizontal();
			UnityGUI.BeginHorizontal();
			UnityGUI.Label("Position Z: " + pos.z.ToString("0.000"));
			UnityGUI.Label("Rotation Z: " + rot.z.ToString("0.000"));
			UnityGUI.EndHorizontal();

			ButtonStateLabel(VRButton.Vive_RightMenu);
			ButtonStateLabel(VRButton.Vive_RightTrackpad);
			ButtonStateLabel(VRButton.Vive_RightTrigger);
			AxisStateLabel(VRButton.Vive_RightTrigger);
			AxisStateLabel(VRButton.Vive_RightGrip);
			AxesStateLabel(VRButton.Vive_RightTrackpad);
			UnityGUI.IndentLevel--;
		}

		private void ButtonStateLabel(VRButton button)
		{
			var state = "Up";
			if (VRInput.GetButton(button))
			{
				state = "Down";
			}
			else if (VRInput.GetTouch(button))
			{
				state = "Touching";
			}
			else if (VRInput.GetHovering(button) > Mathf.Epsilon)
			{
				state = "Hovering";
			}

			UnityGUI.Label(button.ToString() + ": " + state);
		}

		private void AxisStateLabel(VRButton button)
		{
			UnityGUI.Label(button.ToString() + ": " + VRInput.GetAxis(button).ToString("0.000"));
		}

		private void AxesStateLabel(VRButton button)
		{
			var axes = VRInput.GetAxes(button);
			UnityGUI.Label(button.ToString() + " X: " + axes[0].ToString("0.000"));
			UnityGUI.Label(button.ToString() + " Y: " + axes[1].ToString("0.000"));
		}
	}
}
