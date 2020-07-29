using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace LNE.VR
{
	[Flags]
	public enum VRButton
	{
		None = 0,
		
		Rift_A = 1,
		Rift_B = 2,
		Rift_X = 4,
		Rift_Y = 8,
		Rift_Start = 16,
		Rift_LeftIndexTrigger = 32,
		Rift_RightIndexTrigger = 64,
		Rift_LeftMiddleTrigger = 128,
		Rift_RightMiddleTrigger = 256,
		Rift_LeftThumbstick = 512,
		Rift_RightThumbstick = 1024,
		Rift_LeftThumbRest = 2048,
		Rift_RightThumbRest = 4096,

		Vive_LeftMenu = 8192,
		Vive_RightMenu = 16384,
		Vive_LeftTrackpad = 32768,
		Vive_RightTrackpad = 65536,
		Vive_LeftTrigger = 131072,
		Vive_RightTrigger = 262144,
		Vive_LeftGrip = 524288,
		Vive_RightGrip = 1048576
	}

	/// <summary>
	/// Agnostic VR Input for Vive and Oculus
	/// </summary>
	public static class VRInput
	{
		/*
		 * Private fields
		 */

		private readonly static Dictionary<VRButton, string> pressables = new Dictionary<VRButton, string>
		{
			{ VRButton.Rift_A, "joystick button 0" },
			{ VRButton.Rift_B, "joystick button 1" },
			{ VRButton.Rift_X, "joystick button 2" },
			{ VRButton.Rift_Y, "joystick button 3" },
			{ VRButton.Rift_Start, "joystick button 7" },
			{ VRButton.Rift_LeftThumbstick, "joystick button 8" },
			{ VRButton.Rift_RightThumbstick, "joystick button 9" },

			{ VRButton.Vive_LeftMenu, "joystick button 2" },
			{ VRButton.Vive_RightMenu, "joystick button 0" },
			{ VRButton.Vive_LeftTrackpad, "joystick button 8" },
			{ VRButton.Vive_RightTrackpad, "joystick button 9" }
		};

		private readonly static Dictionary<VRButton, string> touchables = new Dictionary<VRButton, string>
		{
			{ VRButton.Rift_A, "joystick button 10" },
			{ VRButton.Rift_B, "joystick button 11" },
			{ VRButton.Rift_X, "joystick button 12" },
			{ VRButton.Rift_Y, "joystick button 13" },
			{ VRButton.Rift_LeftThumbstick, "joystick button 16" },
			{ VRButton.Rift_RightThumbstick, "joystick button 17" },
			{ VRButton.Rift_LeftThumbRest, "joystick button 18" },
			{ VRButton.Rift_RightThumbRest, "joystick button 19" },
			{ VRButton.Rift_LeftIndexTrigger, "joystick button 14" },
			{ VRButton.Rift_RightIndexTrigger, "joystick button 15" },

			{ VRButton.Vive_LeftTrackpad, "joystick button 16" },
			{ VRButton.Vive_RightTrackpad, "joystick button 17" },
			{ VRButton.Vive_LeftTrigger, "joystick button 14" },
			{ VRButton.Vive_RightTrigger, "joystick button 15" }
		};

		private readonly static Dictionary<VRButton, string> hoverables = new Dictionary<VRButton, string>
		{
			{ VRButton.Rift_LeftThumbstick, "Left Rift Thumbstick Hover" },
			{ VRButton.Rift_LeftThumbRest, "Left Rift Thumb Rest Hover" },
			{ VRButton.Rift_LeftIndexTrigger, "Left Rift Index Trigger Hover" },
			{ VRButton.Rift_RightThumbstick, "Right Rift Thumbstick Hover" },
			{ VRButton.Rift_RightThumbRest, "Right Rift Thumb Rest Hover" },
			{ VRButton.Rift_RightIndexTrigger, "Right Rift Index Trigger Hover" }
		};

		private readonly static Dictionary<VRButton, string> axes = new Dictionary<VRButton, string>
		{
			{ VRButton.Rift_LeftIndexTrigger, "Left Rift Index Trigger" },
			{ VRButton.Rift_LeftMiddleTrigger, "Left Rift Middle Trigger" },
			{ VRButton.Rift_RightIndexTrigger, "Right Rift Index Trigger" },
			{ VRButton.Rift_RightMiddleTrigger, "Right Rift Middle Trigger" },

			{ VRButton.Vive_LeftTrigger, "Left Vive Trigger" },
			{ VRButton.Vive_RightTrigger, "Right Vive Trigger" },
			{ VRButton.Vive_LeftGrip, "Left Vive Grip" },
			{ VRButton.Vive_RightGrip, "Right Vive Grip" }
		};

		private readonly static Dictionary<VRButton, (string, string)> axes2d = new Dictionary<VRButton, (string, string)>
		{
			{ VRButton.Rift_LeftThumbstick, ("Left Rift Thumbstick X Axis", "Left Rift Thumbstick Y Axis") },
			{ VRButton.Rift_RightThumbstick, ("Right Rift Thumbstick X Axis", "Right Rift Thumbstick Y Axis") },
			{ VRButton.Vive_LeftTrackpad, ("Left Vive Trackpad X Axis", "Left Vive Trackpad Y Axis") },
			{ VRButton.Vive_RightTrackpad, ("Right Vive Trackpad X Axis", "Right Vive Trackpad Y Axis") }
		};

		private static Dictionary<VRButton, float> axisValues = new Dictionary<VRButton, float>()
		{
			{ VRButton.Rift_LeftIndexTrigger, 0 },
			{ VRButton.Rift_LeftMiddleTrigger, 0 },
			{ VRButton.Rift_RightIndexTrigger, 0 },
			{ VRButton.Rift_RightMiddleTrigger, 0 },

			{ VRButton.Vive_LeftTrigger, 0 },
			{ VRButton.Vive_RightTrigger, 0 },
			{ VRButton.Vive_LeftGrip, 0 },
			{ VRButton.Vive_RightGrip, 0 }
		};

		/*
		 * Public properties
		 */

		public static Vector3 LeftHandPosition
		{
			get
			{
#if UNITY_2019_2_OR_NEWER
				InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
				return position;
#else
				return InputTracking.GetLocalPosition(XRNode.LeftHand);
#endif
			}
		}
		public static Vector3 RightHandPosition
		{
			get
			{
#if UNITY_2019_2_OR_NEWER
				InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
				return position;
#else
				return InputTracking.GetLocalPosition(XRNode.RightHand);
#endif
			}
		}

		public static Quaternion LeftHandRotation
		{
			get
			{
#if UNITY_2019_2_OR_NEWER
				InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
				return rotation;
#else
				return InputTracking.GetLocalRotation(XRNode.LeftHand);
#endif
			}
		}

		public static Quaternion RightHandRotation
		{
			get
			{
#if UNITY_2019_2_OR_NEWER
				InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
				return rotation;
#else
				return InputTracking.GetLocalRotation(XRNode.RightHand);
#endif
			}
		}

		public static Ray LeftHandRay { get => new Ray(LeftHandPosition, LeftHandRotation.eulerAngles); }

		public static Ray RightHandRay { get => new Ray(RightHandPosition, RightHandRotation.eulerAngles); }

		/*
		 * Public methods
		 */

		public static bool GetButtonDown(VRButton button)
		{
			return pressables.ContainsKey(button) ? Input.GetKeyDown(pressables[button]) : false;
		}

		public static bool GetButton(VRButton button)
		{
			return pressables.ContainsKey(button) ? Input.GetKey(pressables[button]) : false;
		}

		public static bool GetButtonUp(VRButton button)
		{
			return pressables.ContainsKey(button) ? Input.GetKeyUp(pressables[button]) : false;
		}

		public static bool GetTouchDown(VRButton button)
		{
			return touchables.ContainsKey(button) ? Input.GetKeyDown(touchables[button]) : false;
		}
		
		public static bool GetTouch(VRButton button)
		{
			return touchables.ContainsKey(button) ? Input.GetKey(touchables[button]) : false;
		}

		public static bool GetTouchUp(VRButton button)
		{
			return touchables.ContainsKey(button) ? Input.GetKeyUp(touchables[button]) : false;
		}

		public static float GetHovering(VRButton button)
		{
			return hoverables.ContainsKey(button) ? Input.GetAxis(hoverables[button]) : 0;
		}
		
		public static float GetAxis(VRButton button)
		{
			return GetAxis(button, Mathf.Epsilon);
		}

		public static float GetAxis(VRButton button, float dead)
		{
			var axis = axes.ContainsKey(button) ? Input.GetAxis(axes[button]) : 0;
			return Mathf.Abs(axis) > dead ? axis : 0;
		}

		public static Vector2 GetAxes(VRButton button)
		{
			return GetAxes(button, Mathf.Epsilon);
		}

		public static Vector2 GetAxes(VRButton button, float dead)
		{
			var contains = axes2d.ContainsKey(button);
			if (contains)
			{
				var x = Input.GetAxis(axes2d[button].Item1);
				var y = Input.GetAxis(axes2d[button].Item1);

				var axes = new Vector2(
					Mathf.Abs(x) > dead ? x : 0,
					Mathf.Abs(y) > dead ? y : 0
				);

				return axes;
			}
			else
			{
				return Vector2.zero;
			}
		}

		public static bool GetAxisAsButtonDown(VRButton button)
		{
			var contains = axes.ContainsKey(button);
			if (contains)
			{
				var oldVal = axisValues[button];
				var newVal = Input.GetAxis(axes[button]);

				axisValues[button] = newVal;

				if (newVal == 1 && oldVal != 1)
				{
					return true;
				}
			}

			return false;
		}

		public static bool GetAxisAsButton(VRButton button)
		{
			return GetAxis(button) == 1;
		}

		public static bool GetAxisAsButtonUp(VRButton button)
		{
			var contains = axes.ContainsKey(button);
			if (contains)
			{
				var oldVal = axisValues[button];
				var newVal = Input.GetAxis(axes[button]);

				axisValues[button] = newVal;

				if (newVal != 1 && oldVal == 1)
				{
					return true;
				}
			}

			return false;
		}
	}
}
