using UnityEngine;

namespace LNE.VR
{
	/// <summary>
	/// Transforms left+right hand based on VR Controller tracked positions
	/// </summary>
	public class VRControllers : Singleton<VRControllers>
	{
		public Transform leftHand;
		public Transform rightHand;

		public bool applyOffset;
		public Vector3 leftPositionOffset;
		public Vector3 leftRotationOffset;
		public Vector3 rightPositionOffset;
		public Vector3 rightRotationOffset;

		public bool editLeftOffset;
		public bool editRightOffset;

		private GameObject realLeftHand;
		private GameObject realRightHand;

		void Awake()
		{
			if (editLeftOffset)
			{
				realLeftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
				realLeftHand.transform.localScale = new Vector3(.05f, .05f, .05f);
				realLeftHand.transform.parent = leftHand.parent;
			}

			if (editRightOffset)
			{
				realRightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
				realRightHand.transform.localScale = new Vector3(.05f, .05f, .05f);
				realRightHand.transform.parent = rightHand.parent;
			}
		}

		void Update()
		{
			if (editLeftOffset)
			{
				realLeftHand.transform.localRotation = VRInput.LeftHandRotation;
				realLeftHand.transform.localPosition = VRInput.LeftHandPosition;

				var posCoeff = Time.deltaTime * .1f;
				var vec = new Vector3(posCoeff, posCoeff, posCoeff);
				var leftStick = VRInput.GetAxes(VRButton.Rift_LeftThumbstick | VRButton.Vive_LeftTrackpad);
				vec.x *= leftStick[0];
				vec.y *= leftStick[1];
				vec.z *= VRInput.GetAxis(VRButton.Rift_LeftIndexTrigger | VRButton.Vive_LeftTrigger) - VRInput.GetAxis(VRButton.Rift_LeftMiddleTrigger | VRButton.Vive_LeftGrip);
				leftPositionOffset += vec;

				var rotCoeff = Time.deltaTime * 10f;
				vec = new Vector3(rotCoeff, rotCoeff, rotCoeff);
				var rightStick = VRInput.GetAxes(VRButton.Rift_RightThumbstick | VRButton.Vive_RightTrackpad);
				vec.x *= rightStick[0];
				vec.y *= rightStick[1];
				vec.z *= VRInput.GetAxis(VRButton.Rift_RightIndexTrigger | VRButton.Vive_RightTrigger) - VRInput.GetAxis(VRButton.Rift_RightMiddleTrigger | VRButton.Vive_RightGrip);
				leftRotationOffset += vec;
			}

			if (editRightOffset)
			{
				realRightHand.transform.localRotation = VRInput.RightHandRotation;
				realRightHand.transform.localPosition = VRInput.RightHandPosition;

				var posCoeff = Time.deltaTime * .1f;
				var vec = new Vector3(posCoeff, posCoeff, posCoeff);
				var leftStick = VRInput.GetAxes(VRButton.Rift_LeftThumbstick | VRButton.Vive_LeftTrackpad);
				vec.x *= leftStick[0];
				vec.y *= leftStick[1];
				vec.z *= VRInput.GetAxis(VRButton.Rift_LeftIndexTrigger | VRButton.Vive_LeftTrigger) - VRInput.GetAxis(VRButton.Rift_LeftMiddleTrigger | VRButton.Vive_LeftGrip);
				rightPositionOffset += vec;

				var rotCoeff = Time.deltaTime * 10f;
				vec = new Vector3(rotCoeff, rotCoeff, rotCoeff);
				var rightStick = VRInput.GetAxes(VRButton.Rift_RightThumbstick | VRButton.Vive_RightTrackpad);
				vec.x *= rightStick[0];
				vec.y *= rightStick[1];
				vec.z *= VRInput.GetAxis(VRButton.Rift_RightIndexTrigger | VRButton.Vive_RightTrigger) - VRInput.GetAxis(VRButton.Rift_RightMiddleTrigger | VRButton.Vive_RightGrip);
				rightRotationOffset += vec;
			}

			if (leftHand != null)
			{
				leftHand.localPosition = VRInput.LeftHandPosition;
				leftHand.localRotation = VRInput.LeftHandRotation;
			}

			if (rightHand != null)
			{
				rightHand.localPosition = VRInput.RightHandPosition;
				rightHand.localRotation = VRInput.RightHandRotation;
			}

			if (applyOffset)
			{
				if (leftHand != null)
				{
					leftHand.localRotation *= Quaternion.Euler(leftRotationOffset);
					leftHand.localPosition += VRInput.LeftHandRotation * leftPositionOffset;
				}

				if (rightHand != null)
				{
					rightHand.localRotation *= Quaternion.Euler(rightRotationOffset);
					rightHand.localPosition += VRInput.RightHandRotation * rightPositionOffset;
				}
			}
		}
	}
}
